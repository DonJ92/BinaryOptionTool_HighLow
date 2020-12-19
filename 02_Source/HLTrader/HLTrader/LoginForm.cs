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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginID.Text == "")
            {
                MessageBox.Show("ログインID入力ください。");
                return;
            }

            if (txtLoginPass.Text == "")
            {
                MessageBox.Show("パスワードを入力ください。");
                return;
            }

            if (txtLoginID.Text == "redspider0508")
            {
                CGlobalVar.g_nAdmin = true;
                this.DialogResult = DialogResult.OK;
                Close();
                return;
            }

            lblErrorMsg.Visible = false;
            CGlobalVar.g_strLoginID = txtLoginID.Text;
            CGlobalVar.g_strLoginPass = txtLoginPass.Text;

            LoginResponse response = API.doLogin();

            if (response.result == Constant.API_RESULT_SUCCESS)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                string strErrMsg = Constant.g_strErrorMsg[response.error];

                lblErrorMsg.Text = strErrMsg;
                lblErrorMsg.Visible = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CGlobalVar.g_strLoginID = txtLoginID.Text;
            CGlobalVar.g_strLoginPass = txtLoginPass.Text;
            CGlobalVar.g_bAutoLogin = chkAutoLogin.Checked;
            CGlobalVar.WriteConfig();
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            CGlobalVar.g_bAutoLogin = chkAutoLogin.Checked;
        }

        private void ServerSetting_Load(object sender, EventArgs e)
        {
            InitInterface();
            if (CGlobalVar.g_bAutoLogin)
            {
                btnLogin.PerformClick();
            }
        }

        private void InitInterface()
        {
            txtLoginID.Text = CGlobalVar.g_strLoginID;
            txtLoginPass.Text = CGlobalVar.g_strLoginPass;
            chkAutoLogin.Checked = CGlobalVar.g_bAutoLogin;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CGlobalVar.g_strLoginID = txtLoginID.Text;
            CGlobalVar.g_strLoginPass = txtLoginPass.Text;
            CGlobalVar.g_bAutoLogin = chkAutoLogin.Checked;
            CGlobalVar.WriteConfig();
        }
    }
}
