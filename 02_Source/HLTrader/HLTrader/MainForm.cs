using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Configuration;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.IO;

namespace HLTrader
{
    public partial class MainForm : Form
    {
        private static readonly object syncLock = new object();
        private static readonly object syncOrder = new object();

        private bool m_bAllowTrade = true;
        private bool m_bTestMode = false;
        private CHttpCommon http_request = new CHttpCommon();

        private bool m_bTrading = false;
        private bool m_bOrdering = false;
        private bool m_bLoadFinished = false;
        private string m_strCurrency = "¥";

        private string m_strToday = "";
        private int m_nCurrentOrderCount = 0;
        private int m_nOriginTradeHistoryCount = 0;

        private HighLowSymbol m_stSelSymbol = new HighLowSymbol();
        private Dictionary<string, int> m_dicClientIndex = new Dictionary<string, int>();

        private Socket socket;
        private int m_nUserID = 0;
        private List<int> m_lnProcessedSignals = new List<int>();
        private bool m_bBackOfficeConnected = false;
        private object m_lkConnect = new Object();

        private Thread thread_server = null;
        private Thread thread_start = null;
        private Thread thread_refresh = null;
        private List<ClientInfo> m_stClientInfo = new List<ClientInfo>();
        private List<HighLowSymbol> m_lstHighLowSymbols = new List<HighLowSymbol>();

        delegate void AddLogCallBack(string log);
        delegate void SetControlEnableCallBack(Control ctrl, bool enable);
        delegate void SetControlTextCallBack(Control ctrl, string text);
        delegate string GetControlTextCallBack(Control ctrl);
        delegate void AddListItemCallBack(Control ctrl, string text);
        delegate void AddTradeHistoryCallBack(TradeHistory history);
        delegate void UpdateTradeHistoryCallBack(int index, TradeHistory history);
        delegate void ClearTradeHistoryCallBack();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!CheckToday())
            {
                MessageBox.Show("You cannot use this program!");
                this.Close();
            }

            CGlobalVar.ReadConfig();

