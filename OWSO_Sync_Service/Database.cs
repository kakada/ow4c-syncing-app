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

        public String readUpdatedDataInJson(DateTime lastUpdatedTime)
        {
            var json = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = setting.databaseUrl;
                    conn.Open();

                    String query = String.Format(setting.query, "'" + lastUpdatedTime.ToString(setting.compareDateTimeFormat) + "'");

                    Logger.getInstance().log(this, "Query: " + query);
                    SqlCommand command = new SqlCommand(query + RETURN_JSON_QUERY, conn);
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
                            json.Append("");
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
