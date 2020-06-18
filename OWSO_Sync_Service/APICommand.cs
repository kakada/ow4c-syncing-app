using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly String _baseUrl;
        private readonly String _databaseSyncAPI;
        private readonly String _healthStatusUpdateAPI;

        public APICommand(String baseUrl, String databaseSyncAPI, String healthStatusUpdateAPI)
        {
            _baseUrl = baseUrl;
            _databaseSyncAPI = databaseSyncAPI;
            _healthStatusUpdateAPI = healthStatusUpdateAPI;
        }

        public void SubmitData(String content)
        {
            RunAsync(content).GetAwaiter().GetResult();
        }

        async Task RunAsync(String content)
        {
            HttpStatusCode statusCode;
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                statusCode = await SendAPI(Method.GET, _healthStatusUpdateAPI);

                if (statusCode == HttpStatusCode.OK)
                {
                    await SendAPI(Method.POST, _databaseSyncAPI, content);
                }
            }

            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
            }
        }

        async Task<HttpStatusCode> SendAPI(Method method, String api, String content = null)
        {
            HttpResponseMessage response = await (method == Method.POST ? client.PostAsync(api, new StringContent(content, Encoding.UTF8, "application/json")) : client.GetAsync(api));
            return response.StatusCode;
        }
    }

    enum Method
    {
        GET,
        POST
    }
}
