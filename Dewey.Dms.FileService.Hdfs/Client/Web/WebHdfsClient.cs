using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Dewey.Dms.FileService.Hdfs.Client.Web
{
    
    
    
    public class WebHdfsClient:IHdfsClient
    {
        #region Properties
        public string BaseApi { get; }
        public bool? ExpectContinue { get; }
        public TimeSpan? Timeout { get; }
        
        #endregion
        
        #region Variables
        private HttpClient httpClient;
        #endregion
        
        #region Constructor
        public WebHdfsClient(string BaseApi, bool? ExpectContinue = null, TimeSpan? Timeout = null)
        {

            this.Timeout = Timeout;
            this.BaseApi = BaseApi.TrimEnd('/');


            HttpClientHandler httpMessageHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false, 
                PreAuthenticate = true
            };

            httpClient = new HttpClient(httpMessageHandler)
            {
                Timeout = this.Timeout.GetValueOrDefault(TimeSpan.FromMinutes(5))
            };

            this.ExpectContinue = ExpectContinue;
            httpClient.DefaultRequestHeaders.ExpectContinue = this.ExpectContinue;
        }
        #endregion

        #region File and Directory Operation
        public async Task<bool> MakeDirectory(
            string path,
            string permission = null)
        {
            
            string requestPath = $"{BaseApi}{path}?OP=MKDIRS";
            if (!string.IsNullOrWhiteSpace(permission))
                requestPath = $"{requestPath}&permission={permission}";
                
                 

            var response = await httpClient.PutAsync(requestPath, new ByteArrayContent(new byte[] { }));
            response.EnsureSuccessStatusCode();
            var serializer = new DataContractJsonSerializer(typeof(WebHdfsBoolean));
            return ((WebHdfsBoolean)serializer.ReadObject(await response.Content.ReadAsStreamAsync())).boolean;
        }
        
        
        public async Task<bool> Delete(
            string path,
            bool? recursive = null)
        {
            string requestPath = $"{BaseApi}{path}?OP=DELETE";
            if (recursive.HasValue)
                requestPath = $"{requestPath}&recursive={recursive.Value}";

            var response = await httpClient.DeleteAsync(requestPath);
            response.EnsureSuccessStatusCode();
            var serializer = new DataContractJsonSerializer(typeof(WebHdfsBoolean));
            return ((WebHdfsBoolean)serializer.ReadObject(await response.Content.ReadAsStreamAsync())).boolean;
        }
        
        
        public async Task<HdfsFileStatus> GetFileStatus(
            string path)
        {
           

            var response = await httpClient.GetAsync(requestUri: $"{BaseApi}{path}?op=GETFILESTATUS" );
            response.EnsureSuccessStatusCode();
            var serializer = new DataContractJsonSerializer(typeof(WebHdfsFileStatusClass));
            return 
                ((WebHdfsFileStatusClass)serializer.ReadObject(await response.Content.ReadAsStreamAsync()))
                .FileStatus
                .HdfsFileStatus();
        }
        
        public async Task<IEnumerable<HdfsFileStatus>> ListStatus(
            string path)
        {
            var response = await httpClient.GetAsync(requestUri: $"{BaseApi}{path}?op=LISTSTATUS" );
            response.EnsureSuccessStatusCode();
            var serializer = new DataContractJsonSerializer(typeof(WebHdfsFileStatusesClass));
            return ((WebHdfsFileStatusesClass)serializer.ReadObject(await response.Content.ReadAsStreamAsync())).FileStatuses.HdfsFileStatuses();
        }
        
        #endregion
        
        #region Stream operation
        
        public async Task<bool> WriteStream(Stream stream, string path)
        {
            HttpResponseMessage response = await httpClient.PutAsync(
                $"{BaseApi}{path}?op=CREATE",
                (HttpContent) new ByteArrayContent(new byte[0]));
            if (response.StatusCode.Equals((object) HttpStatusCode.RedirectKeepVerb))
            {
                   
                HttpRequestMessage msg = new HttpRequestMessage();
                msg.RequestUri = response.Headers.Location;
                msg.Content = (HttpContent) new StreamContent(stream);
                msg.Method = HttpMethod.Put;
                
                HttpResponseMessage httpResponseMessage =
                await httpClient.SendAsync(msg, HttpCompletionOption.ResponseContentRead); 
                       
                httpResponseMessage.EnsureSuccessStatusCode();
                return httpResponseMessage.IsSuccessStatusCode;
            }

            throw new InvalidOperationException("Should get a 307. Instead we got: " +
                                                (object) response.StatusCode + " " + response.ReasonPhrase);
        }
        
        
   
        
        public async Task<bool> ReadStream(
            Stream stream,
            string path
            )
        {
          
            var response = await httpClient.GetAsync(requestUri: $"{BaseApi}{path}?op=OPEN");
            if (response.StatusCode.Equals(HttpStatusCode.TemporaryRedirect))
            {
                var response2 = await httpClient.GetAsync(response.Headers.Location);
                response2.EnsureSuccessStatusCode();
                if (response2.IsSuccessStatusCode)
                {
                     stream.Position = 0;
                     await response2.Content.CopyToAsync(stream);
                }

                
            
                return response2.IsSuccessStatusCode;
            }
            throw new InvalidOperationException("Should get a 307. Instead we got: " +
                                                response.StatusCode + " " + response.ReasonPhrase);
        }
        
        #endregion
        
        
       
    }

}