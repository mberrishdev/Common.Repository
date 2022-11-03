using Microsoft.AspNetCore.Mvc;
using Sample.Application.Rates;
using Sample.Domain.Rates.Commands;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;

        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpPost]
        public async Task<ActionResult> SaveRates(SaveRatesCommand command, CancellationToken cancellationToken)
        {
            await _rateService.SaveRates(command, cancellationToken);
            return Ok();
        }
    }
}
