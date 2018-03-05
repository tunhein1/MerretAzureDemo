using log4net;
using MerretAzureDemo.MerretDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess.Mapper
{
    public class MerretMapper
    {
        private static string _DATETIME_FMT = "yyyyMMdd HHmmss";
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<PDLPRCP> PDLPRCPFromDataTable(DataTable dt)
        {
            List<PDLPRCP> rows = new List<PDLPRCP>();
            PDLPRCP row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new PDLPRCP();

                unchecked
                {
                    //sku number not on data source any more
                    //row.Sku = (decimal.ToInt32((decimal)dr["PRSKU"]));
                    row.PriceChangeHeader.PriceChangeNumber = int.Parse(dr["LPPCNO"].ToString());
                }

                row.StyleNumber = int.Parse(dr["LPSTYL"].ToString());
                row.ColourCode = dr["LPCOLO"]?.ToString();
                row.SizeCode = dr["LPSIZE"]?.ToString();
                row.CountryCode = dr["LPCTRY"]?.ToString();
                row.BaseCcyOriginalRet = (decimal)dr["LPBORT"];
                row.BaseCcyCurrentRet = (decimal)dr["LPBBRT"];
                row.UpdateDateTime = DateTimeFromMerretStrings(dr["LPUDAT"].ToString(), dr["LPUTIM"].ToString());
                row.PriceChangeHeader.PriceChangeType = dr["CIPCTP"]?.ToString();
                row.PriceChangeHeader.RepriceOrMarkdown = dr["CIRORM"]?.ToString().Trim();

                rows.Add(row);
            }

            return rows;
        }

        public static List<PDPRODP> PDPRODPFromDataTable(DataTable dt)
        {
            List<PDPRODP> rows = new List<PDPRODP>();

            PDPRODP row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new PDPRODP();

                row.Sku = int.Parse(dr["PRSKU"].ToString());

                row.StyleNumber = int.Parse(dr["PRSTYL"].ToString());

                row.ColourCode = dr["PRCOLO"]?.ToString();

                row.SizeCode = dr["PRSIZE"]?.ToString();

                rows.Add(row);
            }

            return rows;
        }

        public static List<PCCSDTP> PCCSDTPFromDataTable(DataTable dt)
        {
            List<PCCSDTP> rows = new List<PCCSDTP>();

            PCCSDTP row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new PCCSDTP();

                row.Sku = Convert.ToInt32(dr["CSSKU"]);
                row.StyleNumber = Convert.ToInt32(dr["CSSTYL"]);
                row.SizeCode = dr["CSSIZE"]?.ToString();
                row.ColourCode = dr["CSCOLO"]?.ToString();
                row.BookCcyOriginalRet = dr["CSBORG"]?.ToString();
                unchecked
                {
                    row.Sku = (decimal.ToInt32((decimal)dr["CSSKU"]));
                }

                row.StyleNumber = Convert.ToInt32(dr["CSSTYL"]);
                row.SizeCode = dr["CSSIZE"]?.ToString();
                row.ColourCode = dr["CSCOLO"]?.ToString();
                row.BookCcyOriginalRet = dr["CSBORG"]?.ToString();
                row.BookCcyNewRet = (decimal)dr["CSBBRT"];
                row.ZoneCcyNewTck = (decimal)dr["CSTBRT"];

                rows.Add(row);
            }

            return rows;
        }

        public static List<PCCIHDP> PCCIHDPFromDataTable(DataTable dt)
        {
            List<PCCIHDP> rows = new List<PCCIHDP>();
            PCCIHDP row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = PCCIHDPFromRow(dr);
                rows.Add(row);
            }

            return rows;
        }

        private static PCCIHDP PCCIHDPFromRow(DataRow dr)
        {
            PCCIHDP header = new PCCIHDP();
            header.PriceChangeNumber = Convert.ToInt32(dr["CIPCNO"]);
            header.PriceChangeType = dr["CIPCTP"].ToString();
            header.RepriceOrMarkdown = dr["CIRORM"].ToString();
            header.CreationType = dr["CICRTP"]?.ToString();
            header.Status = dr["CIST"]?.ToString();
            header.ApprovedDateTime = DateTimeFromMerretStrings(dr["CIAPPD"].ToString(), dr["CIAPPT"].ToString());
            header.BaseStartDateTime = DateTimeFromMerretStrings(dr["CISTDT"].ToString(), dr["CISTTM"].ToString());
            header.DeparmentNumber = dr["CIDEPT"]?.ToString();
            header.CountryCode = dr["CICTRY"]?.ToString();
            header.ZoneCurrencyCode = dr["CIZCCY"]?.ToString();
            header.CreationType = dr["CICRTP"]?.ToString();
            header.Status = dr["CIST"]?.ToString();
            header.DeparmentNumber = dr["CIDEPT"]?.ToString();
            header.CountryCode = dr["CICTRY"]?.ToString();
            header.ZoneCurrencyCode = dr["CIZCCY"]?.ToString();
            header.StorePriceZone = (decimal)dr["CIPZON"];
            header.DateCreated = DateTimeFromMerretStrings(dr["CIDTCT"].ToString(), "000000");
            header.MaintainedDateTime = DateTimeFromMerretStrings(dr["CIMNTD"].ToString(), dr["CIMNTT"].ToString());
            header.UpdateDateTime = DateTimeFromMerretStrings(dr["CIUDAT"].ToString(), dr["CIUTIM"].ToString());

            return header;
        }

        private static DateTime DateTimeFromMerretStrings(string date, string time)
        {
            DateTime dt = default(DateTime);

            if (6 != time.Length)
                time = "000000";

            if (8 != date.Length)
                dt = DateTime.MinValue;
            else
            {

                try
                {
                    string sdt = date + ' ' + time;

                    dt = DateTime.ParseExact(sdt, "d",
                                             new DateTimeFormatInfo()
                                             {
                                                 ShortDatePattern = _DATETIME_FMT
                                             });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return dt;
        }



    }
}
