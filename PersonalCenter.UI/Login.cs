using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.Utils;
using PersonalCenter.Service.User;
using DevExpress.LookAndFeel;
using System.Configuration;


namespace PersonalCenter.UI
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        private IUserLogic UserLogic;

        public Login()
        {
            InitializeComponent();

            string dbType = ConfigurationSettings.AppSettings["DBType"];

            if (dbType.Equals("Filesystem", StringComparison.CurrentCultureIgnoreCase))
                UserLogic = new PersonalCenter.Service.Impl.Filesystem.User.UserLogic();
            else
                UserLogic = new PersonalCenter.Service.Impl.Database.User.UserLogic();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (loginValidation.Validate())
            {
                try
                {
                    string account = accountEdit.Text.Trim();
                    string password = passwordEdit.Text.Trim();

                    var user = UserLogic.Login(account, password);

                    if (user != null)
                    {
                        this.Visible = false;
                        this.passwordEdit.Text = string.Empty;
                        if (!string.IsNullOrEmpty(user.Skin))
                            UserLookAndFeel.Default.SetSkinStyle(user.Skin);

                        MainForm main = new MainForm();
                        main.CurrUser = user;
                        main.Show();
                    }
                    else
                    {
                        ToolTipControllerShowEventArgs args = errorTip.CreateShowArgs();
                        args.ToolTip = "用户名或密码错误";
                        args.IconType = ToolTipIconType.Warning;
                        args.ImageIndex = -1;
                        args.IconSize = ToolTipIconSize.Small;

                        errorTip.ShowHint(args, accountEdit);
                        passwordEdit.Focus();
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}