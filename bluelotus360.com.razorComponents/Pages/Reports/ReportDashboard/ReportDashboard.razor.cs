using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Components.ReportDashboard;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.MB;
using BlueLotus360.Com.Shared.Constants.Storage;
using BL10.CleanArchitecture.Domain.Entities.Report;
using bluelotus360.Com.commonLib.Reports.Telerik;
using static Telerik.Blazor.ThemeConstants;
using System.Reflection.Emit;
using System.Reflection;
using System.Text.RegularExpressions;
using static MudBlazor.CategoryTypes;
using BlueLotus.Com.Domain.PartnerEntity;
using Newtonsoft.Json;
using ApexCharts;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using System.ComponentModel.DataAnnotations;
using BL10.CleanArchitecture.Application.Validators.Report;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;

namespace bluelotus360.com.razorComponents.Pages.Reports.ReportDashboard
{
    public partial class ReportDashboard
    {
        //[CascadingParameter(Name = "ReportPinnedMenus")]
        long elementKey = 1;
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private IDictionary<string, string> _dynamicBindings { get; set; }
        MenuItem Childlist = new MenuItem();
        private AppDbContext _recentlyAccessed=new AppDbContext();
        public event Action<ReportModuleItem> OnReportCardClicked;
        IList<ReportSubModule> Childsublist = new List<ReportSubModule>();
        IList<ReportModuleItem> ChildModulelist = new List<ReportModuleItem>();
        IList<ReportFilterFields> ModuleResponselist = new List<ReportFilterFields>();


        BLUIElement DynamicComponent= new BLUIElement();
        private string filterText = "";
        private TerlrikReportOptions _reportOption;
        CompletedUserAuth auth;

        private BLUIElement ShortcutSection, RecentlyAccessedSection, ModulesSection;

        
        private UIBuilder _refBuilder = new UIBuilder();

        private bool isDashboardVisible { get; set; } = true;
        private bool isSubModuleVisible { get; set; }
        public string SearchString { get; private set; }
        private bool isParameterVisible { get; set; } = false;
        private bool isReportVisible { get; set; } = false;
        private bool isPInnnedMenu { get; set; } = false;

        public string FirstName { get; private set; }
        public char FirstLetterOfName { get; private set; }
        public string CompanyName { get; private set; }
        public string? ExpandedTitle { get; private set; }
        public int subModuleParentKey { get; private set; } = 0;
        string reportName = "";
        dynamic? dynamicRepParam;
      
        private IReportValidator validator;

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {

                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;

                formDefinition = await _navManger.GetMenuUIElement(formrequest);
                ShortcutSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ShortcutSection")).FirstOrDefault();
                RecentlyAccessedSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("RecentlyAccessedSection")).FirstOrDefault();
                ModulesSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ModulesSection")).FirstOrDefault();


                if (ShortcutSection != null)
                {
                    ShortcutSection.Children = formDefinition.Children.Where(x => x.ParentKey == ShortcutSection.ElementKey).ToList();
                }

                if (RecentlyAccessedSection != null)
                {
                    RecentlyAccessedSection.Children = formDefinition.Children.Where(x => x.ParentKey == RecentlyAccessedSection.ElementKey).ToList();
                }
                if (ModulesSection != null)
                {
                    ModulesSection.Children = formDefinition.Children.Where(x => x.ParentKey == ModulesSection.ElementKey).ToList();
                }


            }

            formDefinition.IsDebugMode = true;

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _dynamicBindings = new Dictionary<string, string>();

            HookInteractions();
            isSubModuleVisible = false;
            await GetPinnedReportAsync();
            await AddRecentPageAsync();
            await GetReportModulesAsync();
            //await GetReportFilterAsync(11);

