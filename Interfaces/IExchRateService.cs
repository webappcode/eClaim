using System;
using eClaim.Models;

namespace eClaim.Interfaces
{
    public interface IExchRateService 
    {
        ExchRateModel GetExchangeRates(string baseCurrenry);
    }
}
