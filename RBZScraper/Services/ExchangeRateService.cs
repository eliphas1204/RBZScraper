using HtmlAgilityPack;
using System.Collections.Generic;
using RBZScraper.Model;

namespace ExchangeRatesApp.Services
{
    
    public class ExchangeRateService
    {
        public async Task<List<ExchangeRateResponse>> GetExchangeRates(bool format = false)
        {
            var doc = await SendRequest();

            var rates = new List<ExchangeRateResponse>
            {
                GetExchangeRate(doc, "//div[@id='baTab1']//table//tr[5]"),
                GetExchangeRate(doc, "//div[@id='baTab1']//table//tr[6]"),
                GetExchangeRate(doc, "//div[@id='baTab1']//table//tr[7]"),
                GetExchangeRate(doc, "//div[@id='baTab1']//table//tr[8]"),
                GetExchangeRate(doc, "//div[@id='baTab1']//table//tr[9]")
                
            };

            return rates;
        }

        private static async Task<HtmlDocument> SendRequest()
        {
            
            var client = new HttpClient(); 
        
            var htmlString =  await client.GetStringAsync("https://www.rbz.co.zw/index.php/13-daily-exchange-rates");
        
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            return doc;
        }

        public async Task<Dictionary<string, decimal>> FormatRates()
        {
            var doc = await SendRequest();
            var rates = new Dictionary<string, decimal>
            {
                { "USD", GetAverageRate(doc, "//div[@id='baTab1']//table//tr[5]//td[4]/span") },
                { "GBP", GetAverageRate(doc, "//div[@id='baTab1']//table//tr[6]//td[4]/span") },
                { "EUR", GetAverageRate(doc, "//div[@id='baTab1']//table//tr[7]//td[4]/span") },
                { "ZAR", GetAverageRate(doc, "//div[@id='baTab1']//table//tr[8]//td[4]/span") },
                { "BWP", GetAverageRate(doc, "//div[@id='baTab1']//table//tr[9]//td[4]/span") }
            };

            return rates;
        }
        private decimal GetAverageRate(HtmlDocument doc, string xPath)
        {
            var avgRateNode = doc.DocumentNode.SelectSingleNode(xPath);
            return avgRateNode != null ? decimal.Parse(avgRateNode.InnerText.Trim()) : 0;
        }
        private ExchangeRateResponse GetExchangeRate(HtmlDocument doc, string xPath)
        {
            var currency = doc.DocumentNode.SelectSingleNode($"{xPath}//td[1]/span/strong").InnerText.Trim();
            var bid = decimal.Parse(doc.DocumentNode.SelectSingleNode($"{xPath}//td[2]/span").InnerText.Trim());
            var ask = decimal.Parse(doc.DocumentNode.SelectSingleNode($"{xPath}//td[3]/span").InnerText.Trim());
            var avg = decimal.Parse(doc.DocumentNode.SelectSingleNode($"{xPath}//td[4]/span").InnerText.Trim());

            return new ExchangeRateResponse
            {
                Currency = currency,
                Bid = bid,
                Ask = ask,
                Avg = avg
            };
        }
    }
}