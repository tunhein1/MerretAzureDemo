using MerretAzureDemo.MerretDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess
{
    public interface IMerretRepository
    {
        /// <summary>
        /// Gets Data from Line Price File (Active)
        /// </summary>
        /// <param name="updateFrom"></param>
        /// <returns></returns>
        List<PDLPRCP> GetLinePriceFile(DateTime updateFrom);

        List<PDLPRCP> GetLinePriceFile(int styleNumber);

        List<PDPRODP> GetProducts(int styleNumber, string colourCode = null, string sizeCode = null);

        List<PCCIHDP> GetFirstMarkdownHeader(string repriceOrMarkdown, DateTime updateFrom);

        PCCIHDP GetMarkdownHeader(int priceChangeNumber);

        List<PCCSDTP> GetFirstMarkdownDetail(string priceChangeNumber);
    }
}
