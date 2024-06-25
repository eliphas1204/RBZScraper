using System.Text.Json.Serialization;
using ExchangeRatesApp.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RBZScraper.Controllers.Api;

[ApiController]
[Route("api/rates")]
public class Rates(ExchangeRateService exchangeRateService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string? formatted)
    {
        return Ok(formatted != null ? JsonConvert.SerializeObject(await exchangeRateService.FormatRates()) : JsonConvert.SerializeObject(await exchangeRateService.GetExchangeRates()));
    }
    
}