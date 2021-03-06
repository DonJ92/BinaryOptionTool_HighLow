//+------------------------------------------------------------------+
//|                                            MtCheck_Interface.mq4 |
//|                                                        RedSpider |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "RedSpider"
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

#define OP_NONE                  -1
#define OP_BUYCLOSE              1000
#define OP_SELLCLOSE             1001
#define MAX_INDICATOR_COUNT      10

#define INFINITE                 2000000000
#define YLS_INDICATOR_NAME       "YLS"
#define RD_VLDMI_INDICATOR_NAME  "RDcomboVLDMI2"

input int InpRetryCount = 3;

struct HLAS_INIDICATOR {
   string name;
   int sub_window;
   int indi_index;
   
   int buy_index;
   int sell_index;
   string occurred_at;
};

int g_nIndicatorCount = 0;
HLAS_INIDICATOR g_stIndicators[MAX_INDICATOR_COUNT];

#import "HLAS_Sender.dll"
int SendSignal(char &source[], char &symbol[], int cmd, int &result);
#import

int DoSendSignal(string source, string symbol, int cmd)
{
   int _result = 0;
   char _source[255];
   char _symbol[255];
   
   string strSymbol = symbol;
   if (StringLen(symbol) >= 6)
   {
      strSymbol = StringSubstr(symbol, 0, 3) + "/" + StringSubstr(symbol, 3, 3);
   }
   
   ArrayInitialize(_source, '\0');
   StringToCharArray(source, _source, 0, WHOLE_ARRAY,CP_UTF8);
   ArrayInitialize(_symbol, '\0');
   StringToCharArray(strSymbol, _symbol, 0, WHOLE_ARRAY,CP_UTF8);
   
   int nRetryIndex = 1;
   
   while (nRetryIndex <= InpRetryCount)
   {
   #ifdef USE_LOG
      Print("SendSignal(" + nRetryIndex + ") ==> " + source + ", " + symbol + ", " + CmdStr(cmd));
   #endif
      
      int nRet = SendSignal(_source, _symbol, cmd, _result);
      
   #ifdef USE_LOG
      Print("SendSignal ==> Ret = " + nRet + ", Result = " + _result);
   #endif
   
      if (nRet == 0 || _result != 0)
      {
         #ifdef USE_LOG
            Print("SendSignal has failed! RetryIndex = " + nRetryIndex);
         #endif
         nRetryIndex ++;
      }
      else
      {
         #ifdef USE_LOG
            Print("SendSignal has succeeded!");
         #endif
         return 1;
      }
   }
   
   return 0;
}

string CmdStr(int cmd)
{
   switch (cmd)
   {
      case OP_NONE: return "None"; break;
      case OP_BUY: return "Buy"; break;
      case OP_SELL: return "Sell"; break;
      case OP_BUYLIMIT: return "BuyLimit"; break;
      case OP_SELLLIMIT: return "SellLimit"; break;
      case OP_BUYSTOP: return "BuyStop"; break;
      case OP_SELLSTOP: return "SellStop"; break;
      case OP_BUYCLOSE: return "BuyClose"; break;
      case OP_SELLCLOSE: return "SellClose"; break;
   }
   
   return "";
}

bool ButtonCreate(const long              chart_ID=0,               // chart's ID
                  const string            name="Button",            // button name
                  const int               sub_window=0,             // subwindow index
                  const int               x=0,                      // X coordinate
                  const int               y=0,                      // Y coordinate
                  const int               width=50,                 // button width
                  const int               height=18,                // button height
                  const ENUM_BASE_CORNER  corner=CORNER_LEFT_UPPER, // chart corner for anchoring
                  const string            text="Button",            // text
                  const string            font="Arial",             // font
                  const int               font_size=10,             // font size
                  const color             clr=clrBlack,             // text color
                  const color             back_clr=C'236,233,216',  // background color
                  const color             border_clr=clrNONE,       // border color
                  const bool              state=false,              // pressed/released
                  const bool              back=false,               // in the background
                  const bool              selection=false,          // highlight to move
                  const bool              hidden=true,              // hidden in the object list
                  const long              z_order=0)                // priority for mouse click
  {
//--- reset the error value
   ResetLastError();
//--- create the button
   if(!ObjectCreate(chart_ID,name,OBJ_BUTTON,sub_window,0,0))
     {
      Print(__FUNCTION__,
            ": failed to create the button! Error code = ",GetLastError());
      return(false);
     }
//--- set button coordinates
   ObjectSetInteger(chart_ID,name,OBJPROP_XDISTANCE,x);
   ObjectSetInteger(chart_ID,name,OBJPROP_YDISTANCE,y);
//--- set button size
   ObjectSetInteger(chart_ID,name,OBJPROP_XSIZE,width);
   ObjectSetInteger(chart_ID,name,OBJPROP_YSIZE,height);
//--- set the chart's corner, relative to which point coordinates are defined
   ObjectSetInteger(chart_ID,name,OBJPROP_CORNER,corner);
//--- set the text
   ObjectSetString(chart_ID,name,OBJPROP_TEXT,text);
//--- set text font
   ObjectSetString(chart_ID,name,OBJPROP_FONT,font);
//--- set font size
   ObjectSetInteger(chart_ID,name,OBJPROP_FONTSIZE,font_size);
//--- set text color
   ObjectSetInteger(chart_ID,name,OBJPROP_COLOR,clr);
//--- set background color
   ObjectSetInteger(chart_ID,name,OBJPROP_BGCOLOR,back_clr);
//--- set border color
   ObjectSetInteger(chart_ID,name,OBJPROP_BORDER_COLOR,border_clr);
//--- display in the foreground (false) or background (true)
   ObjectSetInteger(chart_ID,name,OBJPROP_BACK,back);
//--- set button state
   ObjectSetInteger(chart_ID,name,OBJPROP_STATE,state);
//--- enable (true) or disable (false) the mode of moving the button by mouse
   ObjectSetInteger(chart_ID,name,OBJPROP_SELECTABLE,selection);
   ObjectSetInteger(chart_ID,name,OBJPROP_SELECTED,selection);
//--- hide (true) or display (false) graphical object name in the object list
   ObjectSetInteger(chart_ID,name,OBJPROP_HIDDEN,hidden);
//--- set the priority for receiving the event of a mouse click in the chart
   ObjectSetInteger(chart_ID,name,OBJPROP_ZORDER,z_order);
//--- successful execution
   return(true);
}
