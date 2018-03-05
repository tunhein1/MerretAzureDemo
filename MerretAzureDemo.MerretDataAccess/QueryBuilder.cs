using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess
{
    internal static class QueryBuilder
    {
        private const string __SCHEMA_PLACEHOLDER = "##SCHEMA##";
        private const string __COMPANY_NUMBER = "01";
        private const string __COUNTRY_CODE = "GB";
        private const string __STORE_PRICE_ZONE = "01";
        private const string __STOREBRANCHWHSNUMBER = "0002";

        private static string GetMerretDateString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        private static string GetMerretTimeString(DateTime dt)
        {
            return dt.ToString("HHmmss");
        }

        private static string SetSchema(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                string msg = "QueryProvider::SetSchema.  No Sql provided.";
                throw new ArgumentException(msg, nameof(sql));
            }

            string qry = null;

            string schema = ConfigurationManager.AppSettings["merretSchema"];

            qry = sql.Replace(__SCHEMA_PLACEHOLDER, schema);

            return qry;
        }

        /// <summary>
        /// A single query used for both Reprice and active Markdowns (first or further)
        /// param repriceorMarkdown dictates type of rows, 'R' for Reprice and 'M' for Markdown
        /// </summary>
        /// <param name="updateFrom">PDLPRCP.LPUDAT</param>
        /// <returns>query for Line Price File</returns>
        internal static string MerretLinePriceFileQuery(DateTime updateFrom)
        {
            string updateDate = GetMerretDateString(updateFrom);
            string updateTime = GetMerretTimeString(updateFrom);
            string updateNextDate = GetMerretDateString(updateFrom.Date.AddDays(1));

            StringBuilder sb =
                 new StringBuilder("SELECT pdl.LPSTYL, pdl.LPCOLO, pdl.LPSIZE, pdl.LPCTRY, pdl.LPBORT, pdl.LPBBRT, pdl.LPUDAT, pdl.LPUTIM, pdl.LPPCNO,hdp.CIPCTP ,hdp.CIRORM ");

            sb.Append($" FROM {__SCHEMA_PLACEHOLDER}.PDLPRCP pdl ");
            sb.Append($" LEFT OUTER JOIN {__SCHEMA_PLACEHOLDER}.PCCIHDP hdp ON hdp.CIPCNO = pdl.LPPCNO ");
            sb.Append($" WHERE pdl.LPCTRY = \'{__COUNTRY_CODE}\' AND ((pdl.LPUDAT >= \'{updateDate}\' and pdl.LPUTIM >= \'{updateTime}\') OR pdl.LPUDAT >= \'{updateNextDate}\') ");
            //Gemini # 85332 - following where condition is for supporting new valid price without pcn OR any valid pcn (even if the pcn has a £0 price)
            sb.Append(" AND ((pdl.LPPCNO = 0 AND LPBBRT > 0)   OR (pdl.LPPCNO > 0))");
            //sb.Append($" AND UPPER(hdp.CIRORM) = \'{repriceOrMarkdown}\' ");
            sb.Append(" ORDER BY pdl.LPUDAT, pdl.LPUTIM"); //working through oldest change first, just incase a batch has more than one change - so at the end latest change will be applied


            return SetSchema(sb.ToString());
        }

        /// <summary>
        /// A single query used for both Reprice and active Markdowns (first or further)
        /// param repriceorMarkdown dictates type of rows, 'R' for Reprice and 'M' for Markdown
        /// </summary>
        /// <param name="styleId">PDLPRCP.StyleId</param>
        /// <returns>query for Line Price File</returns>
        internal static string MerretLinePriceFileQuery(int styleId)
        {
            StringBuilder sb =
                 new StringBuilder("SELECT pdl.LPSTYL, pdl.LPCOLO, pdl.LPSIZE, pdl.LPCTRY, pdl.LPBORT, pdl.LPBBRT, pdl.LPUDAT, pdl.LPUTIM, pdl.LPPCNO,hdp.CIPCTP ,hdp.CIRORM ");

            sb.Append($" FROM {__SCHEMA_PLACEHOLDER}.PDLPRCP pdl ");
            sb.Append($" LEFT OUTER JOIN {__SCHEMA_PLACEHOLDER}.PCCIHDP hdp ON hdp.CIPCNO = pdl.LPPCNO ");
            sb.Append($" WHERE pdl.LPSTYL = \'{styleId}\' AND pdl.LPCTRY = \'{__COUNTRY_CODE}\'");
            //Gemini # 85332 - following where condition is for supporting new valid price without pcn OR any valid pcn (even if the pcn has a £0 price)
            sb.Append(" AND ((pdl.LPPCNO = 0 AND LPBBRT > 0)   OR (pdl.LPPCNO > 0))");
            //sb.Append($" AND UPPER(hdp.CIRORM) = \'{repriceOrMarkdown}\' ");

            return SetSchema(sb.ToString());
        }

        /// <summary>
        /// Merret Product File query - PDPRODP
        /// </summary>
        /// <param name="styleNumber">PRSTYL</param>
        /// <param name="colourCode">PRCOLO</param>
        /// <param name="sizeCode">PRSIZE</param>
        /// <returns></returns>
        internal static string MerretProductFileQuery(int styleNumber, string colourCode = null, string sizeCode = null)
        {
            StringBuilder sb =
                new StringBuilder("SELECT PRSKU, PRSTYL, PRCOLO, PRSIZE");

            sb.Append($" FROM {__SCHEMA_PLACEHOLDER}.PDPRODP");
            sb.Append($" WHERE PRSTYL = {styleNumber}");

            if (!String.IsNullOrEmpty(colourCode) && colourCode.Trim() != string.Empty)
                sb.Append($" AND UPPER(PRCOLO) = UPPER(\'{colourCode}\')");

            if (!String.IsNullOrEmpty(sizeCode) && sizeCode.Trim() != string.Empty)
                sb.Append($" AND UPPER(PRSIZE) = UPPER(\'{sizeCode}\')");

            return SetSchema(sb.ToString());
        }


        /*FIRST MARKDOWN HEADER*/
        /*
        SELECT CIPCNO, CIPCTP, CIRORM, CICRTP, CIST, CIAPPD, CIAPPT, CISTDT, CISTTM, CIDEPT, CICTRY, CIZCCY, CIPZON, CIDTCT, CIMNTD, CIMNTT, CIUDAT, CIUTIM
        FROM V0608TDHHN.PCCIHDP
        WHERE CICOMP = '01' 
        AND CIRORM = 'M'
        AND CIST ='A' //CONDITION MIGHT BE REMOVED, WHEN UN-ASSIGNED ACTIONS ARE CLEAR
                AND CIUDAT >= '20160912' AND CIUTIM >= '000000'
        //AND CIPCNO = 71136
        ORDER BY CISTDT
        FETCH FIRST 100 ROWS ONLY;
        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repriceOrMarkdown">CIRORM</param>
        /// <param name="updateFrom">CIUDAT</param>
        /// <returns></returns>
        internal static string GetFirstMarkdownHeaderSql(string repriceOrMarkdown, DateTime updateFrom)
        {
            string updateDate = GetMerretDateString(updateFrom);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CIPCNO, CIPCTP, CIRORM, CICRTP, CIST, CIAPPD, CIAPPT, CISTDT, ");
            sql.Append("CISTTM, CIDEPT, CICTRY, CIZCCY, CIPZON, CIDTCT, CIMNTD, CIMNTT, CIUDAT, CIUTIM ");
            sql.Append($"FROM {__SCHEMA_PLACEHOLDER}.PCCIHDP ");
            sql.Append($"WHERE CICOMP = \'{__COMPANY_NUMBER}\' ");
            sql.Append($"AND CIRORM = \'{repriceOrMarkdown}\' ");

            sql.Append($"AND CIUDAT >= '{updateDate}' ");
            sql.AppendFormat("AND CISTDT >= '{0}' ", GetMerretDateString(DateTime.Now.AddDays(1)));
            sql.Append("ORDER BY CIPCNO ");

            return SetSchema(sql.ToString());
        }


        /// <param name="priceChangeNumber">CIPCNO</param>
        /// <returns></returns>
        internal static string GetMarkdownHeaderSql(int priceChangeNumber)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT CIPCNO, CIPCTP, CIRORM, CICRTP, CIST, CIAPPD, CIAPPT, CISTDT, ");
            sql.Append("CISTTM, CIDEPT, CICTRY, CIZCCY, CIPZON, CIDTCT, CIMNTD, CIMNTT, CIUDAT, CIUTIM ");
            sql.Append($"FROM {__SCHEMA_PLACEHOLDER}.PCCIHDP ");
            sql.Append($"WHERE CIPCNO  = {priceChangeNumber} ");

            return SetSchema(sql.ToString());
        }

        /*FIRST MARKDOWN DETAIL*/
        /*
        SELECT CSSKU, CSSTYL, CSSIZE, CSCOLO, CSBORG, CSBBRT, CSTBRT
        FROM V0608TDHHN.PCCSDTP
        WHERE CSCOMP = '01' AND CSCTRY = 'GB' AND CSPZON='01' AND CSSITE = '0002'
        AND CSPCNO = 71136
        //GROUP BY CSSKU, CSSTYL, CSSIZE, CSCOLO, CSBORG, CSBBRT, CSTBRT
        FETCH FIRST 1000 ROWS ONLY;
        */
        /// <summary>
        /// 
        /// </summary>

        /// <param name="priceChangeNumber">CSPNO</param>
        /// <returns></returns>
        internal static string GetFirstMarkdownDetailSql(string priceChangeNumber)
        {
            string sql = "SELECT CSSKU, CSSTYL, CSSIZE, CSCOLO, CSBORG, CSBBRT, CSTBRT " +
                         $"FROM {__SCHEMA_PLACEHOLDER}.PCCSDTP " +
                         $"WHERE CSCOMP = \'{__COMPANY_NUMBER}\' AND CSCTRY = \'{__COUNTRY_CODE}\' AND CSPZON = \'{__STORE_PRICE_ZONE}\' " +
                         $"AND CSSITE = \'{__STOREBRANCHWHSNUMBER}\' " +
                         $"AND CSPCNO = {priceChangeNumber} ";
            //GROUP BY CSSKU, CSSTYL, CSSIZE, CSCOLO, CSBORG, CSBBRT, CSTBRT

            sql = SetSchema(sql);

            return sql;
        }


    }
}
