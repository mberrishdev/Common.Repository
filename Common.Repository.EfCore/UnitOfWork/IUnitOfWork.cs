namespace Common.Repository.EfCore.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<IUnitOfWorkScope> CreateScopeAsync(CancellationToken cancellationToken = default);
    }
}
