//+------------------------------------------------------------------+
//|                                                  HLAS_Sender.mq4 |
//|                                                        RedSpider |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "RedSpider"
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

#define USE_LOG
#include <HLAS_Interface.mqh>

// Input parameters
input string strNote1 = "YLS ---------------------------------------------"; // ---------------------------------------------
input bool InpUseYLS = true;
input int RSI_Period = 14;                   // RSI期間
input int RSI_UpLimit = 68;                  // RSI上限値
input int RSI_DownLimit = 32;                // RSI下限値
input int STO1_K_Period = 9;                 // Stochastic1 K Period
input int STO1_D_Period = 3;                 // Stochastic1 D Period
input int STO1_Slowing = 3;                  // Stochastic1 Slowing
input int STO2_K_Period = 26;                // Stochastic2 K Period
input int STO2_D_Period = 3;                 // Stochastic2 D Period
input int STO2_Slowing = 3;                  // Stochastic2 Slowing
input int STO3_K_Period = 40;                // Stochastic3 K Period
input int STO3_D_Period = 3;                 // Stochastic3 D Period
input int STO3_Slowing = 3;                  // Stochastic3 Slowing
input int STO_UpLimit = 90;                  // Stochastic上限値
input int STO_DownLimit = 10;                // Stochastic下限値
input int BB_Period = 20;                    // BB期間
input double BB_Diff = 2.0;                  // BB偏差
input int Depth = 12;                        // Depth
input int Deviation = 5;                     // Deviation
input int BackStep = 3;                      // BackStep
input bool ShowAlert = true;                 // アラート
input int SIGN_PosControl = 40;              // SIGN 位置調整
input color UP_SIGN_Color = Magenta;         // UP SINE カラー
input color DOWN_SIGN_Color = DeepSkyBlue;   // DOWN SINE カラー
input int MaxBarCount = 3000; //最大バーカウント

input string strNote2 = "RDcomboVLDMI2 ---------------------------------------"; // ---------------------------------------
input bool InpUseRD_VLDMI = true;
input int ArrowPosition = 20;                // サイン位置
input int ArrowSize = 2;                     // サインサイズ
input color UpArrowColor = Orange;           // UPサインカラー
input color DnArrowColor = Yellow;           // DOWNサインカラー
input color ChartBackGround = Blue;          // DOWNサインカラー
input int BandPeriod1 = 20;                  // ボリンジャー(1) 期間
input double BandDeviation1 = 2.0;           // ボリンジャー(1) 偏差
input int BandPeriod2 = 100;                 // ボリンジャー(2) 期間
input double BandDeviation2 = 2.0;           // ボリンジャー(2) 偏差
input ENUM_APPLIED_PRICE BandApplied = PRICE_CLOSE;  // ボリンジャー 適用価格
input int RD_HistorySize = 1000;
input int RD_ColorThreshold = 5;
input double RD_NoiseFilterRVI = 0.03; 

// Global variables
string g_strBuyBtn = "Buy";
string g_strSellBtn = "Sell";
string g_strLastOccuredAt = "";

int init()
{
   //ReadIndicators();
   EventSetMillisecondTimer(200);
   
   if(!ButtonCreate(0, g_strBuyBtn, 0, 10, 10, 100, 28, CORNER_LEFT_UPPER, g_strBuyBtn, "Arial", 10))
   {
      return 0;
   }
   if(!ButtonCreate(0, g_strSellBtn, 0, 120, 10, 100, 28, CORNER_LEFT_UPPER, g_strSellBtn, "Arial", 10))
   {
      return 0;
   }
   ChartRedraw();
   
   return 0;
}

int deinit()
{
   ObjectDelete(ChartID(), g_strBuyBtn);
   ObjectDelete(ChartID(), g_strSellBtn);

   return 0;
}

int start()
{
   return 0;
}

void OnTimer()
{
   int i;
   
   /*for (i = 0; i < g_nIndicatorCount; i ++)
   {
      CheckIndicator(i);
   }*/
   CheckSignal();
}

double GetIndicatorValue(string strIndiName, int nValueIndex)
{
   int nShift = 0;
   double dVal = INFINITE;
   
   if (strIndiName == YLS_INDICATOR_NAME)
   {
      dVal = iCustom(Symbol(), 0, YLS_INDICATOR_NAME, 
         RSI_Period, RSI_UpLimit, RSI_DownLimit,
         STO1_K_Period, STO1_D_Period, STO1_Slowing, 
         STO2_K_Period, STO2_D_Period, STO2_Slowing, 
         STO3_K_Period, STO3_D_Period, STO3_Slowing, 
         STO_UpLimit, STO_DownLimit, BB_Period, BB_Diff,
         Depth, Deviation, BackStep, ShowAlert, 
         SIGN_PosControl, UP_SIGN_Color, DOWN_SIGN_Color, MaxBarCount, 
         nValueIndex, nShift);
      if (dVal == 0)
      {
         dVal = INFINITE;
      }
   }
   else if (strIndiName == RD_VLDMI_INDICATOR_NAME)
   {
      dVal = iCustom(Symbol(), 0, RD_VLDMI_INDICATOR_NAME, 
         ArrowPosition, ArrowSize, UpArrowColor, DnArrowColor, ChartBackGround, 
         BandPeriod1, BandDeviation1, BandPeriod2, BandDeviation2, BandApplied, 
         RD_HistorySize, RD_ColorThreshold, RD_NoiseFilterRVI, 
         nValueIndex, nShift);
      if (dVal == 0)
      {
         dVal = INFINITE;
      }
   }
   
   return dVal;
}

