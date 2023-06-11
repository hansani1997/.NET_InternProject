using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.Human_Resource.Profile
{
    public partial class EmployeePortal
    {
        #region parameters
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private EmployeeModel _empModel;
        private LeaveDetails _leave;
        private UserDetails _user;
        private IList<LeaveSummary> leaveSummary;
        string ImageDataUrl = "";
        string firstName = "";
        char _firstLetterOfName;
        bool isLoading = false;
        long elementKey;
        #endregion

        #region General
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            InitializeEmployee();
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }
            if (formDefinition!=null)
            {
                formDefinition.IsDebugMode = true;
            }
            
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();

            _user = await _hrManager.GetUserAsync();

            GetProfileDetails();
            await LoadLeaveSummary(new DateTime(DateTime.Now.Year, 1, 1));

            isLoading = false;
            

        }

        private void InitializeEmployee()
        {
            _user = new UserDetails();
            _leave = new LeaveDetails();
            _empModel = new EmployeeModel();
            leaveSummary = new List<LeaveSummary>();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//form definition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks
            //AppSettings.RefreshTopBar("Employee Portal");                                              // 
            appStateService._AppBarName = "Employee Portal";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        private async void GetProfileDetails()
        {
            if (leaveSummary!=null)
            {
                _empModel = await _hrManager.LoadEmployeeDetails();
                _empModel.LeaveSummary = leaveSummary;
                LoadImage();

                UIStateChanged();
            }
            
        }

        private async Task LoadLeaveSummary(DateTime year)
        {
            if (_user!=null && _leave!=null)
            {
                _leave.EmpKy = _user.AdrKy;
                _leave.Year = year;
                leaveSummary = await _hrManager.LoadLeaveSummary(_leave);
                UIStateChanged();
            }
            
        }

        private void LoadImage()
        {
            if (_empModel!=null)
            {
                var data = _empModel.ProfPicture.FileContent;

                if (data != null)
                {
                    ImageDataUrl = "data:image/png;base64," + data;
                    UIStateChanged();
                }
                if (_empModel.EmployeeBasicDetails!=null  && _empModel.EmployeeBasicDetails.EmployeeName.Length > 0)
                {
                    _firstLetterOfName = _empModel.EmployeeBasicDetails.EmployeeName[0];
                    firstName = _empModel.EmployeeBasicDetails.EmployeeName.Split(".")[0];
                    UIStateChanged();
                }
            }
            

            


        }
    }
}
