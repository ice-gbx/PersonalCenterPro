using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalCenter.Service.Record
{
    public interface IRecordLogic
    {
        int Delete(int rid);

        void Load(PageView pageView);

        int Update(RecordView view);

        int Create(RecordView view);
    }
}
