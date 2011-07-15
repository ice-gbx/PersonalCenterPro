using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonalCenter.Service.Record;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace PersonalCenter.Service.Impl.Filesystem.Record
{
    public class RecordLogic : IRecordLogic
    {
        private string path = Process.GetCurrentProcess().MainModule.FileName.
            Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf("\\")) + @"\data.bin";
        private List<RecordView> Records;

        public RecordLogic()
        {
            Records = new List<RecordView>();

            try
            {
                Records = SerializeUtils.Deserialize(path) as List<RecordView>;
            }
            catch (SerializationException)
            {
                Init();
            }
        }

        public int Delete(int rid)
        {
            var record = Records.Where(n => n.Id == rid).FirstOrDefault();
            if (record != null)
            {
                Records.Remove(record);
                SerializeUtils.Serialize(path, Records);
                return 1;
            }
            return 0;
        }

        public void Load(PageView pageView)
        {
            Records = SerializeUtils.Deserialize(path) as List<RecordView>;

            int skipCount = pageView.PageSize * (pageView.CurrPage - 1);
            pageView.Total = Records.Count();
            pageView.SetPageCount();
            if (pageView.PageCount == 0)
                pageView.CurrPage = 0;
            pageView.Records = Records.OrderByDescending(n => n.CreateDate).Skip(skipCount).Take(pageView.PageSize).ToList();
        }


        public int Update(RecordView view)
        {
            var record = Records.Where(n => n.Id == view.Id).FirstOrDefault();

            if (record != null)
            {
                record.Account = view.Account;
                record.Type = view.Type;
                record.Remark = view.Remark;
                record.LastModifyDate = DateTime.Now;
            }

            return SerializeUtils.Serialize(path, Records);
        }


        public int Create(RecordView view)
        {
            try
            {
                view.Id = GetNextID();
                Records.Add(view);
                SerializeUtils.Serialize(path, Records);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void Init()
        {
            List<RecordView> records = new List<RecordView>();
            SerializeUtils.Serialize(path, records);
        }

        private int GetNextID()
        {
            return Records.OrderByDescending(n => n.Id).Select(n => n.Id).FirstOrDefault() + 1;
        }
    }
}
