using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Management;

namespace HLTrader
{
    enum TransMode
    {
        HighLow = 1,
        HighLowSpread = 2,
    }

    enum DemoReal
    {
        Demo = 0,
        Real = 1,
    }

    enum Timeframe
    {
        _NONE = 0,
        _30_SECONDS = 30,
        _1_MINUTE = 60,
        _3_MINUTES = 180,
        _5_MINUTES = 300,
        _15_MINUTE = 900,
        _1_HOUR = 3600,
        _1_DAY = 86400,
    }

    enum HighLow
    {
        High = 1,
        Low = 2,
    }

    enum EntryTiming
    {
        Recent = 1,
        Medium = 2,
        Late = 3,
    };

    enum Signal
    {
        None = -1,
        Buy = 0,
        Sell = 1,
    }

    enum CopiesStatus
    {
        None = 0,
        Pending = 1,
        Completed = 2,
        Sold = 3,
    };

    struct HighLowSymbol
    {
        public string id;
        public string name;
        public string closing_time;
        public Timeframe timeframe;
        public string payout_rate;
    }

    struct TradeSymbol
    {
        public int index;
        public string symbol;
        public TransMode trans_mode;
        public Timeframe timeframe;
        public bool is_trading;
    }

    struct ClientInfo
    {
        public string symbol;
    }

    struct StopTime
    {
        public string strBeginDay;
        public string strBeginTime;
        public string strEndDay;
        public string strEndTime;
    }

    struct TradeHistory
    {
        public int id;
        public int signal_id;
        public int trader_id;
        public string symbol;
        public DemoReal demo_real;
        public Signal cmd;
        public TransMode trans_mode;
        public int trans_timeunit;
        public string trans_suffix;
        public decimal order_price;
        public decimal judge_price;
        public decimal order_amount;
        public decimal payout_amount;
        public CopiesStatus status;
        public string ordered_at;
        public string judged_at;
    }

    struct LoginResponse
    {
        public int result;
        public int error;
    }

    struct TradeHistoryResponse
    {
        public int result;
        public int error;
        public List<TradeHistory> detail;
    }

    struct TradeSignal
    {
        public int id;
        public string source;
        public string symbol;
        public Signal cmd;
        public string created_at;
    }

    struct AddHistoryResponse
    {
        public int signal_id;
        public int history_id;
    }

    class Constant
    {
        public static string APPLICATION_NAME = "HLTrader";
        public static string APPLICATION_GUID = "2020AFA5-E915-47DA-AAAA-2ABC22261992";

        public static string BACKOFFICE_IP = "153.127.70.45";
        public static int BACKOFFICE_PORT = 8484;

        public static string REAL_SITE_URL = "https://highlow.com/login";
        public static string DEMO_SITE_URL = "https://trade.highlow.com/";
        public static string API_URL_GET_TODAY = "http://54.238.208.47/public/kkh/today";

        public static int ADD_UTC_HOUR = 9;

        // Urls
        public static string API_MAIN_URL = "http://153.127.70.45";
        //public static string API_MAIN_URL = "http://localhost:8000";
        public static string API_LOGIN_URL = API_MAIN_URL + "/api/login";
        public static string API_GET_TRADE_HISTORY_URL = API_MAIN_URL + "/api/getTradeHistory";

        public static int API_RESULT_SUCCESS = 0;
        public static int API_RESULT_FAILURE = 1;

        // Login
        public static int API_ERROR_NONE = 0;
        public static int API_ERROR_RUNTIME = 1;
        public static int API_ERROR_CANT_CONNECT_SERVER = 2;
        public static int API_ERROR_INVALID_USER = 11;
        public static int API_ERROR_INVALID_PASSWORD = 12;
        public static int API_ERROR_BANNED_USER = 13;
        public static int API_ERROR_ALREADY_LOGINNED = 14;

