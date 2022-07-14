﻿/*
 The MIT License (MIT)

Copyright (c) 2015 Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cqc.Helpers.GraphClient
{
    /// <summary>
    /// Helper class to call a protected API and process its result
    /// </summary>
    public class ProtectedApiCallHelper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient used to call the protected API</param>
        public ProtectedApiCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; private set; }


        /// <summary>
        /// Calls the protected Web API and processes the result
        /// </summary>
        /// <param name="webApiUrl">Url of the Web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the Web API</param>
        /// <param name="postData">Callback used to process the result of the call to the Web API</param>
        private async Task<JObject> CallWebApiAndProcessResultASync(string webApiUrl, string accessToken, string postData = null)
        {
            JObject responseJson = new JObject();
            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null || defaultRequestHeaders.Accept.All(m => m.MediaType != "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                HttpResponseMessage response;
                if (postData != null)
                {
                    response = await HttpClient.PostAsync(webApiUrl, new StringContent(postData, Encoding.UTF8, "application/json"));
                }
                else
                {
                    response = await HttpClient.GetAsync(webApiUrl);
                }

                string json = await response.Content.ReadAsStringAsync();
                responseJson = JsonConvert.DeserializeObject(json) as JObject;
                if (!response.IsSuccessStatusCode)
                {
                    Exception ex = new Exception($"Failed call to the Graph Api: {response.StatusCode}");
                    ex.Data.Add("ResponseJson", responseJson);
                    throw ex;
                }
            }

            return responseJson;
        }

        /// <summary>
        /// Calls the protected Web API using GET request and processes the result
        /// </summary>
        /// <param name="webApiUrl">Url of the Web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the Web API</param>
        public async Task<JObject> GetRequestWebApiAndProcessResultASync(string webApiUrl, string accessToken)
        {
            return await CallWebApiAndProcessResultASync(webApiUrl, accessToken);
        }

        /// <summary>
        /// Calls the protected Web API using POST request and processes the result
        /// </summary>
        /// <param name="webApiUrl">Url of the Web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the Web API</param>
        /// <param name="postData">jsonData to post to api</param>
        public async Task<JObject> PostToWebApiAndProcessResultASync(string webApiUrl, string accessToken, string postData)
        {
            return await CallWebApiAndProcessResultASync(webApiUrl, accessToken, postData);
        }
    }
}
