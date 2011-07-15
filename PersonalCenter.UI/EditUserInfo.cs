using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using PersonalCenter.Service.User;
using System.Configuration;
namespace PersonalCenter.UI
{
    public partial class EditUserInfo : DevExpress.XtraEditors.XtraForm
    {
        private IUserLogic UserLogic;

        public EditUserInfo()
        {
            InitializeComponent();

            string dbType = ConfigurationSettings.AppSettings["DBType"];

            if (dbType.Equals("Filesystem", StringComparison.CurrentCultureIgnoreCase))
                UserLogic = new PersonalCenter.Service.Impl.Filesystem.User.UserLogic();
            else
                UserLogic = new PersonalCenter.Service.Impl.Database.User.UserLogic();
        }

        public UserView UserInfo { get; set; }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (updateValidation.Validate())
            {
                UserInfo.Account = txtAccount.Text.Trim();
                UserInfo.Uname = txtName.Text.Trim();
                UserInfo.Email = txtEmail.Text.Trim();

                if (!string.IsNullOrEmpty(oldPassEdit.Text.Trim()))
                {
                    if (oldPassEdit.Text.Trim() == UserInfo.Password)
                    {
                        if (!string.IsNullOrEmpty(newPassEdit.Text.Trim()))
                        {
                            UserInfo.Password = newPassEdit.Text.Trim();
                        }
                        else
                        {
                            XtraMessageBox.Show("新密码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("旧密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        oldPassEdit.Focus();
                        return;
                    }
                }
                UserLogic.EditUserInfo(UserInfo);
                XtraMessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void EditUserInfo_Load(object sender, EventArgs e)
        {
            txtAccount.Text = UserInfo.Account;
            txtName.Text = UserInfo.Uname;
            txtEmail.Text = UserInfo.Email;
        }

    }
}