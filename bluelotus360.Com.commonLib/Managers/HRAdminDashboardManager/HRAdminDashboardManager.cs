using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.HRAdminDashboardManager
{
    public class HRAdminDashboardManager : IHRAdminDashboardManager 
    {
        private readonly RestClient _restClient;
        private bool _checkIfExceptionReturn;

        public HRAdminDashboardManager(RestClient restClient)
        {
            _restClient = restClient;
        }


        //cards details
        public async Task<HRAdminDashboardCardResponse> GetHRAdminDashboardCardDetails(HRAdminDashboardCardRequest request)
        {
            HRAdminDashboardCardResponse details = new HRAdminDashboardCardResponse();
            ApiServerResponse<HRAdminDashboardCardResponse> apiResponse = new ApiServerResponse<HRAdminDashboardCardResponse>();
            try
            {
                var request_client = new RestRequest(TokenEndpoints.GetHRAdminDashboardCardDetailsEndPoint);
                request_client.AddJsonBody(request);
                var response = await _restClient.PostAsync(request_client);
                string content = response.Content.ToString();
                apiResponse = JsonConvert.DeserializeObject<ApiServerResponse<HRAdminDashboardCardResponse>>(content);

                if (apiResponse != null && apiResponse.ExecutionException is null)
                {
                    details = apiResponse.Value;
                }
                else
                {
                    details = new HRAdminDashboardCardResponse();
                    throw new Exception("An Exception has been returned ,pls check");

                }
            }
            catch (Exception exp)
            {
               
            }
            finally
            {

            }

            return details;
        }

        //Head Count Chart
        public async Task<HrAdminDashboardChartResponse> GetLocationWiseHeadCount(HRAdminDashboardRequest request)
        {
            HrAdminDashboardChartResponse details = new HrAdminDashboardChartResponse();
            ApiServerResponse<HrAdminDashboardChartResponse> apiResponse = new ApiServerResponse<HrAdminDashboardChartResponse>();
            try
            {
                var request_client = new RestRequest(TokenEndpoints.GetLocationWiseHeadCountEndPoint);
                request_client.AddJsonBody(request);
                var response = await _restClient.PostAsync(request_client);
                string content = response.Content.ToString();
                apiResponse = JsonConvert.DeserializeObject<ApiServerResponse<HrAdminDashboardChartResponse>>(content);

                if (apiResponse!=null && apiResponse.ExecutionException is null)
                {
                    details = apiResponse.Value;
                }
                else
                {
                    details = new HrAdminDashboardChartResponse();
                    throw new Exception("An Exception has been returned ,pls check");
                    
                }
            }
            catch (Exception exp)
            {
               // _checkIfExceptionReturn = true;
            }
            finally
            {

            }

            return details;
        }

        //Attendance summary chart
        public async Task<HRAdminDashboardChart2Response> GetAttendanceSummary(HRAdminDashboardRequest request)
        {
            HRAdminDashboardChart2Response details = new HRAdminDashboardChart2Response();
            ApiServerResponse<HRAdminDashboardChart2Response> apiResponse = new ApiServerResponse<HRAdminDashboardChart2Response>();
            try
            {
                var request_client = new RestRequest(TokenEndpoints.GetAttendanceSummaryEndPoint);
                request_client.AddJsonBody(request);
                var response = await _restClient.PostAsync(request_client);
                string content = response.Content.ToString();
                apiResponse = JsonConvert.DeserializeObject<ApiServerResponse<HRAdminDashboardChart2Response>>(content);

                if (apiResponse != null && apiResponse.ExecutionException is null)
                {
                    details = apiResponse.Value;
                }
                else
                {
                    details = new HRAdminDashboardChart2Response();
                    throw new Exception("An Exception has been returned ,pls check");

                }
            }
            catch (Exception exp)
            {
                // _checkIfExceptionReturn = true;
            }
            finally
            {

            }

            return details;
        }
        //Task wise Attendance
        public async Task<HRAdminTaskwiseAttendanceActualResponse> GetTaskWiseAttendance(HRAdminDashboardRequest request)
        {
            HRAdminTaskwiseAttendanceActualResponse details = new HRAdminTaskwiseAttendanceActualResponse();
            ApiServerResponse<HRAdminTaskwiseAttendanceActualResponse> apiResponse = new ApiServerResponse<HRAdminTaskwiseAttendanceActualResponse>();
            try
            {
                var request_client = new RestRequest(TokenEndpoints.GetTaskWiseAttendanceEndPoint);
                request_client.AddJsonBody(request);
                var response = await _restClient.PostAsync(request_client);
                string content = response.Content.ToString();
                apiResponse = JsonConvert.DeserializeObject<ApiServerResponse<HRAdminTaskwiseAttendanceActualResponse>>(content);

                if (apiResponse != null && apiResponse.ExecutionException is null)
                {
                    details = apiResponse.Value;
                }
                else
                {
                    details = new HRAdminTaskwiseAttendanceActualResponse();
                    throw new Exception("An Exception has been returned ,pls check");

                }

            }
            catch (Exception exp)
            {
                //
            }
            finally
            {

            }

            return details;
        }

        public bool IsExceptionthrown()
        {
            throw new NotImplementedException();
        }

    }
}
