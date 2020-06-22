using Sentry;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace OWSO_Sync_Service
{
    class Database
    {
        private const String RETURN_JSON_QUERY = " FOR JSON PATH, Include_Null_Values;";
        private readonly Setting setting;

        public Database(Setting setting)
        {
            this.setting = setting;
        }

        public String readUpdatedDataInJson(long lastUpdatedTime)
        {
            var json = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = setting.databaseUrl;
                    conn.Open();

                    Logger.getInstance().log(this, "Last updated date and time: " + lastUpdatedTime);
                    String query = setting.query + RETURN_JSON_QUERY;
                    if (lastUpdatedTime > 0)
                    {
                        DateTime dateTime = new DateTime(lastUpdatedTime);
                        query = String.Format(query, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        query = String.Format(query, 0);
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
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
            }


            return Regex.Replace(json.ToString(), "\"[\\s\\t]+|[\\s\\t]+\"", "\"");
        }
    }
}
