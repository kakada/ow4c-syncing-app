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
        }

        public void SubmitData(String content, DateTime newTime)
        {
            RunAsync(content, newTime).GetAwaiter().GetResult();
        }

        async Task RunAsync(String content, DateTime newTime)
        {
            HttpStatusCode statusCode;
            client.BaseAddress = new Uri(_setting.baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _setting.accessToken);

            try
            {
                statusCode = await SendAPI(String.Format(_setting.healthStatusAPI, _setting.siteCode), "");
                Logger.getInstance().log(this, "Health Status: " + statusCode);

                if (statusCode == HttpStatusCode.OK)
                {
                    if(!content.Equals(""))
                    {
                        Logger.getInstance().log(this, "Database sync content: " + content);
                        statusCode = await SendAPI(String.Format(_setting.databaseSyncAPI, _setting.siteCode), content);

                        if (statusCode == HttpStatusCode.OK)
                        {
                            Logger.getInstance().log(this, "Store current time: " + newTime.ToString());
                            lastupdateStorage.storeLastUpdateSync(newTime);
                            Logger.getInstance().log(this, "Database sync success");
                        }
                        else
                        {
                            Logger.getInstance().logError(this, "Database sync error: " + statusCode.ToString());
                        }
                    } else
                    {
                        Logger.getInstance().log(this, "Store current time: " + newTime.ToString());
                        lastupdateStorage.storeLastUpdateSync(newTime);
                    }
                }
            }

            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
            }
        }

        async Task<HttpStatusCode> SendAPI(String api, String content)
        {
            HttpResponseMessage response = await client.PutAsync(api, new StringContent(content, Encoding.UTF8, "application/json"));
            string result = response.Content.ReadAsStringAsync().Result;
            Logger.getInstance().log(this, "request: " + api + "\nresponse: " + result);
            return response.StatusCode;
        }
    }
}
