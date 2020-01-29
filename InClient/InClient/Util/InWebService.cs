using InClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace InClient.Util
{
    public class InWebService
    {
        private static HttpClient httpClient = null;
        private static Task<HttpResponseMessage> response = null;
        private static HttpResponseMessage result = null;
        //public static HubConnection hubConnection = null;
        public static Token token = null;

        #region Constructor
        /// <summary>
        /// Don't let anyone instantiate this class
        /// </summary>
        private InWebService()
        {
        }
        #endregion

        public static HttpClient UFHttpClient
        {
            get
            {
                if (httpClient != null && httpClient.DefaultRequestHeaders.Contains("Bearer") && !string.IsNullOrEmpty(httpClient.DefaultRequestHeaders.GetValues("Bearer").First()))
                    return httpClient;
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.AutomaticDecompression = DecompressionMethods.Deflate;
                httpClient = new HttpClient(clientHandler, false);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
                httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebServiceUrl"]);
                return httpClient;
            }
        }

        public static T HttpRequest<T>(string urn, object args = null, UFHttpAction? httpMethod = null)
        {
            if (httpMethod == null)
            {
                if (args != null)
                    httpMethod = UFHttpAction.Post;
                else
                    httpMethod = UFHttpAction.Get;
            }

            ValidateUrn(ref urn);
            response = null;
            result = new HttpResponseMessage();
            try
            {
                if (httpMethod == UFHttpAction.Get)
                {
                    response = UFHttpClient.GetAsync(urn);
                }
                else if (httpMethod == UFHttpAction.Post)
                {
                    if (args == null)
                        response = UFHttpClient.PostAsJsonAsync(urn, args);
                    else if (args != null)
                        response = UFHttpClient.PostAsJsonAsync(urn, args);
                }
                else if (httpMethod == UFHttpAction.Put)
                {
                    if (args == null)
                        response = UFHttpClient.PutAsJsonAsync(urn, args);
                    else if (args != null)
                        response = UFHttpClient.PutAsJsonAsync(urn, args);
                }
                else if (httpMethod == UFHttpAction.Delete)
                {
                    response = UFHttpClient.DeleteAsync(urn);
                }
                result = response.Result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.GetType() == typeof(TaskCanceledException))
                    throw new Exception("Web Service is not Responding." + Environment.NewLine + urn, ex.InnerException);
                throw new Exception("Web Service Server not found." + Environment.NewLine + urn, ex.InnerException);
            }
            return HttpRequestResult<T>(urn, result);
        }

        private static T HttpRequestResult<T>(string urn, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (T)response.Content.ReadAsAsync(typeof(T)).Result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Web Service Request or Response format is incorrect." + Environment.NewLine + urn, ex);
                }
            }
            else
            {
                InExceptionDetails exDetails = new InExceptionDetails();
                try
                {
                    exDetails = response.Content.ReadAsAsync<InExceptionDetails>().Result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Web Service requested resource was not found." + Environment.NewLine + urn, ex);
                }

                if (exDetails.Ex != null)
                    throw new Exception(exDetails.Ex.Message + Environment.NewLine + urn, exDetails.Ex);
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public static T HttpRequest<T>(string urn, string reportToken)
        {
            try
            {
                UFHttpClient.DefaultRequestHeaders.Remove("ReportToken");
                UFHttpClient.DefaultRequestHeaders.Add("ReportToken", reportToken);
                response = UFHttpClient.GetAsync(urn);
                result = response.Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + urn, ex);
            }
            return HttpRequestResult<T>(urn, result);
        }

        private static void ValidateUrn(ref string urn)
        {
            if (urn.Contains("#"))
            {
                urn = urn.Replace("#", "%23");
            }
        }

        public async Task<T> SendRequestAsync<T>(string urn)
        {
            HttpContent content = new StringContent("SampleJson", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await UFHttpClient.PostAsync(urn, content);
            T resultObj = (T)response.Content.ReadAsAsync(typeof(T)).Result;
            return resultObj;
        }

        public static ArrayList ConvertJobjectToObject<T>(ArrayList oldList)
        {
            ArrayList newList = new ArrayList();
            for (int i = 0; i < oldList.Count; i++)
            {
                JObject jobj = oldList[i] as JObject;
                Object objT = jobj.ToObject(typeof(T));
                newList.Add(objT);
            }
            return newList;
        }
    }

    //public static HttpResponseMessage GetServiceData(string serviceUrl)
    //{
    //    HttpResponseMessage result = null;
    //    string apiUrl = ConfigurationManager.AppSettings["WebServiceUrl"] + serviceUrl;
    //    Task<HttpResponseMessage> response = null;
    //    using (HttpClient client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(apiUrl);
    //        client.DefaultRequestHeaders.Accept.Clear();
    //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

    //        response = client.GetAsync(apiUrl);
    //        result = response.Result;
    //    }
    //    return result;
    //}

    [Serializable]
    public enum UFHttpAction
    {
        Unknown = -1,
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3,
    }

    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class InExceptionDetails
    {
        public InExceptionDetails() { }
        public InExceptionDetails(Type exType, Exception ex)
        {
            this.ExType = exType;
            this.Ex = ex;
        }

        public Type ExType { get; set; }
        public Exception Ex { get; set; }
    }
}