namespace Common.Repository.EfCore.Pagination
{
    public class PagedList<T> : PagedListBase
    {
        public List<T> List { get; set; }
        public bool IsEmpty => List == null || !List.Any();

        protected PagedList()
        {
            List = new List<T>(0);
        }

        protected PagedList(List<T> source, int currentPage, int resultsPerPage, int totalPages, long totalResults)
            : base(currentPage, resultsPerPage, totalPages, totalResults)
        {
            List = source;
        }
        public static PagedList<T> Create(List<T> items, int currentPage, int resultsPerPage, int totalPages, long totalResults)
        {
            return new PagedList<T>(items, currentPage, resultsPerPage, totalPages, totalResults);
        }

        public static PagedList<T> Empty => new();
    }

    public abstract class PagedListBase
    {
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }
        protected PagedListBase()
        {
        }
        protected PagedListBase(int currentPage, int resultsPerPage, int totalPages, long totalResults)
        {
            CurrentPage = currentPage > totalPages ? totalPages : currentPage;
            ResultsPerPage = resultsPerPage;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }
    }
}
