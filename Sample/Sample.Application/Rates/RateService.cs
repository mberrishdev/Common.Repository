using Common.Repository.Repository;
using Common.Repository.UnitOfWork;
using Sample.Domain.Rates;
using Sample.Domain.Rates.Commands;

namespace Sample.Application.Rates
{
    public class RateService : IRateService
    {
        private readonly IRepository<Rate> _repository;
        private readonly IQueryRepository<Rate> _queryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RateService(IRepository<Rate> repository, IQueryRepository<Rate> queryRepository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _queryRepository = queryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task SaveRates(SaveRatesCommand command, CancellationToken cancellationToken)
        {
            var ratesFromUser = command.Rates;

            using var scope = await _unitOfWork.CreateScopeAsync(cancellationToken);
            foreach (var rateEntity in ratesFromUser.Select(rate => new Rate(rate)))
            {
                await _repository.InsertAsync(rateEntity, cancellationToken);
            }
            await scope.CompletAsync(cancellationToken);
        }

        public async Task<List<Rate>> GetRates(CancellationToken cancellationToken)
        {
            return await _queryRepository.GetListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Rate> GetById(int id, CancellationToken cancellationToken)
        {
            return await _queryRepository.GetAsync(predicate:x=>x.Id == id, cancellationToken: cancellationToken);
        }

        public async Task Update(int id, SaveRateCommand command, CancellationToken cancellationToken)
        {
            var rate =  await _repository.GetForUpdateAsync(predicate:x=>x.Id == id, cancellationToken: cancellationToken);

            rate.Update(command);
            await _repository.UpdateAsync(rate, cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var rate =  await _repository.GetForUpdateAsync(predicate:x=>x.Id == id, cancellationToken: cancellationToken);
            await _repository.DeleteAsync(rate, cancellationToken);
        }
    }
}
