using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Projects.ProjectProfileMobile.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Projects.ProjectProfileMobile
{
    public partial class ProjectProfileMobile
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLTransaction transaction = new();

        private ProjectProfileRequest projectProfileRequest;
        private ProjectProfileList updateRequest, insertRequest, currentDetails, updatedDetails;

        private UIBuilder _refBuilder;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private IList<ProjectProfileList> gridDetails;

        private BLProjectProfileInsertBasicDetails _bLProjectProfileInsertBasicDetails;
        private BLProjectProfileUpdateBasicDetails _bLProjectProfileUpdateBasicDetails;
        private BLTelGrid<ProjectProfileList> _blTb;

        private bool showsgrid = true;
        private bool ShowInsertDetails = false;
        private bool ShowUpdateDetails = false;
        private bool isTableLoading = false;

        BLUIElement inmodalUIElement, upmodalUIElement,gridUIElement;

        long elementKey;

        #endregion

        #region General

        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey); 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);

                inmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InsertBasicDetails")).FirstOrDefault();
                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();
                upmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UpdateBasicDetails")).FirstOrDefault();

                if (inmodalUIElement != null)
                {
                    inmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == inmodalUIElement.ElementKey).ToList();
                }
                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }
                if (upmodalUIElement != null)
                {
                    upmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == upmodalUIElement.ElementKey).ToList();
                }
            }

            formDefinition.IsDebugMode = true;

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();
            RefreshGrid();

            projectProfileRequest.ElementKey = elementKey;

            gridDetails = await _projectProfileMobileManager.GetProjectProfileList(projectProfileRequest);

        }

        private async void InitNewLine()
        {
            insertRequest = new ProjectProfileList();
            //_objectHelpers["ProjectNo"].ResetToInitialValue();
            _objectHelpers["ProjectId"].ResetToInitialValue();
            _objectHelpers["ProjectName"].ResetToInitialValue();
            _objectHelpers["ProjectTypeKey"].ResetToInitialValue();
            _objectHelpers["ParentProject"].ResetToInitialValue();
            _objectHelpers["ProjectStartDate"].ResetToInitialValue();
            _objectHelpers["ProjectEndDate"].ResetToInitialValue();
            _objectHelpers["Item"].ResetToInitialValue();
            _objectHelpers["ProjectStatus"].ResetToInitialValue();
            _objectHelpers["ProjectAlias"].ResetToInitialValue();
            _objectHelpers["IsAct"].ResetToInitialValue();
            _objectHelpers["IsApprove"].ResetToInitialValue();
            _objectHelpers["IsAlwTrn"].ResetToInitialValue();
            _objectHelpers["IsParent"].ResetToInitialValue();
            StateHasChanged();
        }
        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            //AppSettings.RefreshTopBar("Project Profile Mobile");
            appStateService._AppBarName = "Project Profile Mobile";
        }
        private void RefreshGrid() 
        {
            projectProfileRequest = new ProjectProfileRequest();
            updateRequest = new ProjectProfileList();
            insertRequest = new ProjectProfileList();
            currentDetails = new ProjectProfileList();
            updatedDetails = new ProjectProfileList();
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion

        #region Ui Related Events

        private async void OnNewAdd(UIInterectionArgs<object> args) 
        {
        
            showsgrid = false;
            ShowInsertDetails = true;

            UIStateChanged();
        }

        #endregion

        #region Insert Related Events

        private async void OnInsertIsPrnt(UIInterectionArgs<bool> args)
        {
            insertRequest.IsPrnt = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsAlwTrn(UIInterectionArgs<bool> args)
        {
            insertRequest.IsAlwTrn = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsApr(UIInterectionArgs<bool> args)
        {
            insertRequest.IsApr = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsAct(UIInterectionArgs<bool> args)
        {
            insertRequest.IsAct = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertAlias(UIInterectionArgs<string> args)
        {
            insertRequest.Alias = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertProjectStatus(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertRequest.ProjectStatus = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItem(UIInterectionArgs<ItemResponse> args)
        {
            insertRequest.Item = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertFinDt(UIInterectionArgs<DateTime?> args)
        {
            insertRequest.FinDt = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertPrjStDt(UIInterectionArgs<DateTime?> args)
        {
            insertRequest.PrjStDt = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertPrjTyp(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertRequest.PrjTyp = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertPrjNm(UIInterectionArgs<string> args)
        {
            insertRequest.PrjNm = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertPrjID(UIInterectionArgs<string> args)
        {
            insertRequest.PrjID = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertPrjNo(UIInterectionArgs<string> args)
        {
            insertRequest.PrjNo = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertBack(UIInterectionArgs<object> args) 
        {
            InitNewLine();
            RefreshGrid();

            ShowInsertDetails = false;
            showsgrid = true;

            UIStateChanged();
        }
        private async void OnInsertSave(UIInterectionArgs<object> args) 
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _projectProfileMobileManager.InsertProjectList(insertRequest);
            gridDetails = await _projectProfileMobileManager.GetProjectProfileList(projectProfileRequest);

            InitNewLine();
            RefreshGrid();

            ShowInsertDetails = false;
            showsgrid = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        #endregion

        #region Update Related Events

        private async void OnUpdatePrjNo(UIInterectionArgs<string> args) 
        {
            updatedDetails.PrjNo = args.DataObject;
            UIStateChanged();
        }

        private async void OnUpdatePrjID(UIInterectionArgs<string> args) 
        {
            updatedDetails.PrjID = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdatePrjNm(UIInterectionArgs<string> args) 
        {
            updatedDetails.PrjNm = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdatePrjTyp(UIInterectionArgs<CodeBaseResponse> args) 
        {
            updatedDetails.PrjTyp = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateParentPrj(UIInterectionArgs<CodeBaseResponse> args) 
        {
            UIStateChanged();
        }
        private async void OnUpdatePrjStDt(UIInterectionArgs<DateTime?> args) 
        {
            updatedDetails.PrjStDt = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateFinDt(UIInterectionArgs<DateTime?> args) 
        {
            updatedDetails.FinDt = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateItem(UIInterectionArgs<ItemResponse> args)
        {
            updatedDetails.Item = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateProjectStatus(UIInterectionArgs<CodeBaseResponse> args) 
        {
            updatedDetails.ProjectStatus = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateAlias(UIInterectionArgs<string> args) 
        {
            updatedDetails.Alias = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsAct(UIInterectionArgs<bool> args) 
        {
            updatedDetails.IsAct = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsApr(UIInterectionArgs<bool> args) 
        {
            updatedDetails.IsApr = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsAlwTrn(UIInterectionArgs<bool> args) 
        {
            updatedDetails.IsAlwTrn = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsPrnt(UIInterectionArgs<bool> args) 
        {
            updatedDetails.IsPrnt = args.DataObject;
            UIStateChanged();
        }

        private async void OnUpdateBack(UIInterectionArgs<object> args) 
        {

            InitNewLine();

            ShowUpdateDetails = false;
            showsgrid = true;

            UIStateChanged();
        }
        private async void OnUpdateSave(UIInterectionArgs<object> args) 
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _projectProfileMobileManager.UpdateProjectList(updatedDetails);
            gridDetails = await _projectProfileMobileManager.GetProjectProfileList(projectProfileRequest);

            InitNewLine();
            RefreshGrid();

            ShowUpdateDetails = false;
            showsgrid = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        #endregion

        #region Grid Level Events

        //private async void OnProjectKey(UIInterectionArgs<string> args)
        //{
        //    UIStateChanged();
        //}
        private async void EditProfile(UIInterectionArgs<object> args) 
        {
            isTableLoading = true;
            UIStateChanged();

            currentDetails = (ProjectProfileList)args.DataObject;

            updateRequest.CopyFrom(currentDetails);

            ////await SetValue("ProjectNo", currentDetails.PrjNo);
            await SetValue("ProjectIdUp", currentDetails.PrjID);
            await SetValue("ProjectNameUp", currentDetails.PrjNm);
            ////await SetValue("ProjectTypeKey", currentDetails.PrjTyp.CodeKey);
            ////await SetValue("ParentProject", currentDetails.PrntKy);
            await SetValue("ProjectStartDateUp", currentDetails.PrjStDt);
            await SetValue("ProjectEndDateUp", currentDetails.FinDt);
            ////await SetValue("Item", currentDetails.Item.ItemKey);
            await SetValue("ProjectStatusUp", currentDetails.ProjectStatus.CodeKey);
            await SetValue("ProjectAliasUp", currentDetails.Alias);
            await SetValue("IsActUp", currentDetails.IsAct);
            await SetValue("IsApproveUp", currentDetails.IsApr);
            await SetValue("IsAlwTrnUp", currentDetails.IsAlwTrn);
            await SetValue("IsParentUp", currentDetails.IsPrnt);

            showsgrid = false;
            ShowUpdateDetails = true;

            isTableLoading = false;
            UIStateChanged();
        }

        #endregion

        #region object helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }

        #endregion

    }
}
