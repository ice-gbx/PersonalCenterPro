using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalCenter.Service.Record
{
    [Serializable]
    public class PageView
    {
        public PageView()
        {
            Total = 0;
            PageSize = 20;
            currPage = 1;
        }

        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<RecordView> Records { get; set; }

        public int PageCount { get; private set; }

        public void SetPageCount()
        {
            if (Total > 0)
                PageCount = Total % PageSize == 0 ? Total / PageSize : Total / PageSize + 1;
            else
                PageCount = 0;
        }

        private int currPage;

        public int CurrPage
        {
            get { return currPage; }
            set 
            {
                if (PageCount == 0)
                    currPage = 0;
                else if (value < 1)
                    currPage = 1;
                else if (value > PageCount)
                    currPage = PageCount;
                else
                    currPage = value;
            }
        }
    
    }
}
