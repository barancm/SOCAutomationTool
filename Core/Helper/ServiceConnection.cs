using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public class ServiceConnection
    {
        public string Request(string url, string method, string contentType, string username, string password, object data, List<KeyValuePair<string, string>> headers)
        {
			try
			{
                string output = "";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                string cookie = string.Empty;
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.KeepAlive = true;
                request.Method = method;
                request.ContentType = contentType;
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    request.Credentials = new System.Net.NetworkCredential(username, password);
                }
                byte[] byteArray = null;
                if (headers != null)
                {
                    headers.Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value))
                            .ToList()
                            .ForEach(item =>
                            {
                                request.Headers.Add(item.Key, item.Value);
                            });
                }
                if (data != null)
                {
                    string requestJson = JsonConvert.SerializeObject(data);
                    byteArray = Encoding.UTF8.GetBytes(requestJson);
                    request.ContentLength = byteArray.Length;
                }
                try
                {
                    if (byteArray != null)
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }
                    }
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.Headers["Set-Cookie"] != null)
                            cookie = response.Headers["Set-Cookie"].ToString();
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            output = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException ex)
                {
                    string response = "";
                    using (WebResponse wr = ex.Response)
                    {
                        using (Stream hataData = wr.GetResponseStream())
                        {
                            using (var reader = new StreamReader(hataData))
                            {
                                response = reader.ReadToEnd();
                            }
                        }
                    }
                    throw new Exception(response);
                }
                return output;
            }
			catch (Exception ex)
			{
                return null;
			}
        }
    }
}
