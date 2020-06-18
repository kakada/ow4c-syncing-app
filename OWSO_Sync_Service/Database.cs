using Sentry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OWSO_Sync_Service
{
    class Database
    {
        private readonly String _databaseUrl;
        private readonly String _query;

        public Database(String databaseUrl, String query)
        {
            _databaseUrl = databaseUrl;
            _query = query + " FOR JSON PATH, Include_Null_Values;";
        }

        public String readUpdatedDataInJson(long lastUpdatedTime)
        {
            var json = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = _databaseUrl;
                    conn.Open();

                    Logger.getInstance().log(this, "Last updated date and time: " + lastUpdatedTime);
                    String query;
                    if(lastUpdatedTime > 0)
                    {
                        DateTime dateTime = new DateTime(lastUpdatedTime);
                        query = String.Format(_query, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    } else
                    {
                        query = String.Format(_query, 0);
                    }

                    Logger.getInstance().log(this, "Query: " + query);
                    SqlCommand command = new SqlCommand(query, conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                json.Append(reader.GetValue(0).ToString());
                            }
                        }
                        else
                        {
                            json.Append("[]");
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                }
            } catch(Exception e)
            {
                SentrySdk.CaptureException(e);
            }
            

            return Regex.Replace(json.ToString(), "\"[\\s\\t]+|[\\s\\t]+\"", "\"");
        }
    }
}
