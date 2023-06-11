using BL10.CleanArchitecture.Domain.Entities.Document;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.UploadManager
{
    public class UploadManager : IUploadManager
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _factory;
        public UploadManager(HttpClient httpClient,IConfiguration config, IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
            _factory = factory;
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<IList<Base64Document>> getBase64Documents(DocumentRetrivaltDTO document)
        {
            IList<Base64Document> docs = new List<Base64Document>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetBase64DocumentsEndPoint, document);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                docs = JsonConvert.DeserializeObject<IList<Base64Document>>(content);



            }
            catch (Exception exp)
            {

            }
            return docs;
        }
        public async Task<IList<Base64Document>> getBase64DocumentsV2(DocumentRetrivaltDTO document)
        {
            IList<Base64Document> docs = new List<Base64Document>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetBase64DocumentsFullExEndPoint, document);
                string content = await response.Content.ReadAsStringAsync();
                docs = JsonConvert.DeserializeObject<IList<Base64Document>>(content);

            }
            catch (Exception exp)
            {

            }
            return docs;
        }
        public async Task UploadFile(FileUpload uploadReq)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FileUploadEndPoint, uploadReq);
            }
            catch (Exception exp)
            {

            }
        }

        public async Task<IList<Base64Document>> GetAllDocuments(DocumentRetrivaltDTO document)
        {
            IList<Base64Document> docs = new List<Base64Document>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAllDocuments, document);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                docs = JsonConvert.DeserializeObject<IList<Base64Document>>(content);



            }
            catch (Exception exp)
            {

            }
            return docs;
        }

        public async Task<bool> DeleteDocument(DocumentRetrivaltDTO document)
        {
           bool docs = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.DeleteDocument, document);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                docs = JsonConvert.DeserializeObject<bool>(content);



            }
            catch (Exception exp)
            {

            }
            return docs;
        }
    }
}
