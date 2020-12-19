using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLTrader
{
    public partial class MainSetting : Form
    {
        public MainSetting()
        {
            InitializeComponent();
        }

        private void MainSetting_Load(object sender, EventArgs e)
        {
            InitInterface();
        }
        
        private void InitInterface()
        {
            dtpBeginDay.Format = DateTimePickerFormat.Custom;
            dtpBeginDay.CustomFormat = "yyyy/MM/dd";
            dtpEndDay.Format = DateTimePickerFormat.Custom;
            dtpEndDay.CustomFormat = "yyyy/MM/dd";
            dtpBeginTime.Format = DateTimePickerFormat.Custom;
            dtpBeginTime.CustomFormat = "HH:mm";
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.CustomFormat = "HH:mm";

            txtLoginID.Text = CGlobalVar.g_strLoginID;
            txtPassword.Text = CGlobalVar.g_strLoginPass;

            txtOrderAmount.Text = CGlobalVar.g_nOrderAmount.ToString();
            txtRetryCount.Text = CGlobalVar.g_nRetryCount.ToString();
            txtRetryInterval.Text = CGlobalVar.g_nRetryInterval.ToString();

            if (CGlobalVar.g_nTransMode == TransMode.HighLow)
                rdbHighLow.Checked = true;
            else if (CGlobalVar.g_nTransMode == TransMode.HighLowSpread)
                rdbHighLowSpread.Checked = true;
            if (CGlobalVar.g_nDemoReal == DemoReal.Demo)
                rdbDemo.Checked = true;
            else
                rdbReal.Checked = true;
            chkAutoLogin.Checked = CGlobalVar.g_bAutoLogin;

            for (int i = 0; i < 7; i ++)
            {
                clbWeekdays.SetItemChecked(i, CGlobalVar.g_lnWeekdays[i]);
            }

            lstStopTime.Items.Clear();
            for (int i = 1; i <= CGlobalVar.g_lstStopTimes.Count; i ++)
            {
                StopTime st = CGlobalVar.g_lstStopTimes[i - 1];

                AddListItem(st);
            }
        }

        private void AddListItem(StopTime st, int nSelIndex = -1)
        {
            int nCount = lstStopTime.Items.Count + 1;
            string strItem = nCount.ToString() + " - [" + st.strBeginDay + " " + st.strBeginTime + " ~ " + st.strEndDay + " " + st.strEndTime + "]";
            if (nSelIndex >= 0)
                lstStopTime.Items.Insert(nSelIndex, strItem);
            else
                lstStopTime.Items.Add(strItem);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CGlobalVar.g_strLoginID = txtLoginID.Text;
            CGlobalVar.g_strLoginPass = txtPassword.Text;

            int.TryParse(txtOrderAmount.Text, out CGlobalVar.g_nOrderAmount);
            int.TryParse(txtRetryCount.Text, out CGlobalVar.g_nRetryCount);
            int.TryParse(txtRetryInterval.Text, out CGlobalVar.g_nRetryInterval);

            if (rdbHighLow.Checked == true)
                CGlobalVar.g_nTransMode = TransMode.HighLow;
            else
                CGlobalVar.g_nTransMode = TransMode.HighLowSpread;
            if (rdbDemo.Checked == true)
                CGlobalVar.g_nDemoReal = DemoReal.Demo;
            else
                CGlobalVar.g_nDemoReal = DemoReal.Real;

            CGlobalVar.g_bAutoLogin = chkAutoLogin.Checked;

            for (int i = 0; i < 7; i ++)
            {
                CGlobalVar.g_lnWeekdays[i] = clbWeekdays.GetItemChecked(i);
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAddStopTime_Click(object sender, EventArgs e)
        {
            StopTime st;

            st.strBeginDay = dtpBeginDay.Text;
            st.strBeginTime = dtpBeginTime.Text;
            st.strEndDay = dtpEndDay.Text;
            st.strEndTime = dtpEndTime.Text;

            int nDayCmp = st.strBeginDay.CompareTo(st.strEndDay);
            int nTimeCmp = st.strBeginTime.CompareTo(st.strEndTime);
            if (nDayCmp > 0 || (nDayCmp == 0 && nTimeCmp >= 0))
            {
                MessageBox.Show("日付を正確に入力してください。");
                return;
            }

            CGlobalVar.g_lstStopTimes.Add(st);
            AddListItem(st);
        }

        private void btnModifyStopTime_Click(object sender, EventArgs e)
        {
            int nSelIndex = lstStopTime.SelectedIndex;
            if (nSelIndex < 0) return;

            if (MessageBox.Show("日付を本当に変更しますか?", "警告", MessageBoxButtons.YesNo) == DialogResult.No) return;

            StopTime st;
            st.strBeginDay = dtpBeginDay.Text;
            st.strBeginTime = dtpBeginTime.Text;
            st.strEndDay = dtpEndDay.Text;
            st.strEndTime = dtpEndTime.Text;

            CGlobalVar.g_lstStopTimes.RemoveAt(nSelIndex);
            CGlobalVar.g_lstStopTimes.Insert(nSelIndex, st);
            lstStopTime.Items.RemoveAt(nSelIndex);
            AddListItem(st, nSelIndex);
        }

        private void btnDeleteStopTime_Click(object sender, EventArgs e)
        {
            int nSelIndex = lstStopTime.SelectedIndex;
            if (nSelIndex < 0) return;

            if (MessageBox.Show("日付を本当に削除しますか?", "警告", MessageBoxButtons.YesNo) == DialogResult.No) return;

            StopTime st = CGlobalVar.g_lstStopTimes[nSelIndex];
            CGlobalVar.g_lstStopTimes.Remove(st);
            lstStopTime.Items.RemoveAt(nSelIndex);
        }

        private void lstStopTime_SelectedValueChanged(object sender, EventArgs e)
        {
            int nSelIndex = lstStopTime.SelectedIndex;
            if (nSelIndex < 0)
            {
                return;
            }

            StopTime st = CGlobalVar.g_lstStopTimes[nSelIndex];
            dtpBeginDay.Text = st.strBeginDay;
            dtpBeginTime.Text = st.strBeginTime;
            dtpEndDay.Text = st.strEndDay;
            dtpEndTime.Text = st.strEndTime;
        }
    }
}
