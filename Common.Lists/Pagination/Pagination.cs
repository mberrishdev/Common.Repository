using Microsoft.EntityFrameworkCore;

namespace Common.Lists.Pagination
{
    public static class Pagination
    {
        public static async Task<PagedList<TItem>?> Paginate<TItem>(this IQueryable<TItem> collection, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            where TItem : class
        {
            if (collection == null)
                return null;

            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            if (!collection.Any())
                return PagedList<TItem>.Empty;

            int totalResults = collection.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalResults / pageSize);

            int skip = (pageIndex - 1) * pageSize;
            List<TItem> data = await collection.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
            return PagedList<TItem>.Create(data, pageIndex, pageSize, totalPages, totalResults);
        }
    }

    public class DomainPagedQueryBase
    {
        public int Page { get; set; }
        public int Results { get; set; }
        public DomainPagedQueryBase(int page, int result)
        {
            Page = page;
            Results = result;
        }
    }

    public abstract class DomainPagedResultBase
    {
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }
        protected DomainPagedResultBase()
        {
        }
        protected DomainPagedResultBase(int currentPage, int resultsPerPage, int totalPages, long totalResults)
        {
            CurrentPage = currentPage > totalPages ? totalPages : currentPage;
            ResultsPerPage = resultsPerPage;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }
    }
}