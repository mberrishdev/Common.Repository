namespace Common.Repository.EfCore.UnitOfWork
{
    public interface IUnitOfWorkScope
    {
        Task CompletAsync(CancellationToken cancellationToken = default);
    }
}
