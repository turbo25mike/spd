﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spd.Console.Models;

namespace Spd.Console.Extensions
{
    public interface IWebService {
        Task<T> Request<T>(RequestType type, string uri, object content = null, string token = "");
    }

    public class WebService : IWebService
    {
        public async Task<T> Request<T>(RequestType type, string uri, object content = null, string token = "")
        {
            StringContent jsonContent = null;
            var request = new HttpClient();
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
                request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (content != null)
                jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            switch (type)
            {
                case RequestType.Get:
                    response = await request.GetAsync(uri);
                    break;
                case RequestType.Post:
                    response = await request.PostAsync(uri, jsonContent);
                    break;
                case RequestType.Put:
                    response = await request.PutAsync(uri, jsonContent);
                    break;
                case RequestType.Delete:
                    response = await request.DeleteAsync(uri);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            if (response == null)
                throw new ArgumentException("Service Error: response was null");
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"Service Error: {response.ReasonPhrase}");

            var result = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(result))
                return default(T);
            return typeof(T) == typeof(string) ? (T)(object)result : JsonConvert.DeserializeObject<T>(result);
        }
    }
}
