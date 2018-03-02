using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess.Models
{
    public class PCCIHDP
    {
        /// <summary>
        /// CIPCNO = PriceChangeNumber
        /// </summary>
        public int PriceChangeNumber { get; set; }

        /// <summary>
        /// CIPCTP = PriceChangeType
        /// </summary>
        public string PriceChangeType { get; set; }

        /// <summary>
        /// CIRORM = RepriceOrMarkdown
        /// </summary>
        public string RepriceOrMarkdown { get; set; }

        /// <summary>
        /// CICRTP = CreationType
        /// </summary>
        public string CreationType { get; set; }

        /// <summary>
        /// CIST = Status
        /// </summary>
        public string Status { get; set; }

        public DateTime ApprovedDateTime { get; set; }

        public DateTime BaseStartDateTime { get; set; }

        /// <summary>
        /// CIDEPT = DeparmentNumber
        /// </summary>
        public string DeparmentNumber { get; set; }

        /// <summary>
        /// CICTRY = CountryCode
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// CIZCCY = ZoneCurrencyCode
        /// </summary>
        public string ZoneCurrencyCode { get; set; }

        /// <summary>
        /// CIPZON = StorePriceZone
        /// </summary>
        public decimal StorePriceZone { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime MaintainedDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
