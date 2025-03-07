using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class CurrencyExchangeRate
    {
        public int ID { get; set; }

        public Currency CurrencyName { get; set; }

        public string USDtoBGN { get; set; }

        public string GBPtoBGN { get; set; }

        public string CHFtoBGN { get; set; }

        public string EURtoBGN { get; set; }

        public string ExchangeRateXml { get; set; }

        public DateTime Date { get; set; }
    }
}