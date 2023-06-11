using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ProjectProfileMobile
{
    public class ProjectProfileMobileManager : IProjectProfileMobileManager
    {

        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        public ProjectProfileMobileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IList<ProjectProfileList>> GetProjectProfileList(ProjectProfileRequest request)
        {
            List<ProjectProfileList> projectProfileList = new List<ProjectProfileList>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetProfileList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                projectProfileList = JsonConvert.DeserializeObject<List<ProjectProfileList>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                projectProfileList = new List<ProjectProfileList>();
            }
            finally
            {

            }

            return projectProfileList;
        }

        public async Task<ProjectProfileList> InsertProjectList(ProjectProfileList request)
        {
            ProjectProfileList response = new ProjectProfileList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.Insertprofile, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ProjectProfileList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ProjectProfileList();
            }
            finally
            {

            }
            return response;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        public async Task<ProjectProfileList> UpdateProjectList(ProjectProfileList request)
        {
            ProjectProfileList response = new ProjectProfileList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateProfile, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ProjectProfileList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ProjectProfileList();
            }
            finally
            {

            }
            return response;
        }

        public async Task<IList<ProjectResponse>> GetAllProjects(ComboRequestDTO request)
        {
            IList<ProjectResponse> response = new List<ProjectResponse>();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAllProjects, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<IList<ProjectResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new List<ProjectResponse>();
            }
            finally
            {

            }
            return response;
        }

        public async Task<IList<TaskResponse>> GetTasksByProject(ComboRequestDTO request)
        {
            IList<TaskResponse> response = new List<TaskResponse>();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetTasks, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<IList<TaskResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new List<TaskResponse>();
            }
            finally
            {

            }
            return response;
        }

    }
}