        // Error Messages
        public static Dictionary<int, string> g_strErrorMsg = new Dictionary<int, string>()
        {
            { API_ERROR_NONE, "" },
            { API_ERROR_RUNTIME, "実行中エラー発生" },
            { API_ERROR_CANT_CONNECT_SERVER, "認証サーバー接続失敗" },
            { API_ERROR_INVALID_USER, "アカウント情報を正確に入力してください。" },
            { API_ERROR_INVALID_PASSWORD, "パスワードが違います。" },
            { API_ERROR_BANNED_USER, "停止されたアカウントです。" },
            { API_ERROR_ALREADY_LOGINNED, "すでにログインされました。" },
        };
    }

    class API
    {
        public static CHttpCommon api_request = new CHttpCommon();

        public static LoginResponse doLogin()
        {
            LoginResponse result = new LoginResponse();
            result.result = Constant.API_RESULT_FAILURE;
            result.error = Constant.API_ERROR_RUNTIME;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "login_id", CGlobalVar.g_strLoginID },
                    { "password", CGlobalVar.g_strLoginPass },
                };
                var param = CGlobalVar.EncodeURIComponent(parameters);

                string url = Constant.API_LOGIN_URL;
                api_request.setURL(url);
                api_request.setSendMode(HTTP_SEND_MODE.HTTP_POST);

                if (!api_request.sendRequest(false, param))
                {
                    result.error = Constant.API_ERROR_CANT_CONNECT_SERVER;
                    return result;
                }

