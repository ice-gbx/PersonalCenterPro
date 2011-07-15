using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonalCenter.Service.User;

namespace PersonalCenter.Service.Record
{
    [Serializable]
    public class RecordView
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
        public int Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifyDate { get; set; }
    }
}
