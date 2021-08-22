using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eClaim.Models;
using eClaim.Interfaces;
using Newtonsoft.Json;

namespace eClaim.Data
{
    public class ExchRateRepository : IExchRateService
    {
        public ExchRateModel GetExchangeRates(string baseCurrenry)
        {
            try
            {
                string access_key = "cbc1522e882227533181e48ac7b3b495";
                string url = "http://data.fixer.io/api/latest?access_key=" + access_key + "&base=" + baseCurrenry;
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(url);
                    ExchRateModel result = JsonConvert.DeserializeObject<ExchRateModel>(json);
                    return result;
                }
            }
            catch (Exception)
            {
                // Log exception
                throw;
            }
        }
    }
}