                string json = api_request.getResponseString();
                result = JsonConvert.DeserializeObject<LoginResponse>(json);

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public static TradeHistoryResponse doGetTradeHistory()
        {
            TradeHistoryResponse result = new TradeHistoryResponse();
            result.result = Constant.API_RESULT_FAILURE;
            result.error = Constant.API_ERROR_RUNTIME;
            result.detail = new List<TradeHistory>();

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "login_id", CGlobalVar.g_strLoginID },
                };
                var param = CGlobalVar.EncodeURIComponent(parameters);

                string url = Constant.API_GET_TRADE_HISTORY_URL;
                api_request.setURL(url);
                api_request.setSendMode(HTTP_SEND_MODE.HTTP_POST);

                if (!api_request.sendRequest(false, param))
                {
                    return result;
                }

                string json = api_request.getResponseString();
                result = JsonConvert.DeserializeObject<TradeHistoryResponse>(json);

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }

    public class IniControl
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, String lpFileName);

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string cipherText)
        {
            byte[] data = System.Convert.FromBase64String(cipherText);
            return System.Text.UTF8Encoding.UTF8.GetString(data);
        }


        public static void WriteIntValue(string lpAppName, string lpKeyName, int nValue, string lpFileName)
        {
            WritePrivateProfileString(lpAppName, lpKeyName, nValue.ToString(), lpFileName);
        }

        public static void WriteBoolValue(string lpAppName, string lpKeyName, bool bValue, string lpFileName)
        {
            WritePrivateProfileString(lpAppName, lpKeyName, bValue ? "1" : "0", lpFileName);
        }

        public static void WriteStringValue(string lpAppName, string lpKeyName, string lpValue, string lpFileName, bool bForce = false)
        {
            if (bForce)
                WritePrivateProfileString(lpAppName, lpKeyName, lpValue, lpFileName);
            else
                WritePrivateProfileString(lpAppName, lpKeyName, Base64Encode(lpValue), lpFileName);
        }

        public static string GetStringValue(string lpAppName, string lpKeyName, string lpDefault, int nSize, string lpFileName, bool bForce = false)
        {
            StringBuilder sbBuffer = new StringBuilder(nSize);

            GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, sbBuffer, nSize, lpFileName);
            if (bForce) return sbBuffer.ToString();

            return Base64Decode(sbBuffer.ToString());
        }

        public static int GetIntValue(string lpAppName, string lpKeyName, int nDefault, string lpFileName)
        {
            return GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, lpFileName);
        }

        public static bool GetBoolValue(string lpAppName, string lpKeyName, int nDefault, string lpFileName)
        {
            int nValue = GetPrivateProfileInt(lpAppName, lpKeyName, nDefault, lpFileName);

            return nValue == 1 ? true : false;
        }

        public static double GetDoubleValue(string lpAppName, string lpKeyName, double dDefault, string lpFileName)
        {
            double dResult = 0;

            int nSize = 0x10;
            StringBuilder sbBuffer = new StringBuilder(nSize);

            string lpDefault = dDefault.ToString();

            GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, sbBuffer, nSize, lpFileName);

            Double.TryParse(sbBuffer.ToString(), out dResult);

            return dResult;
        }
    }

    class CGlobalVar
    {
        public static string CONFIG_FILE_NAME = "HLTrader.ini";
        public static string TRADE_HISTORY_FILE_NAME = "HLTradeHistory.dat";

        public static string g_strConfigPath;
        public static string g_strTradeHistoryPath;
        public static string g_strWinRatePath;

        // Config Variables
        // Config - Login Info
        public static bool g_nAdmin = false;
        public static string g_strLoginID = "";
        public static string g_strLoginPass = "";
        public static bool g_bAutoLogin = false;

        // Config - Trade Setting
        public static TransMode g_nTransMode = TransMode.HighLow;
        public static DemoReal g_nDemoReal = DemoReal.Demo;
        public static int g_nOrderAmount = 1000;
        public static int g_nRetryInterval = 0;
        public static int g_nRetryCount = 0;
        public static int g_nDelayTime = 100;

        public static List<bool> g_lnWeekdays = new List<bool>();
        public static List<StopTime> g_lstStopTimes = new List<StopTime>();

        // Trade History
        public static List<TradeHistory> g_lstTradeHistory = new List<TradeHistory>();

        public static List<TradeSymbol> g_lstTradeSymbols = new List<TradeSymbol>();
        public static List<string> g_lstSymbolList = new List<string>()
        {
            "USD/JPY",
            "AUD/JPY",
            "AUD/USD",
            "NZD/JPY",
            "NZD/USD",
            "AUD/NZD",
            "EUR/JPY",
            "EUR/USD",
            "EUR/AUD",
            "EUR/GBP",
            "GBP/JPY",
            "GBP/USD",
            "GBP/AUD",
            "USD/CAD",
            "CAD/JPY",
            "USD/CHF",
            "CHF/JPY",
            "GOLD",
        };

        public static void ReadConfig()
        {
            g_strConfigPath = Path.Combine(Application.StartupPath, CONFIG_FILE_NAME);

            g_strLoginID = IniControl.GetStringValue("Login", "LoginID", "", 100, g_strConfigPath);
            g_strLoginPass = IniControl.GetStringValue("Login", "LoginPass", "", 100, g_strConfigPath);
            g_bAutoLogin = IniControl.GetBoolValue("Login", "AutoLogin", 0, g_strConfigPath);

            g_nTransMode = (TransMode)IniControl.GetIntValue("MainSetting", "TransMode", (int)TransMode.HighLow, g_strConfigPath);
            g_nDemoReal = (DemoReal)IniControl.GetIntValue("MainSetting", "DemoReal", (int)DemoReal.Demo, g_strConfigPath);
            g_nOrderAmount = IniControl.GetIntValue("MainSetting", "OrderAmount", 1000, g_strConfigPath);
            g_nRetryInterval = IniControl.GetIntValue("MainSetting", "RetryInterval", 100, g_strConfigPath);
            g_nRetryCount = IniControl.GetIntValue("MainSetting", "RetryCount", 3, g_strConfigPath);
            g_nDelayTime = IniControl.GetIntValue("MainSetting", "DelayTime", 100, g_strConfigPath);

            for (int i = 0; i < 7; i ++)
            {
                bool val = IniControl.GetBoolValue("StopTime", "Weekday" + i.ToString(), 0, g_strConfigPath);
                g_lnWeekdays.Add(val);
            }

            int nCount = IniControl.GetIntValue("StopTime", "Count", 0, g_strConfigPath);
            for (int i = 1; i <= nCount; i ++)
            {
                StopTime st;
                st.strBeginDay = IniControl.GetStringValue("StopTime", "BeginDay" + i.ToString(), "", 64, g_strConfigPath);
                st.strBeginTime = IniControl.GetStringValue("StopTime", "BeginTime" + i.ToString(), "", 64, g_strConfigPath);
                st.strEndDay = IniControl.GetStringValue("StopTime", "EndDay" + i.ToString(), "", 64, g_strConfigPath);
                st.strEndTime = IniControl.GetStringValue("StopTime", "EndTime" + i.ToString(), "", 64, g_strConfigPath);
                g_lstStopTimes.Add(st);
            }
        }

        public static void WriteConfig()
        {
            IniControl.WriteStringValue("Login", "LoginID", g_strLoginID, g_strConfigPath);
            IniControl.WriteStringValue("Login", "LoginPass", g_strLoginPass, g_strConfigPath);
            IniControl.WriteBoolValue("Login", "AutoLogin", g_bAutoLogin, g_strConfigPath);

            IniControl.WriteIntValue("MainSetting", "TransMode", (int)g_nTransMode, g_strConfigPath);
            IniControl.WriteIntValue("MainSetting", "DemoReal", (int)g_nDemoReal, g_strConfigPath);
            IniControl.WriteIntValue("MainSetting", "OrderAmount", g_nOrderAmount, g_strConfigPath);
            IniControl.WriteIntValue("MainSetting", "RetryInterval", g_nRetryInterval, g_strConfigPath);
            IniControl.WriteIntValue("MainSetting", "RetryCount", g_nRetryCount, g_strConfigPath);
            IniControl.WriteIntValue("MainSetting", "DelayTime", g_nDelayTime, g_strConfigPath);

            for (int i = 0; i < 7; i ++)
            {
                IniControl.WriteBoolValue("StopTime", "Weekday" + i.ToString(), g_lnWeekdays[i], g_strConfigPath);
            }
            IniControl.WriteIntValue("StopTime", "Count", g_lstStopTimes.Count, g_strConfigPath);
            for (int i = 1; i <= g_lstStopTimes.Count; i ++)
            {
                StopTime st = g_lstStopTimes[i - 1];
                IniControl.WriteStringValue("StopTime", "BeginDay" + i.ToString(), st.strBeginDay, g_strConfigPath);
                IniControl.WriteStringValue("StopTime", "BeginTime" + i.ToString(), st.strBeginTime, g_strConfigPath);
                IniControl.WriteStringValue("StopTime", "EndDay" + i.ToString(), st.strEndDay, g_strConfigPath);
                IniControl.WriteStringValue("StopTime", "EndTime" + i.ToString(), st.strEndTime, g_strConfigPath);
            }
        }

        public static string EncodeURIComponent(Dictionary<string, object> rgData)
        {
            string _result = String.Join("&", rgData.Select((x) => String.Format("{0}={1}", x.Key, x.Value)));

            _result = System.Net.WebUtility.UrlEncode(_result)
                        .Replace("+", "%20").Replace("%21", "!")
                        .Replace("%27", "'").Replace("%28", "(")
                        .Replace("%29", ")").Replace("%26", "&")
                        .Replace("%3D", "=").Replace("%7E", "~");
            return _result;
        }

        public static Timeframe StrToTimeframe(string str)
        {
            switch (str)
            {
                case "15分": return Timeframe._15_MINUTE;
                case "1時間": return Timeframe._1_HOUR;
                case "1日": return Timeframe._1_DAY;
                case "30秒": return Timeframe._30_SECONDS;
                case "1分": return Timeframe._1_MINUTE;
                case "3分": return Timeframe._3_MINUTES;
                case "5分": return Timeframe._5_MINUTES;
            }

            return Timeframe._NONE;
        }
    }
}