            _reportOption = new TerlrikReportOptions();
            auth = await _authenticationManager.GetUserInfoOffline();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "Report Dashboard";
        }

        private void OnStateChanged()
            => this.InvokeAsync(StateHasChanged);

        private async Task GetPinnedReportAsync()
        {
            

            
            Childlist = await _navManger.GetReportPinnedMenus();
            Console.WriteLine(Childlist);
            
            
        }

        private async Task AddRecentPageAsync()
        {
            string pageName = "Report Dashboard";
            string pageUrl = "/report/report_dashboard";
            await _recentlyAccessed.AddRecentPageAsync(pageName, pageUrl);
        }

        private async void HandleReportCardClicked(ReportModuleItem title)
        {
            isSubModuleVisible = true;
            isDashboardVisible = false;
            // Do something with the title object
            await GetReportSubModulesAsync(title.ObjKy);
            subModuleParentKey=title.ObjKy;
            isPInnnedMenu = false;
            StateHasChanged();
        }
        private async Task GetReportSubModulesAsync(int prntKy)
        {
            SubModuleRequest request = new SubModuleRequest();
            request.ParentKey = prntKy;
            Childsublist = await _navManger.GetReportSubModuleMenus(request);
            Console.WriteLine(Childlist);
            StateHasChanged();

        }
        private void SubListBack()
        {    
            isDashboardVisible = true;
            isSubModuleVisible = false;
            isParameterVisible=false; 
            SearchString = "";
            StateHasChanged();
        }
        private void ParameterListBack()
        {
            appStateService._AppBarName = "Report Dashboard";
            if (isPInnnedMenu)
            {
                isDashboardVisible = true;
                isSubModuleVisible = false;
                isParameterVisible = false;
            }
            else 
            {
                isDashboardVisible = false;
                isSubModuleVisible = true;
                isParameterVisible = false;
            }
            StateHasChanged();
        }
        private void SearchSubTitles(ReportModuleItem title)
        {
            ExpandCollapse(title);
        }

        private async void ExpandCollapse(ReportModuleItem title)
        {
            await GetReportSubModulesAsync(title.ObjKy);
            isSubModuleVisible = true;

            if (ExpandedTitle == title.ObjCaptn)
            {
                ExpandedTitle = "";
            }
            else
            {
                ExpandedTitle = title.ObjCaptn;
            }
            
            StateHasChanged();
        }

        private async Task GetReportModulesAsync()
        {



            ChildModulelist = await _navManger.GetReportModuleMenus();
            Console.WriteLine(Childlist);


        }
        public async void OnAfterPinnedMenuClick(MenuItem pincard)
        {
            ReportSubModule report = 
                new ReportSubModule 
                { 
                    ReportPath = pincard.ReportPath, 
                    ObjCaptn = pincard.MenuCaption,
                    ObjKy = pincard.ChildKey,
                    PrntKy = pincard.MenuId
                };
            await SubMenuButtonClicked(report);
            isPInnnedMenu = true;
        }
        public async Task SubMenuButtonClicked(ReportSubModule report)
        {
            appStateService._AppBarName = report.ObjCaptn;
            string[] reportPath = new string[] {};
            reportName = "";
            if (!string.IsNullOrEmpty(report.ReportPath))
            {
                if (report.ReportPath.Contains("\\"))
                {
                    reportPath = report.ReportPath.Split("\\");
                }
                else if (report.ReportPath.Contains("/"))
                {
                    reportPath = report.ReportPath.Split("/");
                }
                
                if (reportPath.Length>1)
                {
                    if (reportPath.Length==2)
                    {
                        reportName = !string.IsNullOrEmpty(reportPath[1]) ? reportPath[1] : "";
                    }
                    if (reportPath.Length == 3)
                    {
                        reportName = !string.IsNullOrEmpty(reportPath[2]) ? reportPath[2] : "";
                    }
                }
                
            }

            await GetReportFilterAsync(report.ObjKy);

            isDashboardVisible = false;
            isSubModuleVisible= false;
            isParameterVisible = true;
            StateHasChanged();
        }

        //private IList<ReportFilterFields> ModuleResponselist = new List<ReportFilterFields>();
        private IList<ReportFilterFields> visibleFields = new List<ReportFilterFields>();
        private Dictionary<long, dynamic> fieldValues = new Dictionary<long, dynamic>();
        private ReportFilterFields formModel = new ReportFilterFields();

        private async Task GetReportFilterAsync(int prntKy)
        {
            string propName = "";
            Type t = typeof(string);

            SubModuleRequest request = new SubModuleRequest();
            request.ParentKey = prntKy;
            ModuleResponselist = await _navManger.GetReportFilterFields(request);

            IList<BLUIElement> dynamicUIComponentList = new List<BLUIElement>();
            DynamicComponent = new BLUIElement() {
                ElementType = "SectnFrmtGrp",
            };

            foreach (var list in ModuleResponselist)
            {
                BLUIElement ui = new BLUIElement();
                ui.ElementKey = list.objectKey;
                ui.ParentKey = list.parentKey;
                ui.ElementName = list.objectName;
                ui._internalElementName= list.objectName;
                ui.ElementCaption = list.objectCaption;
                ui.ToolTip = list.ToolTip;
                ui.DefaultValue = list.defaultValue.ToString();
                ui.IsEnable = Convert.ToBoolean(list.IsEnable);
                ui.IsFreeze = Convert.ToBoolean(list.isFreeze);
                ui.IsMust = Convert.ToBoolean(list.isMust);
                ui.IsVisible= Convert.ToBoolean(list.IsVisible);
                //ui.Width= list.width;
                
                ui.Format=list.format;
                //ui.UrlController = list.uRLController;
                //ui.UrlController = list.uRLAction;
                ui.EnterKeyAction = list.entryKeyAction;
                ui.OurCode = list.ourCd;
                ui.OurCode2 = list.ourCd2;
                ui.DefaultAccessPath = list.DefaultAccessPath;
                ui.OnClickAction= list.OnClickAction;
                ui.IsDynamicalyLoaded = true;
                

                if (!string.IsNullOrEmpty(ui._internalElementName))
                {
                    
                    if (ui._internalElementName.StartsWith("cmFrmCdMas_", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmFrmCdMas_)\w+").Value;
                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "CodeBase";
                            ui.UrlController = "Codebase";
                            ui.UrlAction = "ReadCodes";

                            t= typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }

                       

                    }
                    else if (ui._internalElementName.StartsWith("cmbItm", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbItm)\w+").Value;
                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "Item";
                            ui.UrlController = "Item";
                            ui.UrlAction = "GetItemsForTransactionJson";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }
                    }
                    else if (ui._internalElementName.StartsWith("cmbAdr", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbAdr)\w+").Value;

                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "Address";
                            ui.UrlController = "Address";
                            ui.UrlAction = "ReadAddress";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }



                    }
                    else if (ui._internalElementName.StartsWith("cmbAcc", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbAcc)\w+").Value;
                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "Account";
                            ui.UrlController = "Account";
                            ui.UrlAction = "ReadAccounts";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }

                    }
                    else if (ui._internalElementName.StartsWith("cmbPrj", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbPrj)\w+").Value;
                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "Project";
                            ui.UrlController = "Project";
                            ui.UrlAction = "GetAllProjects";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }

                       
                    }
                    else if (ui._internalElementName.StartsWith("cmbTask", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbTask)\w+").Value;

                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "Task";
                            ui.UrlController = "Project";
                            ui.UrlAction = "GetTasks";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }
                    }
                    else if (ui._internalElementName.StartsWith("cmbPrcsSch", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        propName = Regex.Match(ui._internalElementName, @"(?<=cmbPrcsSch)\w+").Value;
                        if (!string.IsNullOrEmpty(propName) && propName.EndsWith("Ky"))
                        {
                            ui.ElementType = "Cmb";
                            ui.ElementID = "??";
                            ui.UrlController = "??";
                            ui.UrlAction = "??";
                            t = typeof(int);
                        }
                        else
                        {
                            ui.ElementType = "TextBox";
                            ui.IsVisible = false;
                            t = typeof(string);
                        }
                    }
                    else if (ui._internalElementName.StartsWith("textBox", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        ui.ElementType = "TextBox";
                        propName = Regex.Match(ui._internalElementName, @"(?<=textBox)\w+").Value;
                        t = typeof(string);
                    }
                    else if (ui._internalElementName.StartsWith("datPic", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        ui.ElementType = "DatePicker";
                        propName = Regex.Match(ui._internalElementName, @"(?<=datPic)\w+").Value;
                        t = typeof(DateTime?);
                    }
                    else if (ui._internalElementName.StartsWith("numricBox", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        ui.ElementType = "NumericBox";
                        propName = Regex.Match(ui._internalElementName, @"(?<=numricBox)\w+").Value;
                        t = typeof(decimal);
                    }
                    else if (ui._internalElementName.StartsWith("chckBox", StringComparison.OrdinalIgnoreCase))
                    {
                        ui.Md = 6;
                        ui.ElementType = "CheckBox";
                        propName = Regex.Match(ui._internalElementName, @"(?<=chckBox)\w+").Value;
                        t = typeof(bool);
                    }

                    ui.DefaultAccessPath= propName;
                    dynamicUIComponentList.Add(ui);

                    if (ui.IsMust)
                    {
                        ui.IsMustElements.Add(propName);
                    }
                    
                }
            }

            //ui.IsMustElements= dynamicUIComponentList.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();


            DynamicComponent.Children = dynamicUIComponentList;

            StateHasChanged();
        }

       

        private async void OnShowReportClick()
        {
            
           
           
            _reportOption.ReportParameters = new Dictionary<string, object>();
            List<string> cdmasKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmFrmCdMas_")).Select(x => x.Key).ToList()??new List<string>();
            List<string> adrKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbAdr")).Select(x => x.Key).ToList() ?? new List<string>();
            List<string> accKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbAcc")).Select(x => x.Key).ToList() ?? new List<string>();
            List<string> itmKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbItm")).Select(x => x.Key).ToList() ?? new List<string>();
            List<string> prjKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbPrj")).Select(x => x.Key).ToList() ?? new List<string>();
            List<string> taskKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbTask")).Select(x => x.Key).ToList() ?? new List<string>();    
            List<string> prcsSchKeyList = _dynamicBindings.Where(x => x.Key.StartsWith("cmbPrcsSch")).Select(x => x.Key).ToList() ?? new List<string>();




            



                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = reportName;
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("UsrId", auth.AuthenticatedUser.UserID);

                    foreach (KeyValuePair<string,string> reportParam in _dynamicBindings)
                    {
                        if (reportParam.Value!=null)
                        {
                            if (reportParam.Key.StartsWith("cmFrmCdMas_", StringComparison.OrdinalIgnoreCase)&& reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<CodeBaseResponse> code = new UIInterectionArgs<CodeBaseResponse>();

                                code = JsonConvert.DeserializeObject<UIInterectionArgs<CodeBaseResponse>>(reportParam.Value);
                                if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                {
                                    _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.CodeKey);
                                }
                                else
                                {
                                    _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.CodeKey;
                                }
                                 string prefix= Regex.Match(reportParam.Key, @"cmFrmCdMas_(.+?)Ky").Groups[1].Value;

                                foreach (var itm in cdmasKeyList)
                                {
                                    if (itm.Contains(prefix) && (itm.EndsWith("Cd") || itm.EndsWith("Nm")))
                                    {
                                        string objpath =  Regex.Match(itm, @"(?<=cmFrmCdMas_)\w+").Value; 
                                     
                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.CodeName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.CodeName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<CodeBaseResponse>() { ObjectPath = code.ObjectPath, DataObject = new CodeBaseResponse() });
                                
                                
                            }
                            else if (reportParam.Key.StartsWith("cmbItm", StringComparison.OrdinalIgnoreCase) && reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                        UIInterectionArgs<ItemResponse> code = JsonConvert.DeserializeObject<UIInterectionArgs<ItemResponse>>(reportParam.Value);
                                
                                        if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                        {
                                            _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.ItemKey);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.ItemKey;
                                        }
                                string prefix = Regex.Match(reportParam.Key, @"cmbItm(.+?)Ky").Groups[1].Value;
                                foreach (var itm in itmKeyList)
                                {
                                    if (itm.Contains(prefix) &&(itm.EndsWith("Cd") || itm.EndsWith("Nm")))
                                    {
                                        string objpath = Regex.Match(itm, @"(?<=cmbItm)\w+").Value;

                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.ItemName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.ItemName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<ItemResponse>() { ObjectPath = code.ObjectPath, DataObject = new ItemResponse() });

                            }
                            else if (reportParam.Key.StartsWith("cmbAdr", StringComparison.OrdinalIgnoreCase) && reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<AddressResponse> code = JsonConvert.DeserializeObject<UIInterectionArgs<AddressResponse>>(reportParam.Value);
                                
                                        if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                        {
                                            _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.AddressKey);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.AddressKey;
                                        }
                                string prefix = Regex.Match(reportParam.Key, @"cmbAdr(.+?)Ky").Groups[1].Value;
                                foreach (var itm in adrKeyList)
                                {
                                    if (itm.Contains(prefix) && (itm.EndsWith("Cd") || itm.EndsWith("Nm")))
                                    {
                                        string objpath = Regex.Match(itm, @"(?<=cmbAdr)\w+").Value;

                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.AddressName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.AddressName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<AddressResponse>() { ObjectPath = code.ObjectPath, DataObject = new AddressResponse() });

                            }
                            else if (reportParam.Key.StartsWith("cmbAcc", StringComparison.OrdinalIgnoreCase) && reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<AccountResponse> code = JsonConvert.DeserializeObject<UIInterectionArgs<AccountResponse>>(reportParam.Value);
                                
                                        if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                        {
                                            _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.AccountKey);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.AccountKey;
                                        }
                                string prefix = Regex.Match(reportParam.Key, @"cmbAcc(.+?)Ky").Groups[1].Value;
                                foreach (var itm in accKeyList)
                                {
                                    if (itm.Contains(prefix) && (itm.EndsWith("Cd") || itm.EndsWith("Nm")))
                                    {
                                        string objpath = Regex.Match(itm, @"(?<=cmbAcc)\w+").Value;

                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.AccountName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.AccountName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<AccountResponse>() { ObjectPath = code.ObjectPath, DataObject = new AccountResponse() });

                            }
                            else if (reportParam.Key.StartsWith("cmbPrj", StringComparison.OrdinalIgnoreCase) && reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                        UIInterectionArgs<ProjectResponse> code = JsonConvert.DeserializeObject<UIInterectionArgs<ProjectResponse>>(reportParam.Value);
                                
                                        if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                        {
                                            _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.ProjectKey);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.ProjectKey;
                                        }
                                string prefix = Regex.Match(reportParam.Key, @"cmbPrj(.+?)Ky").Groups[1].Value;
                                foreach (var itm in prjKeyList)
                                {
                                    if (itm.EndsWith("Cd") || itm.EndsWith("Nm"))
                                    {
                                        string objpath = Regex.Match(itm, @"(?<=cmbPrj)\w+").Value;

                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.ProjectName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.ProjectName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<ProjectResponse>() { ObjectPath = code.ObjectPath, DataObject = new ProjectResponse() });

                            }
                            else if (reportParam.Key.StartsWith("cmbTask", StringComparison.OrdinalIgnoreCase) && reportParam.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<TaskResponse> code = JsonConvert.DeserializeObject<UIInterectionArgs<TaskResponse>>(reportParam.Value);
                                

                                        if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                        {
                                            _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject.TaskKey);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.TaskKey;
                                        }
                                string prefix = Regex.Match(reportParam.Key, @"cmbTask(.+?)Ky").Groups[1].Value;
                                foreach (var itm in taskKeyList)
                                {
                                    if (itm.Contains(prefix) && (itm.EndsWith("Cd") || itm.EndsWith("Nm")))
                                    {
                                        string objpath = Regex.Match(itm, @"(?<=cmbTask)\w+").Value;

                                        if (!_reportOption.ReportParameters.ContainsKey(objpath))
                                        {
                                            _reportOption.ReportParameters.Add(objpath, code.DataObject.TaskName);
                                        }
                                        else
                                        {
                                            _reportOption.ReportParameters[code.ObjectPath] = code.DataObject.TaskName;
                                        }
                                    }
                                }

                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<ProjectResponse>() { ObjectPath = code.ObjectPath, DataObject = new ProjectResponse() });

                            }
                            else if (reportParam.Key.StartsWith("datPic", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<DateTime?> code = JsonConvert.DeserializeObject<UIInterectionArgs<DateTime?>>(reportParam.Value);
                                if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                {
                                    _reportOption.ReportParameters.Add(code.ObjectPath, (DateTime)code.DataObject);
                                }
                                else
                                {
                                    _reportOption.ReportParameters[code.ObjectPath] = (DateTime)code.DataObject;
                                }
                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<DateTime?>() { ObjectPath = code.ObjectPath, DataObject = DateTime.Now });
                            }
                            else if (reportParam.Key.StartsWith("textBox", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<string> code = JsonConvert.DeserializeObject<UIInterectionArgs<string>>(reportParam.Value);
                                if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                {
                                    _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject);
                                }
                                else
                                {
                                    _reportOption.ReportParameters[code.ObjectPath] = code.DataObject;
                                }
                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<string>() { ObjectPath = code.ObjectPath, DataObject = string.Empty });
                            }
                            else if (reportParam.Key.StartsWith("numricBox", StringComparison.OrdinalIgnoreCase))
                            {
                                UIInterectionArgs<decimal> code = JsonConvert.DeserializeObject<UIInterectionArgs<decimal>>(reportParam.Value);
                                if (!_reportOption.ReportParameters.ContainsKey(code.ObjectPath))
                                {
                                    _reportOption.ReportParameters.Add(code.ObjectPath, code.DataObject);
                                }
                                else
                                {
                                    _reportOption.ReportParameters[code.ObjectPath] = code.DataObject;
                                }
                                _dynamicBindings[reportParam.Key] = JsonConvert.SerializeObject(new UIInterectionArgs<decimal>() { ObjectPath = code.ObjectPath, DataObject = 0 });
                            }
                        }
                        
                    }

                //isReportVisible = true;

                validator = new ReportDashboardValidator(DynamicComponent, _reportOption.ReportParameters);

                if (validator != null && validator.CanSelectItem())
                {
                    isReportVisible = true;
                }


            }
            



            StateHasChanged();
        }

        //private void CreateDynamicReportModel(IList<ReportProperty> list)
        //{

        //    // create a new assembly and module
        //    AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
        //    AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

        //    // create a new type builder
        //    TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);

        //    // create a property for each item in the list
        //    foreach (var kvp in list)
        //    {
        //        PropertyBuilder propBuilder = typeBuilder.DefineProperty(kvp.PropertName, PropertyAttributes.HasDefault, kvp.T, null);
        //        MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        //        // create the getter method
        //        MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + kvp.PropertName, getSetAttr, kvp.T, Type.EmptyTypes);
        //        ILGenerator getIL = getMethodBuilder.GetILGenerator();
        //        getIL.Emit(OpCodes.Ldarg_0);
        //        getIL.Emit(OpCodes.Call, typeof(object).GetMethod("ToString"));
        //        getIL.Emit(OpCodes.Ret);

        //        // create the setter method

        //        MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + kvp.PropertName, getSetAttr, null, new Type[] { kvp.T });
        //        ILGenerator setIL = setMethodBuilder.GetILGenerator();
        //        setIL.Emit(OpCodes.Ldarg_0);
        //        setIL.Emit(OpCodes.Ldarg_1);
        //        setIL.Emit(OpCodes.Call, typeof(object).GetMethod("ToString"));
        //        setIL.Emit(OpCodes.Ret);

        //        // set the getter and setter methods on the property

        //        propBuilder.SetGetMethod(getMethodBuilder);
        //        propBuilder.SetSetMethod(setMethodBuilder);
        //    }

        //    // create the type and get a type reference
        //    Type type = typeBuilder.CreateType();

        //    // create an instance of the class
        //    dynamicRepParam = Activator.CreateInstance(type);


        //}  

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
    }

    public class ReportProperty
    {
        public Type T { get; set; }
        public string PropertName { get; set; }
    }
}
