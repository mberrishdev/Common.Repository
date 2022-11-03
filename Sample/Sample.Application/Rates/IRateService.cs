using Sample.Domain.Rates.Commands;

namespace Sample.Application.Rates
{
    public interface IRateService
    {
        Task SaveRates(SaveRatesCommand command, CancellationToken cancellationToken);
    }
}
