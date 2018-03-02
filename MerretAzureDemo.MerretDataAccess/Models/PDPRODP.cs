using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess.Models
{
    public class PDPRODP
    {
        /// <summary>
        /// PRSKU = Sku
        /// </summary>
        public int Sku { get; set; }

        /// <summary>
        /// PRSTYL = StyleNumber
        /// </summary>
        public int StyleNumber { get; set; }

        /// <summary>
        /// PRCOLO = ColourCode
        /// </summary>
        public string ColourCode { get; set; }

        /// <summary>
        /// PRSIZE = SizeCode
        /// </summary>
        public string SizeCode { get; set; }

    }
}
