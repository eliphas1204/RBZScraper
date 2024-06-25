using ExchangeRatesApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ExchangeRatesApp.Controllers
{
    public class ExchangeRatesController : Controller
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeRatesController()
        {
            _exchangeRateService = new ExchangeRateService();
        }

        public async Task<IActionResult> Index()
        {
            var rates = await _exchangeRateService.GetExchangeRates();
            return View(rates);
        }
    }
}