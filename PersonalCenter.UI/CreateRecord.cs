using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PersonalCenter.Service.User;
using PersonalCenter.Service.Record;
using System.Configuration;

namespace PersonalCenter.UI
{
    public partial class CreateRecord : DevExpress.XtraEditors.XtraForm
    {
        public int CurrUserId { get; set; }

        public MainForm MainForm { get; set; }

        private IRecordLogic RecordLogic;

        public CreateRecord()
        {
            InitializeComponent();

            string dbType = ConfigurationSettings.AppSettings["DBType"];

            if (dbType.Equals("Filesystem", StringComparison.CurrentCultureIgnoreCase))
                RecordLogic = new PersonalCenter.Service.Impl.Filesystem.Record.RecordLogic();
            else
                RecordLogic = new PersonalCenter.Service.Impl.Database.Record.RecordLogic();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtAccount.Text = string.Empty;
            txtType.Text = string.Empty;
            txtRemark.Text = string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (createValidation.Validate())
            {
                RecordView record = new RecordView();
                record.Account = txtAccount.Text.Trim();
                record.Type = txtType.Text.Trim();
                record.Remark = txtRemark.Text.Trim();
                record.CreateDate = DateTime.Now;
                record.LastModifyDate = DateTime.Now;
                record.Creator = CurrUserId;

                try
                {
                    int res = RecordLogic.Create(record);

                    if (res > 0)
                    {
                        XtraMessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MainForm.Init();
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("添加失败,请重试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
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