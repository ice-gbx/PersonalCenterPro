using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonalCenter.Service.Record;
using PersonalCenter.Entity;

namespace PersonalCenter.Service.Impl.Database.Record
{
    public class RecordLogic : IRecordLogic
    {
        private PersonalCenterEntities Context;

        public RecordLogic()
        {
            Context = new PersonalCenterEntities();
        }

        public int Delete(int rid)
        {
            var record = Context.Records.Where(n => n.Id == rid).FirstOrDefault();
            Context.DeleteObject(record);
            return Context.SaveChanges();
        }

        public void Load(PageView pageView)
        {
            int skipCount = pageView.PageSize * (pageView.CurrPage - 1);

            var query = from r in Context.Records
                        select new RecordView
                        {
                            Id = r.Id,
                            Account = r.Account,
                            Type = r.Type,
                            Creator = r.Users.Id,
                            CreateDate = r.CreateDate,
                            LastModifyDate = r.LastModifyDate,
                            Remark = r.Remark
                        };

            pageView.Total = query.Count();
            pageView.SetPageCount();
            if (pageView.PageCount == 0)
                pageView.CurrPage = 0;
            pageView.Records = query.ToList().Skip(skipCount).Take(pageView.PageSize).ToList();
        }


        public int Update(RecordView view)
        {
            var record = Context.Records.Where(n => n.Id == view.Id).FirstOrDefault();

            record.Account = view.Account;
            record.Type = view.Type;
            record.Remark = view.Remark;
            record.LastModifyDate = DateTime.Now;

            return Context.SaveChanges();
        }


        public int Create(RecordView view)
        {
            var user = Context.Users.Where(n => n.Id == view.Creator).FirstOrDefault();

            Records record = new Records()
                             {
                                 Account = view.Account,
                                 Type = view.Type,
                                 Remark = view.Remark,
                                 CreateDate = view.CreateDate,
                                 LastModifyDate = view.LastModifyDate,
                                 Users = user,
                             };
            Context.AddToRecords(record);
            return Context.SaveChanges();
        }
    }
}
