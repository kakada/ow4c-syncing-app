using Sentry;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OWSO_Sync_Service
{
    class APICommand
    {
        private HttpClient client = new HttpClient();

        private readonly Setting _setting;
        private readonly LastSyncUpdateStorage lastupdateStorage;

        public APICommand(Setting setting)
        {
            _setting = setting;
            lastupdateStorage = new LastSyncUpdateStorage();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            initialHttpClient();
        }

        private void initialHttpClient()
        {
            client.BaseAddress = new Uri(_setting.baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _setting.accessToken);
            client.Timeout = TimeSpan.FromSeconds(_setting.connectionTimeout);
        }

        public void SubmitData(STATUS status, String content, int newTimestamp)
        {
            Logger.getInstance().log(this, "submit data: " + content);
            RunAsync(status, content, newTimestamp).GetAwaiter().GetResult();
        }

        async Task RunAsync(STATUS status, String content, int newTimestamp)
        {          
            try
            {
                String healthApi = String.Format(_setting.healthStatusAPI, status == STATUS.SUCCESS ? "success" : "failed", _setting.siteCode);
                HttpStatusCode statusCode = await SendAPI(healthApi, "");

                if (status == STATUS.SUCCESS && statusCode == HttpStatusCode.OK)
                {
                    if(!content.Equals(""))
                    {
                        String dbSyncApi = String.Format(_setting.databaseSyncAPI, _setting.siteCode);
                        statusCode = await SendAPI(dbSyncApi, content);

                        if (statusCode == HttpStatusCode.OK)
                        {
                            Logger.getInstance().log(this, "Store current time: " + newTimestamp);
                            lastupdateStorage.storeLastUpdateSync(newTimestamp);
                        }
                        else
                        {
                            Logger.getInstance().logError(this, "Database sync error: " + statusCode.ToString());
                        }
                    } else
                    {
                        Logger.getInstance().log(this, "Store current time: " + newTimestamp);
                        lastupdateStorage.storeLastUpdateSync(newTimestamp);
                    }
                }
            }

            catch (Exception e)
            {
                Logger.getInstance().logError(this, e);
            }
        }

        async Task<HttpStatusCode> SendAPI(String api, String content)
        {
            HttpResponseMessage response = await client.PutAsync(_setting.baseUrl + api, new StringContent(content, Encoding.UTF8, "application/json"));
            string result = response.Content.ReadAsStringAsync().Result;
            Logger.getInstance().log(this, "\nHttp Request: " + api + "\nHttpResponse: " + result);
            return response.StatusCode;
        }
    }
}
