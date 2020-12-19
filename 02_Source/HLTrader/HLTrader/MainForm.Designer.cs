namespace HLTrader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tpgLog = new System.Windows.Forms.TabPage();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.tpgHistory = new System.Windows.Forms.TabPage();
            this.btnClearTradeHistory = new System.Windows.Forms.Button();
            this.lsvTradeHistory = new System.Windows.Forms.ListView();
            this.colTradeHistory_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_DemoReal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_Option = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_Symbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_Strike = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_StartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_ExpiryTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_ClosingRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_Investment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTradeHistory_ExpiryPayout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNowTime = new System.Windows.Forms.Label();
            this.picBanner = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStartTrade = new System.Windows.Forms.Button();
            this.tbcMain.SuspendLayout();
            this.tpgLog.SuspendLayout();
            this.tpgHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Location = new System.Drawing.Point(626, 11);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(100, 39);
            this.btnSetting.TabIndex = 2;
            this.btnSetting.Text = "設定";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnExit
            // 
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(838, 11);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 39);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "完了";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Location = new System.Drawing.Point(732, 11);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(100, 39);
            this.btnClearLogs.TabIndex = 3;
            this.btnClearLogs.Text = "ログクリア";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // tbcMain
            // 
            this.tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcMain.Controls.Add(this.tpgLog);
            this.tbcMain.Controls.Add(this.tpgHistory);
            this.tbcMain.Location = new System.Drawing.Point(9, 34);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(987, 411);
            this.tbcMain.TabIndex = 5;
            // 
            // tpgLog
            // 
            this.tpgLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tpgLog.Controls.Add(this.txtLogs);
            this.tpgLog.Location = new System.Drawing.Point(4, 22);
            this.tpgLog.Name = "tpgLog";
            this.tpgLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpgLog.Size = new System.Drawing.Size(979, 385);
            this.tpgLog.TabIndex = 0;
            this.tpgLog.Text = "ログ";
            // 
            // txtLogs
            // 
            this.txtLogs.AcceptsReturn = true;
            this.txtLogs.AcceptsTab = true;
            this.txtLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogs.BackColor = System.Drawing.Color.White;
            this.txtLogs.Location = new System.Drawing.Point(6, 6);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogs.Size = new System.Drawing.Size(968, 375);
            this.txtLogs.TabIndex = 0;
            this.txtLogs.WordWrap = false;
            // 
            // tpgHistory
            // 
            this.tpgHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tpgHistory.Controls.Add(this.btnClearTradeHistory);
            this.tpgHistory.Controls.Add(this.lsvTradeHistory);
            this.tpgHistory.Location = new System.Drawing.Point(4, 22);
            this.tpgHistory.Name = "tpgHistory";
            this.tpgHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tpgHistory.Size = new System.Drawing.Size(979, 385);
            this.tpgHistory.TabIndex = 1;
            this.tpgHistory.Text = "取引履歴";
            // 
            // btnClearTradeHistory
            // 
            this.btnClearTradeHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearTradeHistory.Location = new System.Drawing.Point(11, 6);
            this.btnClearTradeHistory.Name = "btnClearTradeHistory";
            this.btnClearTradeHistory.Size = new System.Drawing.Size(91, 28);
            this.btnClearTradeHistory.TabIndex = 1;
            this.btnClearTradeHistory.Text = "履歴クリア";
            this.btnClearTradeHistory.UseVisualStyleBackColor = true;
            // 
            // lsvTradeHistory
            // 
            this.lsvTradeHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvTradeHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTradeHistory_No,
            this.colTradeHistory_DemoReal,
            this.colTradeHistory_Option,
            this.colTradeHistory_Symbol,
            this.colTradeHistory_Strike,
            this.colTradeHistory_StartTime,
            this.colTradeHistory_ExpiryTime,
            this.colTradeHistory_Status,
            this.colTradeHistory_ClosingRate,
            this.colTradeHistory_Investment,
            this.colTradeHistory_ExpiryPayout});
            this.lsvTradeHistory.FullRowSelect = true;
            this.lsvTradeHistory.Location = new System.Drawing.Point(6, 40);
            this.lsvTradeHistory.Name = "lsvTradeHistory";
            this.lsvTradeHistory.Size = new System.Drawing.Size(967, 339);
            this.lsvTradeHistory.TabIndex = 0;
            this.lsvTradeHistory.UseCompatibleStateImageBehavior = false;
            this.lsvTradeHistory.View = System.Windows.Forms.View.Details;
            // 
            // colTradeHistory_No
            // 
            this.colTradeHistory_No.Text = "番号";
            this.colTradeHistory_No.Width = 40;
            // 
            // colTradeHistory_DemoReal
            // 
            this.colTradeHistory_DemoReal.Text = "デモー/レアル";
            this.colTradeHistory_DemoReal.Width = 80;
            // 
            // colTradeHistory_Option
            // 
            this.colTradeHistory_Option.Text = "タイプ";
            this.colTradeHistory_Option.Width = 100;
            // 
            // colTradeHistory_Symbol
            // 
            this.colTradeHistory_Symbol.Text = "取引原資産";
            this.colTradeHistory_Symbol.Width = 80;
            // 
            // colTradeHistory_Strike
            // 
            this.colTradeHistory_Strike.Text = "取引内容";
            this.colTradeHistory_Strike.Width = 80;
            // 
            // colTradeHistory_StartTime
            // 
            this.colTradeHistory_StartTime.Text = "取引時間";
            this.colTradeHistory_StartTime.Width = 120;
            // 
            // colTradeHistory_ExpiryTime
            // 
            this.colTradeHistory_ExpiryTime.Text = "\t判定時刻";
            this.colTradeHistory_ExpiryTime.Width = 120;
            // 
            // colTradeHistory_Status
            // 
            this.colTradeHistory_Status.Text = "ステータス";
            // 
            // colTradeHistory_ClosingRate
            // 
            this.colTradeHistory_ClosingRate.Text = "判定レート";
            this.colTradeHistory_ClosingRate.Width = 80;
            // 
            // colTradeHistory_Investment
            // 
            this.colTradeHistory_Investment.Text = "購入";
            this.colTradeHistory_Investment.Width = 80;
            // 
            // colTradeHistory_ExpiryPayout
            // 
            this.colTradeHistory_ExpiryPayout.Text = "判定時ペイアウト";
            this.colTradeHistory_ExpiryPayout.Width = 120;
            // 
            // lblNowTime
            // 
            this.lblNowTime.AutoSize = true;
            this.lblNowTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNowTime.ForeColor = System.Drawing.Color.Red;
            this.lblNowTime.Location = new System.Drawing.Point(337, 19);
            this.lblNowTime.Name = "lblNowTime";
            this.lblNowTime.Size = new System.Drawing.Size(157, 20);
            this.lblNowTime.TabIndex = 0;
            this.lblNowTime.Text = "2020-12-01 11:37:00";
            // 
            // picBanner
            // 
            this.picBanner.Image = global::HLTrader.Properties.Resources.HighLow;
            this.picBanner.Location = new System.Drawing.Point(9, -5);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(101, 44);
            this.picBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBanner.TabIndex = 7;
            this.picBanner.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HLTrader.Properties.Resources.AutoTrader;
            this.pictureBox1.Location = new System.Drawing.Point(118, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(198, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btnStartTrade
            // 
            this.btnStartTrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnStartTrade.Enabled = false;
            this.btnStartTrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartTrade.Location = new System.Drawing.Point(520, 11);
            this.btnStartTrade.Name = "btnStartTrade";
            this.btnStartTrade.Size = new System.Drawing.Size(100, 39);
            this.btnStartTrade.TabIndex = 1;
            this.btnStartTrade.Text = "取引開始";
            this.btnStartTrade.UseVisualStyleBackColor = false;
            this.btnStartTrade.Click += new System.EventHandler(this.btnStartTrade_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1003, 452);
            this.Controls.Add(this.btnStartTrade);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblNowTime);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClearLogs);
            this.Controls.Add(this.tbcMain);
            this.Controls.Add(this.picBanner);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HLTrader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tbcMain.ResumeLayout(false);
            this.tpgLog.ResumeLayout(false);
            this.tpgLog.PerformLayout();
            this.tpgHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tpgLog;
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.TabPage tpgHistory;
        private System.Windows.Forms.ListView lsvTradeHistory;
        private System.Windows.Forms.ColumnHeader colTradeHistory_No;
        private System.Windows.Forms.ColumnHeader colTradeHistory_Option;
        private System.Windows.Forms.ColumnHeader colTradeHistory_Symbol;
        private System.Windows.Forms.ColumnHeader colTradeHistory_Strike;
        private System.Windows.Forms.ColumnHeader colTradeHistory_StartTime;
        private System.Windows.Forms.ColumnHeader colTradeHistory_ExpiryTime;
        private System.Windows.Forms.ColumnHeader colTradeHistory_Status;
        private System.Windows.Forms.ColumnHeader colTradeHistory_ClosingRate;
        private System.Windows.Forms.ColumnHeader colTradeHistory_Investment;
        private System.Windows.Forms.ColumnHeader colTradeHistory_ExpiryPayout;
        private System.Windows.Forms.Button btnClearTradeHistory;
        private System.Windows.Forms.Label lblNowTime;
        private System.Windows.Forms.PictureBox picBanner;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStartTrade;
        private System.Windows.Forms.ColumnHeader colTradeHistory_DemoReal;
    }
}

