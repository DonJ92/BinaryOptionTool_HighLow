using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HLTrader
{
    public enum HTTP_SEND_MODE
    {
        HTTP_GET,
        HTTP_POST
    }

    public class CHttpCommon
    {
        private HttpWebRequest m_Request = null;
        private string m_strResponse = "";
        private string m_strResponseHeader = "";
        private string m_strResponseUri = "";
        private string m_strRequestUri = "";

        private string m_strSiteName = "";

        public delegate void setRequestHeaders(CHttpCommon httpCommon);

        private setRequestHeaders _setRequestHeaders = null;
        private CookieContainer m_cookieContainer = null;

        private ArrayList m_arCustomHeaders = new ArrayList();

        public CHttpCommon(string siteName = "")
        {
            m_strSiteName = siteName;
            m_cookieContainer = null;
        }

        private void initialize()
        {
            m_strResponse = "";
            m_strResponseHeader = "";
            m_strRequestUri = "";
            m_strResponseUri = "";
        }

        public void setURL(string strURL)
        {
            if (m_Request != null)
                m_Request = null;

            if (strURL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                m_Request = WebRequest.Create(strURL) as HttpWebRequest;
                m_Request.ProtocolVersion = HttpVersion.Version11;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; // 20151124_RSG SSl->Tls
            }
            else
            {
                m_Request = (HttpWebRequest)WebRequest.Create(strURL);
            }
        }

        public void setSendMode(HTTP_SEND_MODE mode)
        {
            if (null == m_Request)
                return;
            m_Request.Method = mode == HTTP_SEND_MODE.HTTP_GET ? "GET" : "POST";
        }

        public void setCookieContainer(CookieContainer container)
        {
            if (container == null)
                m_cookieContainer = new CookieContainer();
            else
                m_cookieContainer = container;
        }

        public void setRequestHeadersCallback(setRequestHeaders headerFunc)
        {
            _setRequestHeaders += new setRequestHeaders(headerFunc);
        }

        public void setAccept(string strAccept)
        {
            if (null == m_Request)
                return;

            m_Request.Accept = strAccept;
        }

        public void setUserAgent(string strUserAgent)
        {
            if (null == m_Request)
                return;

            m_Request.UserAgent = strUserAgent;
        }

        public void setReferer(string strReferer)
        {
            if (null == m_Request)
                return;

            m_Request.Referer = strReferer;
        }

        public void setAutoRedirect(bool autoRedirect)
        {
            if (null == m_Request)
                return;
            m_Request.AllowAutoRedirect = autoRedirect;
        }

        public string getResponseString()
        {
            return m_strResponse;
        }

        public string getResponseHeader()
        {
            return m_strResponseHeader;
        }

        public string getResponseUri()
        {
            return m_strResponseUri;
        }

        public string getRequestUri()
        {
            return m_strRequestUri;
        }

        public bool sendRequest(bool bAsync = false, string parameter = "")
        {
            initialize();

            if (true == bAsync)
                return sendAsyncRequest(parameter);

            return sendSyncRequest(parameter);
        }

        private bool sendSyncRequest(string parameter)
        {
            if (null == m_Request)
                return false;

            if (_setRequestHeaders != null)
            {
                //m_Request.Headers.Clear();
                _setRequestHeaders(this);
            }

            string sErrorHeader = "";
            try
            {
                foreach (string sHeader in m_arCustomHeaders)
                {
                    if (sHeader.Contains("Content-Type") == true)
                    {
                        int nDotPos = sHeader.IndexOf(':');
                        string sContentType = sHeader.Substring(nDotPos + 1);
                        m_Request.ContentType = sContentType;
                        continue;
                    }

                    sErrorHeader = sHeader;
                    m_Request.Headers.Add(sHeader);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            m_Request.KeepAlive = true;
            m_Request.ServicePoint.Expect100Continue = false; // remove Expect header

            if (m_cookieContainer == null)
                m_cookieContainer = new CookieContainer();

            m_Request.CookieContainer = m_cookieContainer;

            m_strRequestUri = m_Request.RequestUri.AbsoluteUri;

            if (parameter != "")
            {
                m_Request.ServicePoint.Expect100Continue = true; // remove Expect header

                //byte[] data = UTF8Encoding.UTF8.GetBytes(parameter);
                byte[] data = Encoding.ASCII.GetBytes(parameter);
                m_Request.ContentType = "application/x-www-form-urlencoded";
                m_Request.ContentLength = data.Length;

                try
                {
                    using (Stream reqStream = m_Request.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                        reqStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    closeConnection();
                    return false;
                }
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)m_Request.GetResponse();

                for (int i = 0; i < response.Cookies.Count; i++)
                {
                    //Cookie resp_cookie = response.Cookies[i];
                    //Cookie cookie = new Cookie(resp_cookie.Name, resp_cookie.Value, resp_cookie.Path);

                    m_cookieContainer.Add(new Uri(m_strRequestUri), response.Cookies[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                closeConnection();
                return false;
            }

            Stream responseStream;
            StreamReader reader;

            try
            {
                responseStream = response.GetResponseStream();

                if (response.ContentEncoding.ToLower().Contains("gzip"))
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                Encoding encoding = Encoding.GetEncoding("utf-8");
                reader = new StreamReader(responseStream, encoding);

                m_strResponse = reader.ReadToEnd();

                reader.Close();
                responseStream.Close();
                response.Close();

                m_strResponseHeader = "";
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    m_strResponseHeader += response.Headers.Keys.Get(i) + ":" + response.Headers[i] + "\n";
                }

                m_strResponseUri = response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                closeConnection();
                return false;
            }

            m_Request.Abort();
            m_Request = null;
            return true;
        }

        private void closeConnection()
        {
            if (m_Request != null)
            {
                m_Request.Abort();
                m_Request = null;
            }

            m_arCustomHeaders.Clear();
        }

        public bool addCookie(string name, string value, string path = "/")
        {
            if (m_cookieContainer == null)
                return false;

            Cookie cookie = new Cookie(name, value, path);

            string uri = "";
            if (m_Request != null)
                uri = m_Request.RequestUri.ToString();

            if (uri != "")
                m_cookieContainer.Add(new Uri(uri), cookie);
            else
                m_cookieContainer.Add(cookie);
            return true;
        }

        public bool addCookie(CookieCollection cookies)
        {
            m_cookieContainer.Add(cookies);
            return true;
        }

        public Cookie getCookie(string key)
        {
            if (m_cookieContainer == null)
                return null;

            CookieCollection cookieCollection = m_cookieContainer.GetCookies(new Uri(getRequestUri()));
            for (int i = 0; i < cookieCollection.Count; i++)
                if (cookieCollection[i].Name == key)
                    return cookieCollection[i];
            return null;
        }

        private bool sendAsyncRequest(string parameter)
        {
            return true;
        }

        public void appendCustomHeader(string strHeader)
        {
            if (m_arCustomHeaders.IndexOf(strHeader) < 0)
                m_arCustomHeaders.Add(strHeader);
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
