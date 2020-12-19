namespace HLTrader
{
    partial class MainSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainSetting));
            this.tbcSetting = new System.Windows.Forms.TabControl();
            this.tpgMain = new System.Windows.Forms.TabPage();
            this.chkAutoLogin = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtRetryCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRetryInterval = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOrderAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdbReal = new System.Windows.Forms.RadioButton();
            this.rdbDemo = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbHighLowSpread = new System.Windows.Forms.RadioButton();
            this.rdbHighLow = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tpgStopTime = new System.Windows.Forms.TabPage();
            this.clbWeekdays = new System.Windows.Forms.CheckedListBox();
            this.lstStopTime = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnDeleteStopTime = new System.Windows.Forms.Button();
            this.btnAddStopTime = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDay = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dtpBeginTime = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginDay = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnModifyStopTime = new System.Windows.Forms.Button();
            this.tbcSetting.SuspendLayout();
            this.tpgMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tpgStopTime.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcSetting
            // 
            this.tbcSetting.Controls.Add(this.tpgMain);
            this.tbcSetting.Controls.Add(this.tpgStopTime);
            this.tbcSetting.Location = new System.Drawing.Point(8, 9);
            this.tbcSetting.Name = "tbcSetting";
            this.tbcSetting.SelectedIndex = 0;
            this.tbcSetting.Size = new System.Drawing.Size(455, 263);
            this.tbcSetting.TabIndex = 0;
            // 
            // tpgMain
            // 
            this.tpgMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tpgMain.Controls.Add(this.chkAutoLogin);
            this.tpgMain.Controls.Add(this.groupBox1);
            this.tpgMain.Controls.Add(this.groupBox5);
            this.tpgMain.Controls.Add(this.groupBox4);
            this.tpgMain.Controls.Add(this.groupBox3);
            this.tpgMain.Location = new System.Drawing.Point(4, 22);
            this.tpgMain.Name = "tpgMain";
            this.tpgMain.Padding = new System.Windows.Forms.Padding(3);
            this.tpgMain.Size = new System.Drawing.Size(447, 237);
            this.tpgMain.TabIndex = 0;
            this.tpgMain.Text = "基本設定";
            // 
            // chkAutoLogin
            // 
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Location = new System.Drawing.Point(237, 197);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(155, 17);
            this.chkAutoLogin.TabIndex = 4;
            this.chkAutoLogin.Text = "起動時に自動起動します。";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtRetryCount);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtRetryInterval);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtOrderAmount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(17, 107);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 112);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " 注文設定 ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(175, 52);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(20, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "ms";
            // 
            // txtRetryCount
            // 
            this.txtRetryCount.Location = new System.Drawing.Point(87, 75);
            this.txtRetryCount.Name = "txtRetryCount";
            this.txtRetryCount.Size = new System.Drawing.Size(108, 20);
            this.txtRetryCount.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "再試行回数";
            // 
            // txtRetryInterval
            // 
            this.txtRetryInterval.Location = new System.Drawing.Point(87, 49);
            this.txtRetryInterval.Name = "txtRetryInterval";
            this.txtRetryInterval.Size = new System.Drawing.Size(82, 20);
            this.txtRetryInterval.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "再試行間隔";
            // 
            // txtOrderAmount
            // 
            this.txtOrderAmount.Location = new System.Drawing.Point(88, 22);
            this.txtOrderAmount.Name = "txtOrderAmount";
            this.txtOrderAmount.Size = new System.Drawing.Size(107, 20);
            this.txtOrderAmount.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "注文金額";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdbReal);
            this.groupBox5.Controls.Add(this.rdbDemo);
            this.groupBox5.Location = new System.Drawing.Point(237, 107);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(197, 78);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " レアル/デモ ";
            // 
            // rdbReal
            // 
            this.rdbReal.AutoSize = true;
            this.rdbReal.Location = new System.Drawing.Point(18, 48);
            this.rdbReal.Name = "rdbReal";
            this.rdbReal.Size = new System.Drawing.Size(53, 17);
            this.rdbReal.TabIndex = 1;
            this.rdbReal.TabStop = true;
            this.rdbReal.Text = "レアル";
            this.rdbReal.UseVisualStyleBackColor = true;
            // 
            // rdbDemo
            // 
            this.rdbDemo.AutoSize = true;
            this.rdbDemo.Location = new System.Drawing.Point(18, 25);
            this.rdbDemo.Name = "rdbDemo";
            this.rdbDemo.Size = new System.Drawing.Size(44, 17);
            this.rdbDemo.TabIndex = 0;
            this.rdbDemo.TabStop = true;
            this.rdbDemo.Text = "デモ";
            this.rdbDemo.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbHighLowSpread);
            this.groupBox4.Controls.Add(this.rdbHighLow);
            this.groupBox4.Location = new System.Drawing.Point(237, 18);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(197, 77);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " トレード形態 ";
            // 
            // rdbHighLowSpread
            // 
            this.rdbHighLowSpread.AutoSize = true;
            this.rdbHighLowSpread.Location = new System.Drawing.Point(18, 48);
            this.rdbHighLowSpread.Name = "rdbHighLowSpread";
            this.rdbHighLowSpread.Size = new System.Drawing.Size(104, 17);
            this.rdbHighLowSpread.TabIndex = 1;
            this.rdbHighLowSpread.TabStop = true;
            this.rdbHighLowSpread.Text = "HighLow Spread";
            this.rdbHighLowSpread.UseVisualStyleBackColor = true;
            // 
            // rdbHighLow
            // 
            this.rdbHighLow.AutoSize = true;
            this.rdbHighLow.Location = new System.Drawing.Point(18, 25);
            this.rdbHighLow.Name = "rdbHighLow";
            this.rdbHighLow.Size = new System.Drawing.Size(67, 17);
            this.rdbHighLow.TabIndex = 0;
            this.rdbHighLow.TabStop = true;
            this.rdbHighLow.Text = "HighLow";
            this.rdbHighLow.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPassword);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtLoginID);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(17, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 77);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ログイン設定 ";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(84, 45);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(111, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "パスワード";
            // 
            // txtLoginID
            // 
            this.txtLoginID.Location = new System.Drawing.Point(84, 19);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(111, 20);
            this.txtLoginID.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "ログインID";
            // 
            // tpgStopTime
            // 
            this.tpgStopTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tpgStopTime.Controls.Add(this.clbWeekdays);
            this.tpgStopTime.Controls.Add(this.lstStopTime);
            this.tpgStopTime.Controls.Add(this.groupBox6);
            this.tpgStopTime.Location = new System.Drawing.Point(4, 22);
            this.tpgStopTime.Name = "tpgStopTime";
            this.tpgStopTime.Padding = new System.Windows.Forms.Padding(3);
            this.tpgStopTime.Size = new System.Drawing.Size(447, 237);
            this.tpgStopTime.TabIndex = 1;
            this.tpgStopTime.Text = "停止時間帯";
            // 
            // clbWeekdays
            // 
            this.clbWeekdays.FormattingEnabled = true;
            this.clbWeekdays.Items.AddRange(new object[] {
            "月曜日",
            "火曜日",
            "水曜日",
            "木曜日",
            "金曜日",
            "土曜日",
            "日曜日"});
            this.clbWeekdays.Location = new System.Drawing.Point(312, 12);
            this.clbWeekdays.Name = "clbWeekdays";
            this.clbWeekdays.Size = new System.Drawing.Size(120, 214);
            this.clbWeekdays.TabIndex = 2;
            // 
            // lstStopTime
            // 
            this.lstStopTime.FormattingEnabled = true;
            this.lstStopTime.Location = new System.Drawing.Point(12, 144);
            this.lstStopTime.Name = "lstStopTime";
            this.lstStopTime.Size = new System.Drawing.Size(294, 82);
            this.lstStopTime.TabIndex = 1;
            this.lstStopTime.SelectedValueChanged += new System.EventHandler(this.lstStopTime_SelectedValueChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnModifyStopTime);
            this.groupBox6.Controls.Add(this.btnDeleteStopTime);
            this.groupBox6.Controls.Add(this.btnAddStopTime);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.dtpEndTime);
            this.groupBox6.Controls.Add(this.dtpEndDay);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.dtpBeginTime);
            this.groupBox6.Controls.Add(this.dtpBeginDay);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(12, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(294, 132);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " 停止時間追加 ";
            // 
            // btnDeleteStopTime
            // 
            this.btnDeleteStopTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteStopTime.Location = new System.Drawing.Point(208, 88);
            this.btnDeleteStopTime.Name = "btnDeleteStopTime";
            this.btnDeleteStopTime.Size = new System.Drawing.Size(69, 30);
            this.btnDeleteStopTime.TabIndex = 11;
            this.btnDeleteStopTime.Text = "削除";
            this.btnDeleteStopTime.UseVisualStyleBackColor = true;
            this.btnDeleteStopTime.Click += new System.EventHandler(this.btnDeleteStopTime_Click);
            // 
            // btnAddStopTime
            // 
            this.btnAddStopTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStopTime.Location = new System.Drawing.Point(208, 22);
            this.btnAddStopTime.Name = "btnAddStopTime";
            this.btnAddStopTime.Size = new System.Drawing.Size(69, 30);
            this.btnAddStopTime.TabIndex = 9;
            this.btnAddStopTime.Text = "追加";
            this.btnAddStopTime.UseVisualStyleBackColor = true;
            this.btnAddStopTime.Click += new System.EventHandler(this.btnAddStopTime_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(156, 107);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "まで";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Location = new System.Drawing.Point(78, 101);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(62, 20);
            this.dtpEndTime.TabIndex = 7;
            // 
            // dtpEndDay
            // 
            this.dtpEndDay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDay.Location = new System.Drawing.Point(78, 75);
            this.dtpEndDay.Name = "dtpEndDay";
            this.dtpEndDay.Size = new System.Drawing.Size(104, 20);
            this.dtpEndDay.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 78);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "完了日付";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(157, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "から";
            // 
            // dtpBeginTime
            // 
            this.dtpBeginTime.CustomFormat = "";
            this.dtpBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpBeginTime.Location = new System.Drawing.Point(78, 45);
            this.dtpBeginTime.Name = "dtpBeginTime";
            this.dtpBeginTime.Size = new System.Drawing.Size(62, 20);
            this.dtpBeginTime.TabIndex = 3;
            // 
            // dtpBeginDay
            // 
            this.dtpBeginDay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBeginDay.Location = new System.Drawing.Point(78, 19);
            this.dtpBeginDay.Name = "dtpBeginDay";
            this.dtpBeginDay.Size = new System.Drawing.Size(104, 20);
            this.dtpBeginDay.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "開始日付";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(210, 278);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 43);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "設定する";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(324, 278);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 43);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnModifyStopTime
            // 
            this.btnModifyStopTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifyStopTime.Location = new System.Drawing.Point(208, 55);
            this.btnModifyStopTime.Name = "btnModifyStopTime";
            this.btnModifyStopTime.Size = new System.Drawing.Size(69, 30);
            this.btnModifyStopTime.TabIndex = 12;
            this.btnModifyStopTime.Text = "変更";
            this.btnModifyStopTime.UseVisualStyleBackColor = true;
            this.btnModifyStopTime.Click += new System.EventHandler(this.btnModifyStopTime_Click);
            // 
            // MainSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(474, 330);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbcSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "設定";
            this.Load += new System.EventHandler(this.MainSetting_Load);
            this.tbcSetting.ResumeLayout(false);
            this.tpgMain.ResumeLayout(false);
            this.tpgMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tpgStopTime.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcSetting;
        private System.Windows.Forms.TabPage tpgMain;
        private System.Windows.Forms.TabPage tpgStopTime;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLoginID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rdbReal;
        private System.Windows.Forms.RadioButton rdbDemo;
        private System.Windows.Forms.RadioButton rdbHighLowSpread;
        private System.Windows.Forms.RadioButton rdbHighLow;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnDeleteStopTime;
        private System.Windows.Forms.Button btnAddStopTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndDay;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtpBeginTime;
        private System.Windows.Forms.DateTimePicker dtpBeginDay;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox lstStopTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtRetryCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRetryInterval;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOrderAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbWeekdays;
        private System.Windows.Forms.CheckBox chkAutoLogin;
        private System.Windows.Forms.Button btnModifyStopTime;
    }
}