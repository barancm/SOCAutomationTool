using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Core.Results;
using Core.Helper;
using Core.Models;
using System.Security.Policy;

namespace DomainReputationServices.VirusTotal
{
    public class VirusTotal
    {
        public string VirusTotalURL { get; set; }
        public string APIKey { get; set; }
        public VirusTotal() 
        {
            VirusTotalURL = "https://www.virustotal.com/api/v3/urls";
            APIKey = "b290fd76d513d98230ee2fdb00e18ba8407770600063ee840ae5f0c44b4bab68";
        }
        public async Task<IDataResult<string>> POSTVirusTotalNetAsync(string urlData)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(VirusTotalURL + "/" + urlData),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "x-apikey", APIKey },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new SuccessDataResult<string>("");
        }
    }
}
