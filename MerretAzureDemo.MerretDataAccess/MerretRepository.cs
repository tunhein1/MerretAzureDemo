using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerretAzureDemo.MerretDataAccess.Models;
using System.Data;
using log4net;
using MerretAzureDemo.MerretDataAccess.Mapper;

namespace MerretAzureDemo.MerretDataAccess
{
    public class MerretRepository : IMerretRepository
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MerretRepository()
        {
        }

        //Price Line File joined with Price Change Header File
        //Has records for RePrice and Markdowns (first and further)
        //we get relevant records based on RePriceorMarkdownCode
        public List<PDLPRCP> GetLinePriceFile(DateTime updateFrom)
        {
            List<PDLPRCP> pdlprcp = null;
            //TODO: REFACTOR THIS GUY AND THE OTHER GetLinePriceFile BELOW
            try
            {
                string sql = QueryBuilder.MerretLinePriceFileQuery(updateFrom);
                DataTable dt = MerretDb.ExecuteQuery(nameof(PDLPRCP), sql);
                pdlprcp = MerretMapper.PDLPRCPFromDataTable(dt);
            }
            catch (Exception e)
            {
                var mde = new MerretDataException("MerretRepository::GetLinePriceFile exception", e);
                log.Error(mde);
            }

            return pdlprcp;
        }

        //Price Line File joined with Price Change Header File
        //Has records for RePrice and Markdowns (first and further)
        //we get relevant records based on RePriceorMarkdownCode
        public List<PDLPRCP> GetLinePriceFile(int styleNumber)
        {
            List<PDLPRCP> pdlprcp = null;

            try
            {
                string sql = QueryBuilder.MerretLinePriceFileQuery(styleNumber);
                DataTable dt = MerretDb.ExecuteQuery(nameof(PDLPRCP), sql);
                pdlprcp = MerretMapper.PDLPRCPFromDataTable(dt);
            }
            catch (Exception e)
            {
                var mde = new MerretDataException("MerretRepository::GetLinePriceFile exception", e);
                log.Error(mde);
            }

            return pdlprcp;
        }

        public List<PDPRODP> GetProducts(int styleNumber, string colourCode = null, string sizeCode = null)
        {
            List<PDPRODP> products = null;

            try
            {
                string sql = QueryBuilder.MerretProductFileQuery(styleNumber, colourCode, sizeCode);

                DataTable dt = MerretDb.ExecuteQuery(nameof(PDPRODP), sql);

                products = MerretMapper.PDPRODPFromDataTable(dt);

            }
            catch (Exception e)
            {
                var mde = new MerretDataException("MerretRepository::GetProducts exception", e);
                log.Error(mde);
            }

            return products;
        }

        public List<PCCIHDP> GetFirstMarkdownHeader(string repriceOrMarkdown, DateTime updateFrom)
        {
            List<PCCIHDP> pccihdp = null;

            string sql = QueryBuilder.GetFirstMarkdownHeaderSql(repriceOrMarkdown, updateFrom);

            DataTable dt = MerretDb.ExecuteQuery(nameof(PCCIHDP), sql);

            pccihdp = MerretMapper.PCCIHDPFromDataTable(dt);

            return pccihdp;
        }

        public PCCIHDP GetMarkdownHeader(int priceChangeNumber)
        {
            string sql = QueryBuilder.GetMarkdownHeaderSql(priceChangeNumber);

            DataTable dt = MerretDb.ExecuteQuery(nameof(PCCIHDP), sql);

            var pccihdp = MerretMapper.PCCIHDPFromDataTable(dt);

            return pccihdp.FirstOrDefault();
        }

        public List<PCCSDTP> GetFirstMarkdownDetail(string priceChangeNumber)
        {
            List<PCCSDTP> pccsdtp = null;

            string sql = QueryBuilder.GetFirstMarkdownDetailSql(priceChangeNumber);

            DataTable dt = MerretDb.ExecuteQuery(nameof(PCCSDTP), sql);

            pccsdtp = MerretMapper.PCCSDTPFromDataTable(dt);

            return pccsdtp;
        }

        
    }
}
