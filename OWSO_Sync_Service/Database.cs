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

        public int readMaxTimestamp()
        {
            int lastMaxTimestamp = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = setting.databaseUrl;
                    conn.Open();

                    String query = setting.queryMaxTimestamp;

                    Logger.getInstance().log(this, "Query max timestamp: " + query);
                    SqlCommand command = new SqlCommand(query, conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            lastMaxTimestamp = reader.GetInt32(0);
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.getInstance().logError(this, e);
            }


            return lastMaxTimestamp;
        }

        public Tuple<STATUS, String> readUpdatedDataInJson(int lastTimestamp)
        {
            var json = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = setting.databaseUrl;
                    conn.Open();

                    String query = String.Format(setting.query, lastTimestamp);

                    Logger.getInstance().log(this, "Query: " + query);
                    SqlCommand command = new SqlCommand(query + RETURN_JSON_QUERY, conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                json.Append(String.Format("{{\"tickets\":{0}}}", reader.GetValue(0).ToString()));
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
                Logger.getInstance().logError(this, e);
                return new Tuple<STATUS, String>(STATUS.FAILED, e.Message);
            }


            return new Tuple<STATUS, String>(STATUS.SUCCESS, Regex.Replace(json.ToString(), "\"[\\s\\t]+|[\\s\\t]+\"", "\""));
        }
    }
}

public enum STATUS
{
    SUCCESS,
    FAILED
}
