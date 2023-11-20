using Sample.Domain.Rates;
using Sample.Domain.Rates.Commands;

namespace Sample.Application.Rates
{
    public interface IRateService
    {
        Task SaveRates(SaveRatesCommand command, CancellationToken cancellationToken);
        Task<List<Rate>> GetRates(CancellationToken cancellationToken);
        Task<Rate> GetById(int id, CancellationToken cancellationToken);
        Task Update(int id, SaveRateCommand command, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}
