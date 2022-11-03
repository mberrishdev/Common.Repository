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

            using (var scope = await _unitOfWork.CreateScopeAsync(cancellationToken))
            {
                foreach (var rate in ratesFromUser)
                {
                    var rateEntity = new Rate(rate);
                    await _repository.InsertAsync(rateEntity, cancellationToken);
                }
                await scope.CompletAsync(cancellationToken);
            }
        }
    }
}
