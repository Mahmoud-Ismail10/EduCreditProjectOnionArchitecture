namespace EduCredit.Service.Helper
{
    public class Pagination<T>
    {
        public Pagination(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }

        public int PageSize { get; set; } // 0
        public int PageIndex { get; set; } // 0
        public int Count { get; set; } // 0
        public IReadOnlyList<T> Data { get; set; } // null


    }
}
