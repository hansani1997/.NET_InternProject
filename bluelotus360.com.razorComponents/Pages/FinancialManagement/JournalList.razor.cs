using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities.Financial;
using BL10.CleanArchitecture.Domain.DTO.RequestDTO;
using bluelotus360.Com.commonLib.Helpers;
using System.Reflection.Metadata;
using MudBlazor;
using Microsoft.JSInterop;
using bluelotus360.Com.commonLib.Reports.Telerik;
using static MudBlazor.CategoryTypes;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components.Web;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.MenuFromUber;
using bluelotus360.com.razorComponents.Components.ReportDashboard;
using bluelotus360.com.razorComponents.Pages.Reports.ReportDashboard;
using ApexCharts;

namespace bluelotus360.com.razorComponents.Pages.FinancialManagement
{
    public partial class JournalList
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private BLUIElement buttonSection;
        private BLUIElement tranNumSection;
        private BLUIElement filterSection;
        private BLUIElement headerSection;
        private BLUIElement journalLiteGrid;

        bool open = true;
        long elementKey;
        private JournalFindDTO _findDto;
        private BLJournalLite _journal;
        private IList<JournalLiteFindResponseDTO> _filterDetails;
        private bool IsFilterResponsePopShown = false;
        private BLTable<AccTrnSingleEntry> _blTb;
        private TerlrikReportOptions _journalRerportOption;
        CompletedUserAuth auth;
        private bool journalReportShown;

        private string searchString1 = "";
        private JournalLiteFindResponseDTO selectedItem1 = null;
        private HashSet<JournalLiteFindResponseDTO> selectedItems = new HashSet<JournalLiteFindResponseDTO>();
        private IEnumerable<JournalLiteFindResponseDTO> Elements = new List<JournalLiteFindResponseDTO>();
        private MudTable<JournalLiteFindResponseDTO> _table;

        private AppDbContext _recentlyAccessed = new AppDbContext();

        private DialogOptions dialogOptions = new() { CloseButton = true };

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected  override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements

