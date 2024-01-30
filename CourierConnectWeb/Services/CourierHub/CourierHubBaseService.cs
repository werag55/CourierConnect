﻿using Azure.Core;
using CourierConnect.Models;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

using System.IO;

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubBaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public CourierHubBaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest, string? apiKey = null)
        {
            try
            {
                var client = httpClient.CreateClient("CourierConnect");

                HttpRequestMessage message = new HttpRequestMessage();
                //message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

                if (!string.IsNullOrEmpty(apiKey))
                    message.Headers.Add("x-api-key", apiKey);

                if (!string.IsNullOrEmpty(apiRequest.Token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                APIResponse ApiResponse = new APIResponse
                {
                    StatusCode = apiResponse.StatusCode,
                    IsSuccess = (apiResponse.StatusCode == System.Net.HttpStatusCode.Created || apiResponse.StatusCode == System.Net.HttpStatusCode.OK),
                    Result = JsonConvert.DeserializeObject<object>(apiContent)
                };
                var res = JsonConvert.SerializeObject(ApiResponse);
                var returnObj = JsonConvert.DeserializeObject<T>(res);
                return returnObj;

            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}