void CheckSignal()
{
   double dBuy, dSell;
   datetime dtNow = TimeCurrent();
   string strNow = TimeToStr(dtNow, TIME_DATE | TIME_MINUTES);

   if (InpUseYLS == true)
   {
      dBuy = GetIndicatorValue(YLS_INDICATOR_NAME, 2);
      dSell = GetIndicatorValue(YLS_INDICATOR_NAME, 3);
      
      if (MathAbs(dBuy) < INFINITE && g_strLastOccuredAt!= strNow)
      {
         #ifdef USE_LOG
            Print(">> New Signal : Buy, YLS, IndicatorValue = " + DoubleToStr(dBuy));
         #endif
         g_strLastOccuredAt = strNow;
         DoSendSignal(YLS_INDICATOR_NAME, Symbol(), OP_BUY);
      }
      if (MathAbs(dSell) < INFINITE && g_strLastOccuredAt != strNow)
      {
         #ifdef USE_LOG
            Print(">> New Signal : Sell, YLS, IndicatorValue = " + DoubleToStr(dBuy));
         #endif
         g_strLastOccuredAt = strNow;
         DoSendSignal(YLS_INDICATOR_NAME, Symbol(), OP_SELL);
      }
   }

   if (InpUseRD_VLDMI == true)
   {
      dBuy = GetIndicatorValue(RD_VLDMI_INDICATOR_NAME, 0);
      dSell = GetIndicatorValue(RD_VLDMI_INDICATOR_NAME, 1);
      
      if (MathAbs(dBuy) < INFINITE && g_strLastOccuredAt!= strNow)
      {
         #ifdef USE_LOG
            Print(">> New Signal : Buy, YLS, IndicatorValue = " + DoubleToStr(dBuy));
         #endif
         g_strLastOccuredAt = strNow;
         DoSendSignal(RD_VLDMI_INDICATOR_NAME, Symbol(), OP_BUY);
      }
      if (MathAbs(dSell) < INFINITE && g_strLastOccuredAt != strNow)
      {
         #ifdef USE_LOG
            Print(">> New Signal : Sell, YLS, IndicatorValue = " + DoubleToStr(dBuy));
         #endif
         g_strLastOccuredAt = strNow;
         DoSendSignal(RD_VLDMI_INDICATOR_NAME, Symbol(), OP_SELL);
      }
   }
}

void ReadIndicators()
{
   #ifdef USE_LOG
      Print("ReadIndicators has started!!!");
   #endif
   g_nIndicatorCount = 0;
   
   bool bFinished = false;
   int nWindowCount = WindowsTotal();
   for (int w = 0; w < nWindowCount; w ++)
   {
      int nCount = ChartIndicatorsTotal(ChartID(), w);
      for (int i = 0; i < nCount; i ++)
      {
         string strName = ChartIndicatorName(ChartID(), w, i);
         if (InpUseYLS == true && strName == YLS_INDICATOR_NAME)
         {
            #ifdef USE_LOG
               Print(">> " + strName + " has found!");
            #endif
            g_stIndicators[g_nIndicatorCount].name = strName;
            g_stIndicators[g_nIndicatorCount].sub_window = w;
            g_stIndicators[g_nIndicatorCount].indi_index = i;
            g_stIndicators[g_nIndicatorCount].buy_index = 2;
            g_stIndicators[g_nIndicatorCount].sell_index = 3;
            g_stIndicators[g_nIndicatorCount].occurred_at = "";
            g_nIndicatorCount ++;
            if (g_nIndicatorCount >= MAX_INDICATOR_COUNT)
            {
               bFinished = true;
               break;
            }
         }
      }
      if (bFinished) break;
   }
   
   #ifdef USE_LOG
      Print("=======================================================");
      Print(">> Total : " + g_nIndicatorCount + " indicators has found!!!");
   #endif
}

void OnChartEvent(const int id,
                  const long &lparam,
                  const double &dparam,
                  const string &sparam)
{
   if(id==CHARTEVENT_OBJECT_CLICK)
   {
      string clickedChartObject = sparam;
      if (clickedChartObject == g_strBuyBtn)
      {
         DoManualOrder(OP_BUY);
      }
      else if (clickedChartObject == g_strSellBtn)
      {
         DoManualOrder(OP_SELL);
      }
   }
}

void DoManualOrder(int cmd)
{
#ifdef USE_LOG
   Print("ManualOrder : " + CmdStr(cmd));
#endif

   DoSendSignal("Manual", Symbol(), cmd);
}
