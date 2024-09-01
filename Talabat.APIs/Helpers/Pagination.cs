namespace Talabat.APIs.Helpers
{
    public class Pagination<T>
    {

        public Pagination(int _pageSize , int _pageIndex , int _count  , IReadOnlyList<T> _data)
        {
            PageSize = _pageSize ;
            PageIndex = _pageIndex ;
            Count = _count ;
            Data = _data;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T>? Data { get; set; }
    }
}
