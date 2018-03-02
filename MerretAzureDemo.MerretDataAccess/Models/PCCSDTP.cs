using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess.Models
{
    public class PCCSDTP
    {
        /// <summary>
        /// CSSKU = Sku
        /// </summary>
        public int Sku { get; set; }

        /// <summary>
        /// CSSTYL = StyleNumber
        /// </summary>
        public int StyleNumber { get; set; }

        /// <summary>
        /// CSSIZE = SizeCode
        /// </summary>
        public string SizeCode { get; set; }

        /// <summary>
        /// CSCOLO = ColourCode
        /// </summary>
        public string ColourCode { get; set; }

        /// <summary>
        /// CSBORG = BookCcyOriginalRet
        /// </summary>
        public string BookCcyOriginalRet { get; set; }

        /// <summary>
        /// CSBBRT = BookCcyNewRet
        /// </summary>
        public decimal BookCcyNewRet { get; set; }

        /// <summary>
        /// CSTBRT = ZoneCcyNewTck
        /// </summary>
        public decimal ZoneCcyNewTck { get; set; }
    }
}
