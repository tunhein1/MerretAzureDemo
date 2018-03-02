using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess.Models
{
    public class PDLPRCP
    {
        public PDLPRCP()
        {
            PriceChangeHeader = new PCCIHDP();
        }

        // PDLPRCP

        /// <summary>
        /// LPSTYL = StyleNumber
        /// </summary>
        public int StyleNumber { get; set; }

        /// <summary>
        /// LPCOLO = ColourCode
        /// </summary>
        public string ColourCode { get; set; }

        /// <summary>
        /// LPSIZE = SizeCode
        /// </summary>
        public string SizeCode { get; set; }

        /// <summary>
        /// LPCTRY = CountryCode
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// LPBORT = BaseCcyOriginalRet
        /// </summary>
        public decimal BaseCcyOriginalRet { get; set; }

        /// <summary>
        /// LPBBRT = BaseCcyCurrentRet
        /// </summary>
        public decimal BaseCcyCurrentRet { get; set; }

        public PCCIHDP PriceChangeHeader { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