            LoginForm dlg = new LoginForm();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                this.Close();
            }
            AddLog("Login has succeeded!");

            InitInterface();
            ConnectBackOffice();

            thread_server = new Thread(new ThreadStart(StartServer));
            thread_server.IsBackground = true;
            thread_server.Start();
        }

        private bool CheckToday()
        {
            try
            {
                return true;

                string url = Constant.API_URL_GET_TODAY;
                http_request.setURL(url);
                http_request.setSendMode(HTTP_SEND_MODE.HTTP_GET);

                if (!http_request.sendRequest(false))
                {
                    return false;
                }

                string response = http_request.getResponseString();
                if (response.CompareTo("2020-12-31") >= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void InitInterface()
        {
            m_strToday = DateTime.Now.ToString("yyyy-MM-dd");

            this.Text = Constant.APPLICATION_NAME + " v" + Properties.Resources.APP_VERSION + "(" + Properties.Resources.APP_RELEASE + ")";
        }

        private void ConnectBackOffice()
        {
            lock (m_lkConnect)
            {
                AddLog("Connecting to BackOffice...");

                IO.Options opts = new IO.Options() { IgnoreServerCertificateValidation = true, AutoConnect = true, ForceNew = true };
                //opts.Transports = (new List<string>() { "websocket" }).ToImmutableList<string>();
                opts.Path = "/1.0.0";
                opts.Query = new Dictionary<string, string>();
                opts.Query.Add("USER", CGlobalVar.g_strLoginID);

                string strServerURL = string.Format("http://{0}:{1}", Constant.BACKOFFICE_IP, Constant.BACKOFFICE_PORT);
                this.socket = null;
                this.socket = IO.Socket(strServerURL, opts);

                // Receive messages
                this.socket.On("Response:Login", (jo) =>
                {
                    this.ResponseLogin(jo.ToString());
                });

                this.socket.On("Response:Get:Trade:Info", (jo) =>
                {
                    this.ResponseGetTradeInfo(jo.ToString());
                });

                this.socket.On("Response:Get:Trade:History", (jo) =>
                {
                    this.ResponseGetTradeHistory(jo.ToString());
                });

                this.socket.On("Response:Add:Trade:History", (jo) =>
                {
                    this.ResponseAddTradeHistory(jo.ToString());
                });

                this.socket.On("Response:Check:Account", (jo) =>
                {
                    this.RecvCheckAccount(jo.ToString());
                });

                this.socket.On("Request:New:Order", (jo) =>
                {
                    this.RecvNewOrder(jo.ToString());
                });

                // SocketIO events
                this.socket.On(Socket.EVENT_CONNECT, () =>
                {
                    // Connect has established
                    m_bBackOfficeConnected = true;
                    AddLog("Connected to BackOffice!");
                });

                this.socket.On(Socket.EVENT_DISCONNECT, () =>
                {
                    m_bBackOfficeConnected = false;
                    AddLog("Disconnected from BackOffice!");
                });

                this.socket.On(Socket.EVENT_ERROR, (kk) =>
                {
                    AddLog(kk.ToString());
                });
            }
        }

        private void ResponseLogin(string strJson)
        {
            AddLog("ResponseLogin : " + strJson);
            socket.Emit("Request:Get:Trade:Info", CGlobalVar.g_strLoginID);
            SetControlEnable(btnStartTrade, true);
        }

        private void ResponseGetTradeInfo(string strJson)
        {
            int.TryParse(strJson, out m_nUserID);
            AddLog("ResponseUserInfo : " + m_nUserID.ToString());
            socket.Emit("Request:Get:Trade:History", m_nUserID);
        }

        private void ResponseGetTradeHistory(string strJson)
        {
            List<TradeHistory> detail = JsonConvert.DeserializeObject<List<TradeHistory>>(strJson);

            lsvTradeHistory.Items.Clear();
            m_nOriginTradeHistoryCount = detail.Count;
            for (int i = 0; i < detail.Count; i++)
            {
                TradeHistory history = detail[i];
                AddTradeHistory(history);
                CGlobalVar.g_lstTradeHistory.Add(history);
            }
            AddLog("Received your trade history! Count = " + m_nOriginTradeHistoryCount.ToString());
            AddLog("");
        }

        private void ResponseAddTradeHistory(string strJson)
        {
            AddHistoryResponse response = JsonConvert.DeserializeObject<AddHistoryResponse>(strJson);

            for (int i = 0; i < CGlobalVar.g_lstTradeHistory.Count; i ++)
            {
                TradeHistory history = CGlobalVar.g_lstTradeHistory[i];
                if (history.signal_id == response.signal_id)
                {
                    history.id = response.history_id;
                    CGlobalVar.g_lstTradeHistory[i] = history;
                    break;
                }
            }
        }

        private void RecvNewOrder(string strJson)
        {
            TradeSignal signal = JsonConvert.DeserializeObject<TradeSignal>(strJson);
            
            if (m_lnProcessedSignals.Contains(signal.id))
            {
                return;
            }

            m_lnProcessedSignals.Add(signal.id);
            AddLog(">>>>>>>>>>>>> New Signal！ Source = " + signal.source + ", Symbol = " + signal.symbol + ", Cmd = " + signal.cmd.ToString());
            signal.symbol = signal.symbol.Replace("/", "");

            int index = FindTradeSymbol(signal.symbol);
            if (index >= 0)
            {
                DateTime dtOrderTime;
                DateTime.TryParse(signal.created_at, out dtOrderTime);

                if (!m_bTrading || !m_bLoadFinished)
                {
                    AddLog("Initializing has not finished yet. Ignore new signals.");
                }
                else
                {
                    SendOrder(CGlobalVar.g_lstTradeSymbols[index], CGlobalVar.g_nOrderAmount.ToString(), signal, dtOrderTime);
                }
            }
        }

        private void RecvCheckAccount(string json)
        {
            int status = 0;
            int.TryParse(json, out status);

            if (status == 0)
            {
                if (m_bAllowTrade == true)
                {
                    AddLog("Your account has blocked!");
                }
                m_bAllowTrade = false;
            }
            else if (status == 1)
            {
                if (m_bAllowTrade == false)
                {
                    AddLog("Your account has activated!");
                }
                m_bAllowTrade = true;
            }
        }

        private void CheckAccount()
        {
            if (CGlobalVar.g_nAdmin || socket == null || !m_bBackOfficeConnected)
            {
                return;
            }

            socket.Emit("Request:Check:Account", m_nUserID);
        }

        private void SendTradeHistory(TradeHistory history)
        {
            if (CGlobalVar.g_nAdmin || socket == null || !m_bBackOfficeConnected)
            {
                return;
            }

            string strJson = JsonConvert.SerializeObject(history);

            socket.Emit("Request:Add:Trade:History", strJson);
        }

        private void SendTradeResult(TradeHistory history)
        {
            if (CGlobalVar.g_nAdmin || socket == null || !m_bBackOfficeConnected)
            {
                return;
            }

            string strJson = JsonConvert.SerializeObject(history);
            AddLog("SendTradeResult : " + strJson);

            socket.Emit("Request:Update:Trade:History", strJson);
        }

        public int FindTradeSymbol(string symbol)
        {
            for (int i = 0; i < CGlobalVar.g_lstTradeSymbols.Count; i ++)
            {
                TradeSymbol tsymbol = CGlobalVar.g_lstTradeSymbols[i];
                if (tsymbol.symbol.Replace("/", "") == symbol)
                {
                    return i;
                }
            }

            return -1;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CGlobalVar.WriteConfig();
        }

        private void AddTradeHistory(TradeHistory history)
        {
            if (lsvTradeHistory.InvokeRequired)
            {
                AddTradeHistoryCallBack d = new AddTradeHistoryCallBack(AddTradeHistory);
                this.Invoke(d, new object[] { history });
            }
            else
            {
                string[] items = new string[11];
                items[0] = (lsvTradeHistory.Items.Count + 1).ToString();
                items[1] = (history.demo_real == DemoReal.Demo) ? "デモー" : "レアル";
                items[2] = (history.trans_mode == TransMode.HighLow) ? "HighLow" : "HighLow-Spread";
                items[3] = history.symbol;
                items[4] = (history.cmd == Signal.Buy ? "↑" : "↓") + history.order_price.ToString("G29");
                items[5] = history.ordered_at;
                items[6] = history.judged_at;
                items[7] = GetCopiesStatusStr(history.status);
                items[8] = (history.judge_price == 0) ? "-" : history.judge_price.ToString("G29");
                items[9] = history.order_amount.ToString("G29");
                items[10] = history.payout_amount.ToString("G29");

                ListViewItem item = new ListViewItem(items);
                item.Name = items[0];
                item.BackColor = GetCopiesBackColor(history);
                lsvTradeHistory.Items.Add(item);
            }
        }

        private void UpdateTradeHistory(int index, TradeHistory history)
        {
            if (lsvTradeHistory.InvokeRequired)
            {
                UpdateTradeHistoryCallBack d = new UpdateTradeHistoryCallBack(UpdateTradeHistory);
                this.Invoke(d, new object[] { index, history });
            }
            else
            {
                ListViewItem item = lsvTradeHistory.Items[index];

                item.BackColor = GetCopiesBackColor(history);

                item.SubItems[7].Text = GetCopiesStatusStr(history.status);
                item.SubItems[8].Text = (history.judge_price == 0) ? "-" : history.judge_price.ToString();
                item.SubItems[9].Text = history.order_amount.ToString();
                item.SubItems[10].Text = history.payout_amount.ToString();
            }
        }

        private void SetControlEnable(Control ctrl, bool enable)
        {
            if (ctrl.InvokeRequired)
            {
                SetControlEnableCallBack d = new SetControlEnableCallBack(SetControlEnable);
                this.Invoke(d, new object[] { ctrl, enable });
            }
            else
            {
                ctrl.Enabled = enable;
            }
        }

        private string GetCopiesStatusStr(CopiesStatus status)
        {
            switch (status)
            {
                case CopiesStatus.None:
                    return "";
                case CopiesStatus.Pending:
                    return "取引中";
                case CopiesStatus.Completed:
                    return "取引完了";
                case CopiesStatus.Sold:
                    return "転売";
            }

            return "";
        }

        private Color GetCopiesBackColor(TradeHistory history)
        {
            switch (history.status)
            {
                case CopiesStatus.None:
                    return Color.White;
                case CopiesStatus.Pending:
                    return Color.LightBlue;
                case CopiesStatus.Completed:
                case CopiesStatus.Sold:
                    if (history.order_amount < history.payout_amount)
                        return Color.LightGreen;
                    else
                        return Color.LightPink;
            }

            return Color.White;
        }

        private void StartServer()
        {
            while (true)
            {
                try
                {
                    string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SetControlText(lblNowTime, strNow);

                    if (!CGlobalVar.g_nAdmin) CheckAccount();
                }
                catch (Exception e)
                {
                    string strErrMsg = "Runtime Server Error : " + e.ToString();
                    AddLog(strErrMsg);
                }

                Thread.Sleep(100);
            }
        }

        private void AddTextLog(string log)
        {
            string strFull = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss:fff] ");

            if (txtLogs.InvokeRequired)
            {
                AddLogCallBack d = new AddLogCallBack(AddTextLog);
                this.Invoke(d, new object[] { log });
            }
            else
            {
                txtLogs.Text += strFull + log;
                txtLogs.SelectionStart = txtLogs.Text.Length;
                txtLogs.ScrollToCaret();
            }
        }

        private void AddLog(string log)
        {
            string strDate = DateTime.Now.ToString("yyyyMMdd");
            string strFull = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss:fff] ");

            AddTextLog(log + "\r\n");

            var string_buffer = new StringBuilder();
            string_buffer.Append(strFull + log);
            try
            {
                using (StreamWriter sw = File.AppendText("Logs/" + strDate + ".log"))
                {
                    sw.WriteLine(string_buffer.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                AddTextLog("WriteLog Error " + ex.Message + "\n");
            }
        }

        private void SetControlText(Control ctrl, string text)
        {
            if (ctrl == null) return;
            if (ctrl.InvokeRequired)
            {
                SetControlTextCallBack d = new SetControlTextCallBack(SetControlText);
                this.Invoke(d, new object[] { ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        private string GetControlText(Control ctrl)
        {
            if (ctrl == null) return "";
            if (ctrl.InvokeRequired)
            {
                GetControlTextCallBack d = new GetControlTextCallBack(GetControlText);
                return (string)this.Invoke(d, new object[] { ctrl });
            }
            else
            {
                return ctrl.Text;
            }
        }

        private void AddListItem(Control ctrl, string item)
        {
            if (ctrl == null) return;
            if (ctrl.InvokeRequired)
            {
                AddListItemCallBack d = new AddListItemCallBack(AddListItem);
                this.Invoke(d, new object[] { ctrl, item });
            }
            else
            {
                ((ListBox)ctrl).Items.Add(item);
            }
        }


        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            SetControlText(txtLogs, "");
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            MainSetting setting = new MainSetting();

            if (setting.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStartTrade_Click(object sender, EventArgs e)
        {
            if (m_bTrading)
            {
                m_bTrading = false;
                WebBrowserFacade.Close();
                EnableSettingControls(true);

                btnStartTrade.Text = "取引開始";
            }
            else
            {
                m_bTrading = true;
                btnStartTrade.Text = "取引停止";
                EnableSettingControls(false);

                // Add Symbols
                CGlobalVar.g_lstTradeSymbols.Clear();
                for (int i = 0; i < CGlobalVar.g_lstSymbolList.Count; i++)
                {
                    TradeSymbol symbol = new TradeSymbol();
                    symbol.index = i;
                    symbol.symbol = CGlobalVar.g_lstSymbolList[i];
                    symbol.is_trading = false;
                    symbol.trans_mode = CGlobalVar.g_nTransMode;
                    symbol.timeframe = Timeframe._15_MINUTE;
                    CGlobalVar.g_lstTradeSymbols.Add(symbol);
                }

                m_bLoadFinished = false;
                thread_start = new Thread(new ThreadStart(StartTrading));
                thread_start.IsBackground = true;
                thread_start.Start();

                thread_refresh = new Thread(new ThreadStart(RefreshSite));
                thread_refresh.IsBackground = true;
                thread_refresh.Start();
            }
        }

        private void EnableSettingControls(bool bEnable)
        {
            btnSetting.Enabled = bEnable;
        }

        private void StartTrading()
        {
            try
            {
                string script = "";

                AddLog("Initializing...");

                // Navigate URL
                WebBrowserFacade.init("chrome");

                if (CGlobalVar.g_nDemoReal == DemoReal.Demo)
                {
                    WebBrowserFacade.Goto(Constant.DEMO_SITE_URL);
                    Thread.Sleep(3000);

                    IWebElement btnQuick = WebBrowserFacade.getDriver.FindElement(By.ClassName("highlight"));
                    if (btnQuick != null)
                    {
                        script = "document.getElementsByClassName('highlight')[0].click()";
                        ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);
                        Thread.Sleep(5000);
                    }

                    CloseCampaign();
                }
                else if (CGlobalVar.g_nDemoReal == DemoReal.Real)
                {
                    WebBrowserFacade.Goto(Constant.REAL_SITE_URL);
                    Thread.Sleep(3000);

                    // Login
                    /*script = "document.getElementById('login-username').value='" + CGlobalVar.g_strLoginID + "'";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);

                    script = "document.getElementById('login-password').value='" + CGlobalVar.g_strPassword + "'";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);

                    script = "document.getElementsByClassName('btn-highlight')[0].click()";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);*/

                    while (true)
                    {
                        try
                        {
                            IWebElement txtUserName = WebBrowserFacade.getDriver.FindElement(By.Id("login-username"));
                            Thread.Sleep(1000);
                        }
                        catch (Exception ex_login)
                        {
                            break;
                        }
                    }
                }

                // Select Trade Type
                int nType = (CGlobalVar.g_nTransMode == TransMode.HighLow) ? 0 : 1;
                script = "document.getElementsByClassName('gameTab')[" + nType.ToString() + "].click()";
                ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);
                Thread.Sleep(2000);

                AddLog("Getting symbol lists..");

                // Get Symbol List
                m_lstHighLowSymbols.Clear();
                int nSuccess = 0;
                for (int i = 0; i < CGlobalVar.g_lstSymbolList.Count; i++)
                {
                    string symbol = CGlobalVar.g_lstSymbolList[i];
                    if (GetSymbolList(symbol))
                    {
                        nSuccess++;
                        AddLog(" >> " + symbol + " : OK");
                    }
                    else
                    {
                        AddLog(" >> " + symbol + " : Failed");
                    }
                }

                if (nSuccess == 0)
                {
                    AddLog("Initialize has failed!");

                    m_bTrading = false;
                    WebBrowserFacade.Close();
                    EnableSettingControls(true);

                    btnStartTrade.Text = "取引開始";

                    return;
                }

                Thread.Sleep(2000);

                AddLog("Initializ has finished! Waiting for new signals!");
                AddLog("===================================================");
                m_bLoadFinished = true;

                if (m_bTestMode)
                {
                    Thread.Sleep(5000);
                    TradeSymbol _tsymbol = new TradeSymbol();
                    _tsymbol.index = 0;
                    _tsymbol.symbol = "USD/JPY";
                    _tsymbol.timeframe = Timeframe._15_MINUTE;
                    _tsymbol.trans_mode = TransMode.HighLow;
                    _tsymbol.is_trading = false;
                    //SendOrder(_tsymbol, "1500", Signal.Buy, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                AddLog("");
                AddLog(ex.ToString());
                AddLog("");
                MessageBox.Show(ex.ToString());
            }
        }

        private void CloseCampaign()
        {
            int i;
            string script = "";

            AddLog("Close campaign has started.");
            for (i = 0; i < 10; i++)
            {
                try
                {
                    script = "$('.exit-onboarding').click()";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);
                    Thread.Sleep(1000);
                }
                catch (Exception ex1)
                {
                    Thread.Sleep(3000);
                }
            }

            for (i = 0; i < 5; i++)
            {
                try
                {
                    script = "$('.trading-platform-popups').hide()";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);
                    Thread.Sleep(1000);
                }
                catch (Exception ex1)
                {
                    Thread.Sleep(3000);
                }
            }

            if (i < 10)
            {
                AddLog("Close campaign has finished.");
            }
        }

        private bool GetSymbolList(string symbol, int retry_index = 0)
        {
            try
            {
                IWebDriver driver = WebBrowserFacade.getDriver;

                IWebElement divAssetsList = driver.FindElement(By.Id("assetsList"));
                ReadOnlyCollection<IWebElement> collAssets = divAssetsList.FindElements(By.TagName("div"));

                // Find & Select Symbol
                int i;
                for (i = 0; i < collAssets.Count; i++)
                {
                    IWebElement divAsset = collAssets[i];
                    string strInnerHTML = divAsset.GetAttribute("innerHTML");
                    if (strInnerHTML.Contains(symbol))
                    {
                        string script = "document.getElementById('assetsList').children[" + i.ToString() + "].click()";
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                        break;
                    }
                }

                Thread.Sleep(1000);

                // GetSymbol Info
                IWebElement divContainer = driver.FindElement(By.Id("carousel_container"));
                ReadOnlyCollection<IWebElement> collSymbols = divContainer.FindElements(By.ClassName("carousel_item"));
                for (i = 0; i < collSymbols.Count; i++)
                {
                    IWebElement divSymbol = collSymbols[i];
                    AddSymbolInfo(divSymbol, symbol);
                    Thread.Sleep(100);
                }

                return true;
            }
            catch (Exception ex)
            {
                string strErrMsg = "商品情報取得が失敗しました。 エラー : " + ex.ToString();
                AddLog(strErrMsg);

                if (retry_index < CGlobalVar.g_nRetryCount)
                {
                    Thread.Sleep(CGlobalVar.g_nRetryInterval);
                    return GetSymbolList(symbol, retry_index + 1);
                }

                return false;
            }
        }

        private void AddSymbolInfo(IWebElement divRoot, string name)
        {
            try
            {
                HighLowSymbol symbol = new HighLowSymbol();

                symbol.id = divRoot.GetAttribute("id");
                symbol.name = name;

                // Get Timeframe
                IWebElement divPanelHeader = divRoot.FindElement(By.ClassName("instrument-panel-header"));
                IWebElement divPanelDuration = divPanelHeader.FindElement(By.ClassName("instrument-panel-duration"));
                IWebElement spanDuration = divPanelDuration.FindElement(By.ClassName("duration"));
                string timeframe = spanDuration.GetAttribute("innerHTML");
                symbol.timeframe = CGlobalVar.StrToTimeframe(timeframe);

                // Get closing time & payout_rate
                IWebElement divPanelBody = divRoot.FindElement(By.ClassName("instrument-panel-body"));
                IWebElement divClosing = divPanelBody.FindElement(By.ClassName("instrument-panel-closing"));
                IWebElement spanClosing = divClosing.FindElement(By.ClassName("time-digits"));
                symbol.closing_time = spanClosing.GetAttribute("innerHTML");

                IWebElement divFirstChild = divRoot.FindElement(By.ClassName("first-child"));
                IWebElement divPanelPayout = divFirstChild.FindElement(By.ClassName("instrument-panel-payout-wrap"));
                IWebElement divPayoutRate = divPanelPayout.FindElement(By.ClassName("instrument-panel-payout"));
                symbol.payout_rate = divPayoutRate.GetAttribute("innerHTML");

                m_lstHighLowSymbols.Add(symbol);
            }
            catch (Exception ex)
            {
                AddLog("");
                AddLog(ex.ToString());
                AddLog("");

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CheckStopTime(DateTime dtCheck)
        {
            string strCheck = dtCheck.ToString("yyyy-MM-dd HH:mm");
            for (int i = 0; i < CGlobalVar.g_lstStopTimes.Count; i++)
            {
                StopTime st = CGlobalVar.g_lstStopTimes[i];
                string strBegin = st.strBeginDay + " " + st.strBeginTime;
                string strEnd = st.strEndDay + " " + st.strEndTime;

                if (strBegin.CompareTo(strCheck) <= 0 && strCheck.CompareTo(strEnd) <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshSite()
        {
            int nSleep = 20 * 60 * 1000;    // 20 min
            string script = "";
            while (true)
            {
                try
                {
                    Thread.Sleep(nSleep);
                    if (!m_bLoadFinished) continue;
                    DateTime dtNow = DateTime.Now;
                    if (dtNow.Minute != 5 && dtNow.Minute != 35) continue;

                    if (!m_bTrading) continue;
                    IWebDriver driver = WebBrowserFacade.getDriver;
                    driver.Navigate().Refresh();
                    AddLog("RefreshSite has succeeded.");

                    Thread.Sleep(1000);
                    CloseCampaign();
                }
                catch (Exception ex)
                {
                    AddLog("");
                    AddLog("RefreshSite has failed with error. Err : " + ex.ToString());
                    AddLog("");
                }
            }
        }

        private void SendOrder(TradeSymbol _tsymbol, string amount, TradeSignal signal, DateTime order_time, int nRetryIndex = 0)
        {
            string symbol_name = _tsymbol.symbol;
            Timeframe timeframe = _tsymbol.timeframe;
            decimal dAmount1 = 0, dAmount2 = 0;

            if (timeframe == Timeframe._NONE)
            {
                AddLog("Invalid timeframe!");
                m_nCurrentOrderCount--;
                return;
            }
            if (!m_bAllowTrade)
            {
                AddLog("Trade is not allowed. SendOrder has ignored.");
                m_nCurrentOrderCount--;
                return;
            }

            if (!m_bTrading)
            {
                AddLog("Trading is not started yet. SendOrder has ignored.");
                m_nCurrentOrderCount--;
                return;
            }

            if (!m_bLoadFinished)
            {
                AddLog("GetInfo hasn't finished yet. SendOrder has ignored.");
                m_nCurrentOrderCount--;
                return;
            }

            DateTime dtNow = order_time;
            AddLog("Ordered Time : " + dtNow.ToString("yyyy-MM-dd HH:mm:ss"));

            lock (syncOrder)
            {
                m_bOrdering = true;
                ReadOnlyCollection<IWebElement> collSpans;
                try
                {
                    int i, nSelIndex = -1;
                    List<HighLowSymbol> lstSelIndicies = new List<HighLowSymbol>();
                    string script = "";

                    if (!CheckStopTime(dtNow))
                    {
                        AddLog("SendOrder has stopped because of stopping time.");
                        m_nCurrentOrderCount--;
                        m_bOrdering = false;
                        return;
                    }

                    AddLog("Prepare for new order..");

                    IWebDriver driver = WebBrowserFacade.getDriver;

                    // Get Balance
                    AddLog("Getting Balance has started.");
                    IWebElement spanBalance = driver.FindElement(By.Id("balance"));
                    string strBalance = spanBalance.Text;
                    AddLog("Balance : " + strBalance);

                    strBalance = strBalance.Substring(1, strBalance.Length - 1);
                    strBalance = strBalance.Replace(",", "");
                    decimal.TryParse(amount, out dAmount1);
                    decimal.TryParse(strBalance, out dAmount2);

                    if (dAmount1 > dAmount2)
                    {
                        AddLog("Not enough balance. SendOrder has ignored.");
                        m_nCurrentOrderCount--;
                        m_bOrdering = false;
                        return;
                    }

                    decimal dBetAmount = dAmount1;
                    AddLog("BetAmount = " + dBetAmount.ToString());

                    // Click trade mode
                    int nMode = ((int)_tsymbol.trans_mode) - 1;
                    script = "document.getElementsByClassName('gameTab')[" + nMode.ToString() + "].click()";
                    ((IJavaScriptExecutor)WebBrowserFacade.getDriver).ExecuteScript(script);

                    IWebElement divAssetsList = driver.FindElement(By.Id("assetsList"));
                    ReadOnlyCollection<IWebElement> collAssets = divAssetsList.FindElements(By.TagName("div"));

                    // Find & Select Symbol
                    for (i = 0; i < collAssets.Count; i++)
                    {
                        IWebElement divAsset = collAssets[i];
                        string strInnerHTML = divAsset.GetAttribute("innerHTML");
                        if (strInnerHTML.Contains(symbol_name))
                        {
                            script = "document.getElementById('assetsList').children[" + i.ToString() + "].click()";
                            ((IJavaScriptExecutor)driver).ExecuteScript(script);
                            break;
                        }
                    }

                    Thread.Sleep(100);

                    for (i = 0; i < m_lstHighLowSymbols.Count; i++)
                    {
                        HighLowSymbol symbol = m_lstHighLowSymbols[i];
                        if (symbol.name != symbol_name || symbol.timeframe != timeframe) continue;

                        // Check closing time
                        string strNow = dtNow.ToString("HH:mm");
                        int nHour = 0, nMin = 0;
                        string closing_time = symbol.closing_time;
                        if (closing_time == "")
                        {
                            closing_time = "23:59";
                        }
                        int.TryParse(closing_time.Substring(0, 2), out nHour);
                        int.TryParse(closing_time.Substring(3, 2), out nMin);
                        int nDiff = 0;
                        int nAddMins = ((int)timeframe) / 60;
                        if (_tsymbol.trans_mode == TransMode.HighLow || _tsymbol.trans_mode == TransMode.HighLowSpread)
                        {
                            // Calc by symbol's closing time
                            nDiff = (dtNow.Minute - nMin) + (dtNow.Hour - nHour) * 60;
                            int nRemain = (nDiff + nAddMins) / nAddMins;
                            nMin += nRemain * nAddMins;
                            nHour += nMin / 60;
                            nMin %= 60;
                            symbol.closing_time = nHour.ToString().PadLeft(2, '0') + nMin.ToString().PadLeft(2, '0');
                        }
                        else
                        {
                            // Calc by add time
                            DateTime dtPlus = dtNow;
                            dtPlus.AddSeconds((int)timeframe);
                            dtPlus.AddSeconds(10); // Plus for delta
                            symbol.closing_time = dtPlus.ToString("HH:mm:ss");
                        }

                        lstSelIndicies.Add(symbol);

                        /*if (closing_time.CompareTo(symbol.closing_time) > 0)
                        {
                            nSelIndex = i;
                            closing_time = symbol.closing_time;
                        }*/
                    }

                    if (lstSelIndicies.Count < 0)
                    {
                        // Not found symbol
                        AddLog("Symbol not found!");
                        m_nCurrentOrderCount--;
                        m_bOrdering = false;
                        return;
                    }

                    // Sort symbols
                    lstSelIndicies.Sort(new CmpSymbol());
                    string strSortResult = lstSelIndicies[0].closing_time + " - " + lstSelIndicies[1].closing_time + " - " + lstSelIndicies[2].closing_time;
                    AddLog("SortResult : " + strSortResult);
                    AddLog("Finding symbols..");

                    if (_tsymbol.timeframe == Timeframe._15_MINUTE)
                    {
                        if (m_bTestMode)
                        {
                            nSelIndex = 0;
                        }
                        else
                        {
                            if (lstSelIndicies.Count < 3)
                            {
                                AddLog("Symbol not found for [Late]!");
                                return;
                            }
                            nSelIndex = 2;
                        }
                    }
                    else
                    {
                        // Select first
                        nSelIndex = 0;
                    }

                    m_stSelSymbol = lstSelIndicies[nSelIndex];
                    AddLog("Found symbol! Symbol = " + m_stSelSymbol.name + ", SymbolID = " + m_stSelSymbol.id + ", Timeframe = " + m_stSelSymbol.timeframe.ToString());

                    // Check for delay
                    Thread.Sleep(CGlobalVar.g_nDelayTime);

                    // Send order
                    script = "document.getElementById('" + m_stSelSymbol.id + "').click()";
                    ((IJavaScriptExecutor)driver).ExecuteScript(script);

                    IWebElement inpAmount = driver.FindElement(By.Id("amount"));
                    if (inpAmount == null)
                    {
                        // Before 1min
                        AddLog("SendOrder has failed. Check if before 1 minute has failed.");
                        m_bOrdering = false;
                        return;
                    }
                    string strReadonly = inpAmount.GetAttribute("readonly");
                    if (strReadonly != null && strReadonly == "readonly")
                    {
                        // Before 1min
                        AddLog("SendOrder has failed. It's just 1 minute before judgement!");
                        m_bOrdering = false;
                        return;
                    }

                    script = "document.getElementById('amount').value='" + dBetAmount.ToString() + "'";
                    ((IJavaScriptExecutor)driver).ExecuteScript(script);
                    IWebElement inputAmount = driver.FindElement(By.Id("amount"));
                    inputAmount.SendKeys(OpenQA.Selenium.Keys.Enter);

                    if (signal.cmd == Signal.Sell)
                    {
                        // Down Button
                        script = "document.getElementById('down_button').click()";
                    }
                    else if (signal.cmd == Signal.Buy)
                    {
                        // Up Button
                        script = "document.getElementById('up_button').click()";
                    }
                    ((IJavaScriptExecutor)driver).ExecuteScript(script);

                    script = "document.getElementById('invest_now_button').click()";
                    ((IJavaScriptExecutor)driver).ExecuteScript(script);

                    string strResult = "";
                    for (int retry = 0; retry < 20; retry++)
                    {
                        try
                        {
                            IWebElement spanNotify = driver.FindElement(By.Id("notification_text"));
                            collSpans = spanNotify.FindElements(By.TagName("span"));
                            if (collSpans == null || collSpans.Count == 0)
                            {
                                Thread.Sleep(CGlobalVar.g_nRetryInterval);
                                continue;
                            }
                            if (collSpans[0].Text != "Processing" && collSpans[0].Text != "Processing!" && collSpans[0].Text != "処理中" && collSpans[0].Text != "処理中!")
                            {
                                strResult = collSpans[0].Text;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        Thread.Sleep(CGlobalVar.g_nRetryInterval);
                    }

                    if (strResult == "成功")
                    {
                        // Success
                        AddLog("SendOrder has succeeded!");
                        m_nCurrentOrderCount--;

                        TradeSymbol _symbol = CGlobalVar.g_lstTradeSymbols[_tsymbol.index];
                        _symbol.is_trading = true;
                        _tsymbol.is_trading = true;
                        CGlobalVar.g_lstTradeSymbols[_tsymbol.index] = _symbol;

                        Thread thread_get_trade_history = new Thread(new ParameterizedThreadStart(GetTradeHistory));
                        thread_get_trade_history.IsBackground = true;
                        object args = new object[3] { _tsymbol, 0, signal.id };
                        thread_get_trade_history.Start(args);
                    }
                    else
                    {
                        // Failure
                        AddLog("SendOrder has failed with error. ErrMsg : " + strResult);
                        m_nCurrentOrderCount--;

                        if (nRetryIndex < CGlobalVar.g_nRetryCount)
                        {
                            nRetryIndex++;

                            AddLog("");
                            AddLog("Retry for SendOrder... RetryIndex = " + nRetryIndex.ToString());
                            Thread.Sleep(CGlobalVar.g_nRetryInterval);

                            SendOrder(_tsymbol, amount, signal, order_time, nRetryIndex);
                        }
                        else
                        {
                            AddLog("");
                            AddLog("!!!!!!!!!!! SendOrder has failed.");
                        }
                    }

                    m_bOrdering = false;
                }
                catch (Exception ex)
                {
                    string strErrMsg = "Send order has failed with error. Err : " + ex.ToString();
                    AddLog(strErrMsg);

                    if (nRetryIndex < CGlobalVar.g_nRetryCount)
                    {
                        nRetryIndex++;

                        AddLog("");
                        AddLog("Retry for SendOrder... RetryIndex = " + nRetryIndex.ToString());
                        Thread.Sleep(CGlobalVar.g_nRetryInterval);

                        SendOrder(_tsymbol, amount, signal, order_time, nRetryIndex);
                    }
                    else
                    {
                        AddLog("");
                        AddLog("!!!!!!!!!!! SendOrder has failed.");
                        m_nCurrentOrderCount--;
                    }
                }
            }
        }

        private CopiesStatus GetHistoryStatus(string strText)
        {
            switch (strText)
            {
                case "Closed":
                case "取引終了":
                    return CopiesStatus.Completed;

                case "Pending":
                case "取引中":
                    return CopiesStatus.Pending;
            }

            return CopiesStatus.None;
        }

        private void GetTradeHistory(object args)
        {
            Array array = (Array)args;
            TradeSymbol symbol = (TradeSymbol)array.GetValue(0);
            int nRetryIndex = (int)array.GetValue(1);
            int nSignalId = (int)array.GetValue(2);

            TradeHistory history = new TradeHistory();

            history.id = 0;
            history.signal_id = nSignalId;
            history.trader_id = m_nUserID;

            history.trans_timeunit = (int)symbol.timeframe;
            history.trans_suffix = "";

            Thread.Sleep(1000);

            try
            {
                // Get First Row
                IWebDriver driver = WebBrowserFacade.getDriver;
                ReadOnlyCollection<IWebElement> trAll = driver.FindElements(By.ClassName("trade-details-tbl"));

                history.trans_mode = symbol.trans_mode;

                // Asset
                IWebElement tdAsset = trAll[0].FindElement(By.ClassName("assetNameItem"));
                IWebElement spanAsset = tdAsset.FindElement(By.ClassName("assetElement"));
                history.symbol = spanAsset.Text;

                // Strike
                IWebElement tdStrike = trAll[0].FindElement(By.ClassName("tradingStrikeItem"));
                IWebElement divStrike = tdStrike.FindElement(By.ClassName("strikeArea"));
                IWebElement spanStrike = divStrike.FindElement(By.ClassName("strikeValue"));
                decimal.TryParse(spanStrike.Text, out history.order_price);

                // HighLow
                IWebElement spanMarker = divStrike.FindElement(By.ClassName("tradeActionMarkerType"));
                string strClassName = spanMarker.GetAttribute("class");
                if (strClassName.Contains("Call"))
                {
                    history.cmd = Signal.Buy;
                }
                else
                {
                    history.cmd = Signal.Sell;
                }

                // Start Time
                IWebElement tdStartTime = trAll[0].FindElement(By.ClassName("tradingDateTimeItem"));
                string strToday = DateTime.Now.ToString("yyyy-MM-dd");
                string strStartTime = strToday + " " + tdStartTime.Text;
                history.ordered_at = strStartTime;

                // Expiry Time
                IWebElement tdExpiryTime = trAll[0].FindElement(By.ClassName("expirationDateTimeItem"));
                history.judged_at = strToday + " " + tdExpiryTime.Text;

                // Status
                IWebElement tdStatus = trAll[0].FindElement(By.ClassName("statusItem"));
                history.status = GetHistoryStatus(tdStatus.Text);

                // Closing Rate
                IWebElement tdClosingRate = trAll[0].FindElement(By.ClassName("closingRateItem"));
                decimal.TryParse(tdClosingRate.Text, out history.judge_price);

                // Investment
                IWebElement tdInvestment = trAll[0].FindElement(By.ClassName("moneyInvestedItem"));
                string strTemp = tdInvestment.Text.Replace(m_strCurrency, "");
                strTemp = strTemp.Replace(",", "");
                decimal.TryParse(strTemp, out history.order_amount);

                // Expiry Payout
                history.payout_amount = 0;

                // Add TradeHistory To List
                AddTradeHistory(history);
                SendTradeHistory(history);
                CGlobalVar.g_lstTradeHistory.Add(history);

                Thread thread_get_trade_result = new Thread(new ParameterizedThreadStart(GetTradeResult));
                thread_get_trade_result.IsBackground = true;
                int index = CGlobalVar.g_lstTradeHistory.Count - 1;
                object param = new object[4] { history.judged_at, index, 0, symbol.index };
                thread_get_trade_result.Start(param);
            }
            catch (Exception ex)
            {
                string strErrMsg = "GetTradeHistory has failed with error. Err : " + ex.ToString();
                AddLog(strErrMsg);

                if (nRetryIndex < CGlobalVar.g_nRetryCount)
                {
                    nRetryIndex++;
                    AddLog("");
                    AddLog("Retry for GetTradeHistory. RetryIndex = " + nRetryIndex.ToString());
                    Thread.Sleep(CGlobalVar.g_nRetryInterval);
                    array.SetValue(nRetryIndex, 1);
                    GetTradeHistory(array);
                }
            }
        }

        private void GetTradeResult(object args)
        {
            Array array = (Array)args;
            string expiry_time = (string)array.GetValue(0);
            int index = (int)array.GetValue(1);
            int nRetryIndex = (int)array.GetValue(2);
            int nSymbolIndex = (int)array.GetValue(3);

            try
            {
                DateTime dtClosing, dtNow = DateTime.Now;
                string strClosingDate = dtNow.ToString("yyyy-MM-dd");
                if (expiry_time.Contains("Tomorrow"))
                {
                    strClosingDate = dtNow.AddDays(1).ToString("yyyy-MM-dd");
                }
                string strClosingTime = expiry_time;
                DateTime.TryParse(strClosingTime, out dtClosing);

                int nRemainSecs = Math.Abs(((int)(dtClosing - dtNow).TotalSeconds)) + 2; // Delay 2s

                Thread.Sleep(nRemainSecs * 1000);

                // Get Result
                TradeHistory history = CGlobalVar.g_lstTradeHistory[index];

                // Get First Row
                IWebDriver driver = WebBrowserFacade.getDriver;
                while (true)
                {
                    ReadOnlyCollection<IWebElement> trAll = driver.FindElements(By.ClassName("trade-details-tbl"));
                    //history.option = (CGlobalVar.g_nTradeType == TradeType.HighLow) ? "HL" : "HL-Spread"; // ???

                    int tdIndex = CGlobalVar.g_lstTradeHistory.Count - index - 1;

                    // Status
                    IWebElement tdStatus = trAll[tdIndex].FindElement(By.ClassName("statusItem"));
                    history.status = GetHistoryStatus(tdStatus.Text);
                    if (history.status != CopiesStatus.Completed && history.status != CopiesStatus.Sold)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    // Closing Rate
                    IWebElement tdClosingRate = trAll[tdIndex].FindElement(By.ClassName("closingRateItem"));
                    decimal.TryParse(tdClosingRate.Text, out history.judge_price);

                    // Expiry Payout
                    IWebElement tdPayout = trAll[tdIndex].FindElement(By.ClassName("profitLossItem"));
                    IWebElement spanPayout = tdPayout.FindElement(By.TagName("span"));
                    string strTemp = spanPayout.Text.Replace(m_strCurrency, "");
                    strTemp = strTemp.Replace(",", "");
                    decimal.TryParse(strTemp, out history.payout_amount);

                    break;
                }

                lock (syncLock)
                {
                    int nTempIndex = index;

                    AddLog("GetTradeResult has succeeded. Index = " + nTempIndex);
                    CGlobalVar.g_lstTradeHistory.RemoveAt(nTempIndex);
                    CGlobalVar.g_lstTradeHistory.Insert(nTempIndex, history);

                    TradeSymbol symbol = CGlobalVar.g_lstTradeSymbols[nSymbolIndex];
                    symbol.is_trading = false;
                    AddLog("Set trading flag to false! Symbol = " + symbol.symbol + ", Index = " + nSymbolIndex);
                    CGlobalVar.g_lstTradeSymbols[nSymbolIndex] = symbol;

                    UpdateTradeHistory(index, history);
                    SendTradeResult(history);
                }
            }
            catch (Exception ex)
            {
                string strErrMsg = "GetTradeResult has failed with error. Err : " + ex.ToString();
                AddLog(strErrMsg);

                if (nRetryIndex < CGlobalVar.g_nRetryCount)
                {
                    nRetryIndex++;
                    AddLog("");
                    AddLog("Retry for GetTradeResult. RetryIndex = " + nRetryIndex.ToString());
                    Thread.Sleep(CGlobalVar.g_nRetryInterval);
                    array.SetValue(nRetryIndex, 2);
                    GetTradeResult(array);
                }
            }
        }

        private string GetHighLowStr(HighLow high_low)
        {
            return high_low == HighLow.High ? "High" : "Low";
        }

        private string DecimalToAmount(decimal value)
        {
            return m_strCurrency + String.Format("{0:n0}", value);
        }

        private void ClearTradeHistory()
        {
            if (lsvTradeHistory.InvokeRequired)
            {
                ClearTradeHistoryCallBack d = new ClearTradeHistoryCallBack(ClearTradeHistory);
                this.Invoke(d, new object[] { });
            }
            else
            {
                lsvTradeHistory.Items.Clear();
            }
        }
    }

    class CmpSymbol : IComparer<HighLowSymbol>
    {
        public int Compare(HighLowSymbol a, HighLowSymbol b)
        {
            return a.closing_time.CompareTo(b.closing_time);
        }
    }
}
