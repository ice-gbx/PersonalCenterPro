using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraEditors;
using PersonalCenter.Service.User;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using PersonalCenter.Service.Record;
using System.Configuration;


namespace PersonalCenter.UI
{
    public partial class MainForm : RibbonForm
    {
        private IRecordLogic RecordLogic;

        private IUserLogic UserLogic;

        public UserView CurrUser { get; set; }

        private PageView Pager;

        public MainForm()
        {
            InitializeComponent();
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        public void Init()
        {
            RecordLogic.Load(Pager);
            gcRecord.DataSource = Pager.Records;

            lblPageInfo.Text = string.Format("当前第{0}页，共{1}页 (本页共{2}条记录)", Pager.CurrPage, Pager.PageCount, Pager.Records.Count);

            int beginPage = Pager.CurrPage;
            lblPN1.Enabled = true;
            lblPN2.Enabled = true;
            lblPN3.Enabled = true;
            lblPN4.Enabled = true;
            lblPN5.Enabled = true;
            lblPN1.ForeColor = Color.Black;
            lblPN2.ForeColor = Color.Black;
            lblPN3.ForeColor = Color.Black;
            lblPN4.ForeColor = Color.Black;
            lblPN5.ForeColor = Color.Black;

            if (beginPage < 3)
            {
                lblPN1.Text = " 1 ";
                lblPN2.Text = " 2 ";
                lblPN3.Text = " 3 ";
                lblPN4.Text = " 4 ";
                lblPN5.Text = " 5 ";

            }
            else
            {
                lblPN1.Text = string.Format(" {0} ", beginPage - 2);
                lblPN2.Text = string.Format(" {0} ", beginPage - 1);
                lblPN3.Text = string.Format(" {0} ", beginPage);
                lblPN4.Text = string.Format(" {0} ", beginPage + 1);
                lblPN5.Text = string.Format(" {0} ", beginPage + 2);

                if (Pager.PageCount == beginPage)
                {
                    lblPN4.Enabled = false;
                    lblPN5.Enabled = false;
                    lblPN4.ForeColor = Color.Gray;
                    lblPN5.ForeColor = Color.Gray;
                }
                else if (Pager.PageCount == beginPage + 1)
                {
                    lblPN5.Enabled = false;
                    lblPN5.ForeColor = Color.Gray;
                }
            }

            if (Pager.PageCount > 5)
                return;

            switch (Pager.PageCount)
            {
                case 0:
                    lblPN1.Enabled = false;
                    lblPN2.Enabled = false;
                    lblPN3.Enabled = false;
                    lblPN4.Enabled = false;
                    lblPN5.Enabled = false;
                    lblPN1.ForeColor = Color.Gray;
                    lblPN2.ForeColor = Color.Gray;
                    lblPN3.ForeColor = Color.Gray;
                    lblPN4.ForeColor = Color.Gray;
                    lblPN5.ForeColor = Color.Gray;
                    break;
                case 1:
                    lblPN2.Enabled = false;
                    lblPN3.Enabled = false;
                    lblPN4.Enabled = false;
                    lblPN5.Enabled = false;
                    lblPN2.ForeColor = Color.Gray;
                    lblPN3.ForeColor = Color.Gray;
                    lblPN4.ForeColor = Color.Gray;
                    lblPN5.ForeColor = Color.Gray;
                    break;
                case 2:
                    lblPN3.Enabled = false;
                    lblPN4.Enabled = false;
                    lblPN5.Enabled = false;
                    lblPN3.ForeColor = Color.Gray;
                    lblPN4.ForeColor = Color.Gray;
                    lblPN5.ForeColor = Color.Gray;
                    break;
                case 3:
                    lblPN4.Enabled = false;
                    lblPN5.Enabled = false;
                    lblPN4.ForeColor = Color.Gray;
                    lblPN5.ForeColor = Color.Gray;
                    break;
                case 4:
                    lblPN5.Enabled = false;
                    lblPN5.ForeColor = Color.Gray;
                    break;
                default:
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string dbType = ConfigurationSettings.AppSettings["DBType"];

            if (dbType.Equals("Filesystem", StringComparison.CurrentCultureIgnoreCase))
            {
                RecordLogic = new PersonalCenter.Service.Impl.Filesystem.Record.RecordLogic();
                UserLogic = new PersonalCenter.Service.Impl.Filesystem.User.UserLogic();
            }
            else
            {
                RecordLogic = new PersonalCenter.Service.Impl.Database.Record.RecordLogic();
                UserLogic = new PersonalCenter.Service.Impl.Database.User.UserLogic();
            }

            Pager = new PageView();

            Init();

            siStatus.Caption = string.Format("您好,{0}！", CurrUser.Uname);
            siDate.Caption = string.Format(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        private void iExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void createItem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            CreateRecord create = new CreateRecord();
            create.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            siDate.Caption = string.Format(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        private void btnModifyPwd_ItemClick(object sender, ItemClickEventArgs e)
        {
            EditUserInfo up = new EditUserInfo();
            up.UserInfo = CurrUser;
            up.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = XtraMessageBox.Show("确定要退出？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (res == DialogResult.OK)
                Application.OpenForms["Login"].Close();
            else
                e.Cancel = true;
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pager.PageSize = int.Parse(cboPageSize.Text);
            Pager.CurrPage = 1;
            gcRecord.Focus();
            Init();
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            Pager.CurrPage--;
            Init();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Pager.CurrPage++;
            Init();
        }

        private void nbCreate_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            CreateRecord cr = new CreateRecord();
            cr.CurrUserId = CurrUser.Id;
            cr.MainForm = this;
            cr.Show();
        }

        private void bnExport_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                using (SaveFileDialog fd = new SaveFileDialog())
                {
                    fd.Title = "导出Excel文件";
                    fd.RestoreDirectory = true;
                    fd.Filter = "excel文件|*.xls";
                    fd.FilterIndex = 1;

                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        gcRecord.ExportToXls(fd.FileName);
                        XtraMessageBox.Show("文件导出成功", "导出", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "导出", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void nbDelete_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            DialogResult res = XtraMessageBox.Show("确定要删除?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Cancel)
                return;

            int[] selectedRowIndexs = gridView1.GetSelectedRows();
            foreach (int index in selectedRowIndexs)
            {
                RecordView temp = (RecordView)gridView1.GetRow(index);
                int rid = temp.Id;
                RecordLogic.Delete(rid);
            }

            gridView1.DeleteSelectedRows();

            Pager.CurrPage = 1;
            Init();
        }

        private void bnRefresh_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Init();
        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            var currRow = (RecordView)e.Row;

            if (currRow != null)
            {
                RecordLogic.Update(currRow);
                Init();
            }
        }

        private void lblPN1_Click(object sender, EventArgs e)
        {
            LabelControl lbl = sender as LabelControl;
            Pager.CurrPage = int.Parse(lbl.Text.Trim());
            Init();
        }

        private void lblPN1_MouseMove(object sender, MouseEventArgs e)
        {
            LabelControl lbl = sender as LabelControl;
            lbl.ForeColor = Color.SteelBlue;
        }

        private void lblPN1_MouseLeave(object sender, EventArgs e)
        {
            LabelControl lbl = sender as LabelControl;
            lbl.ForeColor = Color.Black;
        }

        private bool IsFirst = true;
        private void rgbiSkins_Gallery_ItemCheckedChanged(object sender, GalleryItemEventArgs e)
        {
            if (!IsFirst)
            {
                CurrUser.Skin = e.Item.Tag.ToString();
                UserLogic.ChangeSkin(CurrUser);
            }
            IsFirst = false;
        }
    }
}