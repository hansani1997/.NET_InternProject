using BL10.CleanArchitecture.Shared.Constants;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using bluelotus360.Com.commonLib.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Newtonsoft.Json;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities.ProjectManagement;

namespace bluelotus360.Com.commonLib.Managers.ProcessManager
{
    public class ProcessManager:IProcessManager
    {
        private readonly HttpClient _httpClient;
       private readonly IHttpClientFactory _factory;
        public ProcessManager(HttpClient httpClient, IHttpClientFactory factory)
        {
            _httpClient = httpClient;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
            _factory = factory;
        }

        public async Task<IList<CodeBaseResponseExtended>> GetKanbanBoard(ProcessRequest responses)
        {
            IList<CodeBaseResponseExtended> codes = new List<CodeBaseResponseExtended>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetKanbanBoard, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponseExtended>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<Process>> GetListOfTasks(ProcessRequest responses)
        {
            IList<Process> codes = new List<Process>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetListOfTasks, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<Process>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<CodeBaseResponseExtended>> GetListOfTasksCount(ProcessRequest responses)
        {
            IList<CodeBaseResponseExtended> codes = new List<CodeBaseResponseExtended>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetListOfTasksCount, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponseExtended>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<Process> GetTaskByTaskKey(ProcessRequest responses)
        {
            Process codes = new Process();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetTaskByTaskKey, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<Process>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<ProcessComponent>> GetProcessComponents(ProcessRequest responses)
        {
            IList<ProcessComponent> codes = new List<ProcessComponent>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetProcessComponents, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<ProcessComponent>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<bool> DeleteComponents(ProcessRequest responses)
        {
            bool codes = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.DeleteComponents, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<ProcessRemark>> GetProcessRemarksByProcess(ProcessRequest responses)
        {
            IList<ProcessRemark> codes =new  List<ProcessRemark>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetProcessRemarksByProcess, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<ProcessRemark>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<bool> SaveRemarks(ProcessRemark responses)
        {
            bool codes = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveRemarks, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<bool> UpdateTask(TaskInsertUpdate responses)
        {
            bool codes = false;
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.UpdateTask, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            _httpClient.DefaultRequestHeaders.Authorization.Scheme,
            _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<bool> CreateProcessComponent(ProcessComponent responses)
        {
            bool codes = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateProcessComponent, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<Process>> GetSubTaskByTaskKey(ProcessRequest responses)
        {
            IList<Process> codes = new List<Process>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetSubTaskByTaskKey, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<Process>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<bool> CreateTask(TaskInsertUpdate responses)
        {
            bool codes = false;
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.CreateTask, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<string> GetNextTaskID(ProcessRequest responses)
        {
            string codes = "";
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetNextTaskID, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                codes = content;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<Process>> GetTodo_ListviewSelectWeb(ProcessRequest responses)
        {
            IList<Process> codes = new List<Process>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetTodo_ListviewSelectWeb, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<Process>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<IList<CheckList>> ToDoChkLst(ProcessRequest responses)
        {
            IList<CheckList> codes = new List<CheckList>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ToDoChkLst, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CheckList>>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<bool> CreateUpdateCheckList(CheckList responses)
        {
            bool codes = false;
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.CreateUpdateChkLst, responses);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<bool>(content);
                codes = obj;


            }
            catch (Exception exp)
            {

            }
            return codes;
        }
    }
}
