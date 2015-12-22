using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Untility
{
    public class DataPage<T>
    {
        private int totalCounts;
        private List<T> currentPage;
        /// <summary>
        /// 页数(从1开始)
        /// </summary>
        private int pageIndex;
        /// <summary>
        /// 页数(从1开始)
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        private int pageSize;
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public DataPage()
        {
        }

        public DataPage(int counts, List<T> page)
        {
            totalCounts = counts;
            currentPage = page;
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCounts
        {
            get { return totalCounts; }

            set
            {
                totalCounts = value;
            }
        }
        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
            }
        }
    }
}
