using System;
using System.IO;
using System.Net;
using System.Text;

namespace OWSO_Sync_Service
{
    class APICommand
    {

        private readonly Setting _setting;
        private readonly LastSyncUpdateStorage lastupdateStorage;

        public APICommand(Setting setting)
        {
            _setting = setting;
            lastupdateStorage = new LastSyncUpdateStorage();
        }

        public void SubmitData(STATUS status, String content, int newTimestamp)
        {
            Logger.getInstance().log(this, "submit data: " + content);
            try
            {
                String healthApi = String.Format(_setting.healthStatusAPI, status == STATUS.SUCCESS ? "success" : "failed", _setting.siteCode);
                Logger.getInstance().log(this, "Sending health status api: " + healthApi);
                var statusCode = SendAPI(healthApi, "");
                Logger.getInstance().log(this, "Health Status: " + statusCode);

                if (status == STATUS.SUCCESS && statusCode == HttpStatusCode.OK)
                {
                    if (!content.Equals(""))
                    {
                        String dbSyncApi = String.Format(_setting.databaseSyncAPI, _setting.siteCode);
                        Logger.getInstance().log(this, "Sending database sync api: " + dbSyncApi);
                        statusCode = SendAPI(dbSyncApi, content);

                        if (statusCode == HttpStatusCode.OK)
                        {
                            Logger.getInstance().log(this, "Store current time: " + newTimestamp);
                            lastupdateStorage.storeLastUpdateSync(newTimestamp);
                            Logger.getInstance().log(this, "Database sync success");
                        }
                        else
                        {
                            Logger.getInstance().logError(this, "Database sync error: " + statusCode.ToString());
                        }
                    }
                    else
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

        private HttpStatusCode SendAPI(String api, String content)
        {
            supportHttpsRequest();
            HttpWebRequest request = buildHttpRequest(_setting.baseUrl + api, content);

            // Get the response.
            WebResponse response = request.GetResponse();

            HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;
            // Display the status.
            Logger.getInstance().log(this, "Http Response Status Code : " + statusCode);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Logger.getInstance().log(this, "Http Response content : " + responseFromServer);
            }

            // Close the response.
            response.Close();

            return statusCode;
        }

        private void supportHttpsRequest()
        {
            ServicePointManager.Expect100Continue = true;
            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // Tls12
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768; // Tls11
        }

        private HttpWebRequest buildHttpRequest(String url, String content)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = WebRequestMethods.Http.Put;
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + _setting.accessToken);
            request.Accept = "application/json";

            // add request content
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] httpContent = encoding.GetBytes(content);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = httpContent.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(httpContent, 0, httpContent.Length);
            newStream.Close();

            return request;
        }
}
}
