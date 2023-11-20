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

        [HttpPut("id")]
        public async Task<ActionResult> Update(int id, SaveRateCommand command, CancellationToken cancellationToken)
        {
            await _rateService.Update(id, command, cancellationToken);
            return Ok();
        }


        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _rateService.Delete(id, cancellationToken);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await _rateService.GetRates(cancellationToken));
        }

        [HttpGet("id")]
        public async Task<ActionResult> Get(int id, CancellationToken cancellationToken)
        {
            return Ok(await _rateService.GetById(id, cancellationToken));
        }
    }
}