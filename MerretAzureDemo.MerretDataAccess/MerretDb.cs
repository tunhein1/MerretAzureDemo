using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MerretAzureDemo.MerretDataAccess
{
    internal static class MerretDb
    {
        private static string _cs =
            ConfigurationManager.ConnectionStrings["merretDb"].ConnectionString;

        internal static DataTable ExecuteQuery(string tableName, string sql)
        {
            DataTable dt = new DataTable(tableName);

            using (OdbcConnection con = new OdbcConnection(_cs))
            {
                con.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, con))
                {
                    using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
