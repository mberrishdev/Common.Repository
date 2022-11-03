namespace Common.Lists.Pagination
{
    public class PagedList<TItem> : PagedListBase
    {
        public List<TItem> List { get; set; }
        public bool IsEmpty => List == null || !List.Any();

        protected PagedList()
        {
            List = new List<TItem>(0);
        }

        protected PagedList(List<TItem> source, int currentPage, int resultsPerPage, int totalPages, long totalResults)
            : base(currentPage, resultsPerPage, totalPages, totalResults)
        {
            List = source;
        }
        public static PagedList<TItem> Create(List<TItem> items, int currentPage, int resultsPerPage, int totalPages, long totalResults)
        {
            return new PagedList<TItem>(items, currentPage, resultsPerPage, totalPages, totalResults);
        }

        public static PagedList<TItem> Empty => new();
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