                if (formDefinition != null)
                {
                    HookInteractions();

                    this.BreakComponent();
                    formDefinition.IsDebugMode = true;
                    buttonSection = formDefinition.Children.Where(x => x._internalElementName.Equals("ButtonSection")).FirstOrDefault();
                    tranNumSection = formDefinition.Children.Where(x => x._internalElementName.Equals("TrnNoSection")).FirstOrDefault();
                    filterSection = formDefinition.Children.Where(x => x._internalElementName.Equals("FilterSection")).FirstOrDefault();
                    headerSection = formDefinition.Children.Where(x => x._internalElementName.Equals("HeaderSection")).FirstOrDefault();
                    journalLiteGrid = formDefinition.Children.Where(x => x._internalElementName.Equals("JournalGrid")).FirstOrDefault();
                }


            }



            InitializeJournalLite();
            auth = await _authenticationManager.GetUserInformation();
            await AddRecentPageAsync();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Journal Lite";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        private void InitializeJournalLite()
        {
            _journal = new BLJournalLite();
            _filterDetails = new List<JournalLiteFindResponseDTO>();
            _blTb = new BLTable<AccTrnSingleEntry>();
            _findDto = new JournalFindDTO();
            _journalRerportOption = new TerlrikReportOptions();
            _journalRerportOption.ReportParameters = new Dictionary<string, object>();

        }

        private void BreakComponent()
        {
            if (formDefinition != null)
            {
                var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
                foreach (var child in formDefinition.Children)
                {
                    child.Children = childsHash[child.ElementKey].ToList();
                }
                BLUIElement form = formDefinition.Children.Where(x => x.ElementKey == formDefinition.ElementKey).FirstOrDefault();
                if (form != null)
                {
                    formDefinition = form;

                }
            }
        }

        #region ui events

        //filter,save,print,cancel btn in header btn section 
        private void OnFilterClick(UIInterectionArgs<object> args)
        {
            this.ToggleDrawer();
            UIStateChanged();
        }
        private async void OnSaveClick(UIInterectionArgs<object> args)
        {
            try
            {
                _journal.ElementKey = elementKey;
                BLJournalLite blobj = await _paymentManager.InsertSingleEntryDetail(_journal);

                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Journal has been  Saved Successfully", Severity.Success);

                if (blobj != null)
                {
                    _journal.CopyFrom(blobj);

                    await SetValue("TopTrnNo", blobj.TransactionNumber);

                }
            }
            catch (Exception ex)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error has been occured", Severity.Error);
            }


            UIStateChanged();
        }
        private void OnPrintClick(UIInterectionArgs<object> args)
        {
            if (_journal != null && _journal.TransactionKey > 1)
            {
                if (_journalRerportOption != null && _journalRerportOption.ReportParameters != null)
                {
                    _journalRerportOption.ReportParameters.Clear();
                    _journalRerportOption.ReportName = "JournalReport.trdp";
                    _journalRerportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _journalRerportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _journalRerportOption.ReportParameters.Add("ObjKy", elementKey);
                    _journalRerportOption.ReportParameters.Add("TrnKy", _journal.TransactionKey);
                    _journalRerportOption.ReportParameters.Add("UsrID", 1);
                    _journalRerportOption.ReportParameters.Add("RptTitle", "");
                    _journalRerportOption.ReportParameters.Add("CNm", "");
                    journalReportShown = true;
                }
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
            }
            UIStateChanged();
        }
        private async void OnCancelClick(UIInterectionArgs<object> args)
        {
            InitializeJournalLite();
            await SetValue("TopTrnNo", string.Empty);
            UIStateChanged();
        }

        //Account_filter section
        private void OnFilterAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            _findDto.Account = args.DataObject;
            UIStateChanged();
        }

        //filterbtn_filter section
        private async void OnFilterGridDetailsClick(UIInterectionArgs<object> args)
        {
            _filterDetails.Clear();
            _findDto.ObjectKey = elementKey;
            _filterDetails = await _paymentManager.FindJournalDetails(_findDto);
            IsFilterResponsePopShown = true;
            UIStateChanged();
        }

        //date/debit/credit/descripyion/add/cancel_inputfieldToMainTable
        private async void OnHeaderDateChange(UIInterectionArgs<DateTime?> args)
        {
            _journal.SelectedSingleEntry.EffectiveDate = (DateTime)args.DataObject;
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnDebitAccountClick(UIInterectionArgs<AccountResponse> args)
        {
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnCreditAccountClick(UIInterectionArgs<AccountResponse> args)
        {
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnDescriptionClick(UIInterectionArgs<string> args)
        {
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnAddtoGridClick(UIInterectionArgs<object> args)
        {
            if (_journal == null)
            {
                _journal = new BLJournalLite();
            }


            if (!_journal.SelectedSingleEntry.IsInEditMode)
            {
                _journal.AccTrnSingleEntries.Add(_journal.SelectedSingleEntry);
                await SetValue("Description", string.Empty);
                await SetValue("Debit", string.Empty);
                await SetValue("Credit", string.Empty);
            }
            else
            {
                if (_journal.EditingLineItem != null)
                {
                    _journal.SelectedSingleEntry.IsInEditMode = false;
                    _journal.EditingLineItem.CopyFrom(_journal.SelectedSingleEntry);


                    await SetValue("Description", string.Empty);
                    await SetValue("Debit", string.Empty);
                    await SetValue("Credit", string.Empty);

                }

            }
            _journal.SelectedSingleEntry = new();


            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}

            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnDetailsCancelClick(UIInterectionArgs<object> args)
        {
            _journal.SelectedSingleEntry = new();
            _journal.SelectedSingleEntry.EffectiveDate = DateTime.Now;
            await SetValue("Description", string.Empty);
            UIStateChanged();
        }

        //edit/delete action_main grid
        private async void OnRowEdit(UIInterectionArgs<object> args)
        {
            AccTrnSingleEntry editRequest = args.DataObject as AccTrnSingleEntry;

            if (editRequest != null)
            {
                editRequest.IsInEditMode = true;

                _journal.EditingLineItem = new();
                _journal.EditingLineItem = editRequest;
                _journal.SelectedSingleEntry.CopyFrom(editRequest);
            }


            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnDeleteRow(UIInterectionArgs<object> args)
        {
            try
            {
                AccTrnSingleEntry deleteRequest = args.DataObject as AccTrnSingleEntry;
                if (deleteRequest != null)
                {
                    deleteRequest.DebitAccountTrnKy = deleteRequest.DebitAccountTrnKy;
                    deleteRequest.CreditAccountTrnKy = deleteRequest.CreditAccountTrnKy;
                    deleteRequest.IsActive = 0;
                    await _paymentManager.DeleteSingleEntry(deleteRequest);
                }

                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}

                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Record has been  deleted Successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error is occured", Severity.Success);
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        #endregion


        #region ext
        //opening drawer after clicking filter btn in header btn section
        async void ToggleDrawer()
        {
            open = !open;

            await _jsRuntime.InvokeVoidAsync("CollapseExpand", open, "grid-section", "filter-section");
        }

        //double click action on popup row 
        public async void OnRowDoubleClickHandler(TableRowClickEventArgs<JournalLiteFindResponseDTO> args)
        {
            var model = args.Item as JournalLiteFindResponseDTO;
            model.ObjectKey = elementKey;
            BLJournalLite singleEntryResponse = await _paymentManager.SelectAccTrnSingleEntryDetail(model);
            if (singleEntryResponse != null)
            {
                _journal.CopyFrom(singleEntryResponse);
                _journal.IsPersisted = true;
                await SetValue("TopTrnNo", _journal.TransactionNumber);
                foreach (var itm in _journal.AccTrnSingleEntries)
                {
                    itm.IsPersisted = true;
                }
            }

            IsFilterResponsePopShown = false;
            UIStateChanged();
        }

      

        //filter response popup closing
        private void OnCloseClick()
        {
            IsFilterResponsePopShown = false;
            this.StateHasChanged();
        }

        #endregion

        #region Object Helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }

        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(visible);
                UIStateChanged();
            }
        }

        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        #endregion

        #region

        private bool FilterFunc1(JournalLiteFindResponseDTO element) => FilterFunc(element, searchString1);

        private bool FilterFunc(JournalLiteFindResponseDTO element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.UserId.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.TransactionNumber} {element.InsertDate}".Contains(searchString))
                return true;
            return false;
        }

        private void PageChanged(int i)
        {
            _table.NavigateTo(i - 1);
        }

        #endregion

        private async Task AddRecentPageAsync()
        {
            string pageName = "Journal List";
            string pageUrl = "/Payment/JournalV4";
            await _recentlyAccessed.AddRecentPageAsync(pageName, pageUrl);
        }
    }
}
