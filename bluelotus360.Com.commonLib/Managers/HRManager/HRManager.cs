using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Shared.Constants;

namespace bluelotus360.Com.commonLib.Managers.HRManager
{
    public class HRManager : IHRManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;
        //private readonly IConfiguration _config;
        public HRManager(HttpClient httpClient, IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            //_config = config;

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
        public async Task ApplyLeave(Leaverequest req)
        {
            _checkIfExceptionReturn = false;

            try
            {
                await _httpClient.PostAsJsonAsync(TokenEndpoints.Apply_Leave_EndPoint, req);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
        }

        public async Task ChangeLeaveStatus(LeaveStatusChange req)
        {
            _checkIfExceptionReturn = false;

            try
            {
                await _httpClient.PostAsJsonAsync(TokenEndpoints.Change_Leave_Status_EndPoint, req);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
        }

        public async Task<MultiAtnAnlysis_Response> CreateManualAttendence(AddManualAdt attendence)
        {
            _checkIfExceptionReturn = false;

            MultiAtnAnlysis_Response list = new MultiAtnAnlysis_Response();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Put_Manual_Attendence_EndPoint, attendence);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<MultiAtnAnlysis_Response>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return list;
        }

        public async Task DeleteLeave(LeaveDetails req)
        {
            _checkIfExceptionReturn = false;

            try
            {
                await _httpClient.PostAsJsonAsync(TokenEndpoints.Delete_Leave_EndPoint, req);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
        }

        public async Task<IList<PaySlipDetails>> GeneratePaySlip(SalaryHistory req)
        {
            _checkIfExceptionReturn = false;
            IList<PaySlipDetails> _paySlip = new List<PaySlipDetails>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Generate_Payslip_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                _paySlip = JsonConvert.DeserializeObject<List<PaySlipDetails>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return _paySlip;
        }

        public async Task<IList<LeaveDetails>> GetAlreadyAppliedLeaves()
        {
            _checkIfExceptionReturn = false;

            IList<LeaveDetails> lev_list = new List<LeaveDetails>();
            LoggedUsers usr = new LoggedUsers();
            try
            {
                var response = await _httpClient.GetAsync(TokenEndpoints.Get_Already_Applied_Leave_EndPoint);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                lev_list = JsonConvert.DeserializeObject<List<LeaveDetails>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return lev_list;
        }

        public async Task<IList<MultiAtnAnlysis_Response>> GetExistingRecordForDay(MultiAtnAnlysis attendence)
        {
            _checkIfExceptionReturn = false;
            IList<MultiAtnAnlysis_Response> existingList = new List<MultiAtnAnlysis_Response>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Existing_Record_EndPoint, attendence);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                existingList = JsonConvert.DeserializeObject<List<MultiAtnAnlysis_Response>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                existingList = new List<MultiAtnAnlysis_Response>();
            }

            return existingList;
        }

        public async Task<IList<MultiAtnAnlysis_Response>> GetExistingRecordForDayV2(MultiAtnAnlysis attendence)
        {
            _checkIfExceptionReturn = false;
            IList<MultiAtnAnlysis_Response> existingList = new List<MultiAtnAnlysis_Response>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Existing_Record_EndPointV2, attendence);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                existingList = JsonConvert.DeserializeObject<List<MultiAtnAnlysis_Response>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                existingList = new List<MultiAtnAnlysis_Response>();
            }

            return existingList;
        }
        public async Task<int> GetLeaveTrnTypeDetails(LeaveTrnTypeDetails slh)
        {
            _checkIfExceptionReturn = false;

            int key = 0;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_LeaveTrnTypeDetails_EndPoint, slh);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                key = JsonConvert.DeserializeObject<int>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return key;
        }

        public async Task<decimal> GetLeaveTypeByCompany(UserDetails usr)
        {
            _checkIfExceptionReturn = false;
            decimal max_leave_hour = 0;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Leave_Type_By_Company_EndPoint, usr);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                max_leave_hour = JsonConvert.DeserializeObject<decimal>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return max_leave_hour;
        }

        public async Task<int> GetMultiApproval(MultiApprovalDetails multiApproval)
        {
            _checkIfExceptionReturn = false;
            int multiApprovalKey = 0;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_MultiApproval_EndPoint, multiApproval);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                multiApprovalKey = JsonConvert.DeserializeObject<int>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return multiApprovalKey;
        }

        public async Task<UserDetails> GetReportingPerson(UserDetails req)
        {
            _checkIfExceptionReturn = false;

            UserDetails user = new UserDetails();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Retrive_Reporting_Person_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserDetails>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return user;
        }

        public async Task<InShift> GetShift(ManualAttendence req)
        {
            _checkIfExceptionReturn = false;
            InShift att = new InShift();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Shift_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                att = JsonConvert.DeserializeObject<InShift>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                att = new InShift();
            }

            return att;
        }

        public async Task<UserDetails> GetUserAsync()
        {
            _checkIfExceptionReturn = false;
            UserDetails emp = new UserDetails();
            LoggedUsers usr = new LoggedUsers();
            try
            {
                var response = await _httpClient.GetAsync(TokenEndpoints.Get_User_Details_EndPoint);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                emp = JsonConvert.DeserializeObject<UserDetails>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                emp = new UserDetails();
            }

            return emp;
        }

        public async Task<UserDetails> GetAddressDetailsBylogin()
       {
            _checkIfExceptionReturn = false;
            UserDetails emp = new UserDetails();
            LoggedUsers usr = new LoggedUsers();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.GetAsync(TokenEndpoints.Get_Address_DetailsByLogin_EndPoint);
                string content = await response.Content.ReadAsStringAsync();
                emp = JsonConvert.DeserializeObject<UserDetails>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                emp = new UserDetails();
            }

            return emp;
        }

        public async Task<UserPermission> GetUserPermission(UserPermission obj)
        {
            _checkIfExceptionReturn = false;

            UserPermission usrper = new UserPermission();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.User_Permission_EndPoint, obj);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                usrper = JsonConvert.DeserializeObject<UserPermission>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return usrper;
        }

        public async Task<MultiAtnAnlysis_Response> InOut(ManualAttendence attendence)
        {
            _checkIfExceptionReturn = false;
            MultiAtnAnlysis_Response att = new MultiAtnAnlysis_Response();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Put_In_EndPoint, attendence);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                att = JsonConvert.DeserializeObject<MultiAtnAnlysis_Response>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                att = new MultiAtnAnlysis_Response();
            }

            return att;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        public async Task<IList<LeaveDetails>> LeaveFilter(Leaverequest req)
        {
            _checkIfExceptionReturn = false;
            IList<LeaveDetails> _leaveList = new List<LeaveDetails>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Leave_Filter_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                _leaveList = JsonConvert.DeserializeObject<List<LeaveDetails>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return _leaveList;
        }

        public async Task<EmployeeModel> LoadEmployeeDetails()
        {
            _checkIfExceptionReturn = false;
            EmployeeModel _emp = new EmployeeModel();

            try
            {
                var response = await _httpClient.GetAsync(TokenEndpoints.Get_Employee_Details_EndPoint);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                _emp = JsonConvert.DeserializeObject<EmployeeModel>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return _emp;
        }

        public async Task<IList<LeaveSummary>> LoadLeaveSummary(LeaveDetails levDet)
        {
            _checkIfExceptionReturn = false;
            IList<LeaveSummary> leaves = new List<LeaveSummary>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Leave_Summary_EndPoint, levDet);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                leaves = JsonConvert.DeserializeObject<IList<LeaveSummary>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return leaves;
        }

        public async Task<int> SelectLeaveCheck(LeaveCheck req)
        {
            _checkIfExceptionReturn = false;
            int LevTrnKy = 0;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SelectLeaveCheck_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                LevTrnKy = JsonConvert.DeserializeObject<int>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return LevTrnKy;
        }

        public async Task UpdateRecord(UpdateAttendence attendence)
        {
            _checkIfExceptionReturn = false;


            try
            {
                await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_EndPoint, attendence);


            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
        }

        public async Task<IList<TaskwiseAttendance>> TaskwiseAttendance(InRequest request)
        {
           IList<TaskwiseAttendance> tasks= new List<TaskwiseAttendance>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.TaskwiseAttendance, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                tasks = JsonConvert.DeserializeObject<IList<TaskwiseAttendance>>(content);


            }
            catch (Exception exp)
            {
                new List<TaskwiseAttendance>();
            }

            return tasks;
        }

        public async Task<bool> SaveTaskwiseAttendance(IList<TaskwiseAttendance> request)
        {

            bool success = false;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveTaskwiseAttendance, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                success = JsonConvert.DeserializeObject<bool>(content);


            }
            catch (Exception exp)
            {
                new List<TaskwiseAttendance>();
            }

            return success;
        }
    }
}
