namespace Common.Repository.UnitOfWork
{
    public interface IUnitOfWorkScope
    {
        Task CompletAsync(CancellationToken cancellationToken = default);
    }
}
