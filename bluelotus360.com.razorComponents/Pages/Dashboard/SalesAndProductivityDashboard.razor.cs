using ApexCharts;
using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.Pages.Dashboard.Finance;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;               
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static bluelotus360.com.razorComponents.Pages.Dashboard.SalesAndProductivityDashboard;
using static MudBlazor.CategoryTypes;

namespace bluelotus360.com.razorComponents.Pages.Dashboard
{
    public partial class SalesAndProductivityDashboard
    {
        #region Support Classes used as Data Models for Apex Charts 
        public class AchievementValue
        {
            public String AchievementLabel { get; set; }
            public decimal Value { get; set; }
        }
        public class CompositionValue
        {
            public String CompositionLabel { get; set; }
            public decimal Value { get; set; }
        }
        public class SubCategoryForUI
        {
            public int SubCategoryKey { get; set; }
            public String SubCategoryName { get; set; }
            public decimal SalesQty { get; set; }
            public string XValue { get; set; } 
        }
        #endregion

        #region Parameters

        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;

        private SalesAndProductivityFilter request= new SalesAndProductivityFilter(); //Contains all data passed in the request

        
        private List<BillCoverage> billCoverage;
        private SalesAndProductivityCombinedModelForMainCharts combinedModel;
        private List<BillProductivity> billProductivity;
        List<AchievementValue> AchievementValues;
        List<CompositionValue> CompositionValues;
        List<String> Categories;
        List<CompositionValue> CategoryCompositionValues;

        List<List<SubCategoryForUI>> SubCategoryInfo;

        ApexChartOptions<BillCoverage> EffectiveCoverageChartOptions;
        ApexChartOptions<BillProductivity> SubDealerProductivityChartOptions;
        ApexChartOptions<AchievementValue> OveralSalesAchievementChartOptions;
        ApexChartOptions<CompositionValue> SalesCompositionChartOptions;
        ApexChartOptions<SubCategoryForUI> SubCategoryChartOptions;
        ApexChartOptions<CompositionValue> CategoryWiseAchievmentChartOptions;

        private SummaryInfo info;
        private RouteSummary routeTable;
        private ShopsForRoute shopsTable;
        Boolean tableLoading = false;
        Boolean showLoadingSignForItemCategoryCharts = false;

        int routeKey=0;
        #endregion

        #region Load Charts
        private async void LoadOverallDetails()
        {

            info = await _dashboardManager.SalesAndProductivityDashboard_GetSummaryDetails() ?? new SummaryInfo();
            
            //info = SampleData.GetSummaryInfo(); 
        }
        private async void LoadRouteDetails()
        {
            routeTable = await _dashboardManager.SalesAndProductivityDashboard_GetRouteDetails() ?? new RouteSummary();
            StateHasChanged();
        }
        private async void LoadDetailsByShop()
        {
            shopsTable = await _dashboardManager.SalesAndProductivityDashboard_GetDetailsByShop(routeKey) ?? new ShopsForRoute();
            StateHasChanged();
        }
        private void LoadEffectiveBillCoverageChart()
        {
            billCoverage = new List<BillCoverage> { combinedModel.BillCoverage };
            //billCoverage = new List<BillCoverage> { SampleData.GetBillCoverage() };

            EffectiveCoverageChartOptions = new ApexChartOptions<BillCoverage>
            {

                PlotOptions = new PlotOptions
                {
                    RadialBar = new PlotOptionsRadialBar
                    {
                        StartAngle = -90,
                        EndAngle = 90
                    }

                }
            };
        }

        private void LoadBillProductivityChart()
        {
            billProductivity = new List<BillProductivity> { combinedModel.BillProductivity };
            //billProductivity = new List<BillProductivity> { SampleData.GetBillProductivity() };

            SubDealerProductivityChartOptions = new ApexChartOptions<BillProductivity>
            {
                
                PlotOptions= new PlotOptions
                {
                    RadialBar= new PlotOptionsRadialBar
                    {
                        StartAngle= -90,
                        EndAngle=90
                    }

                    
                }
            };
        }
        private void LoadOverallSalesAndCompositionCharts()
        {
            OverallSales Sales = combinedModel.OverallSales;
            //OverallSales Sales=SampleData.GetOverallSales();
            AchievementValues = new List<AchievementValue>()
            {
                new AchievementValue()
                {
                    AchievementLabel="Achievement",
                    Value=Sales.AchievedPercentage,
                },
                new AchievementValue()
                {
                    AchievementLabel="Balance",
                    Value=Sales.NonAchievedPercentage,
                }
            };

            CompositionValues = new List<CompositionValue>()
            {
                new CompositionValue()
                {
                    CompositionLabel="Cash",
                    Value=Sales.CashPercentage
                },
                new CompositionValue()
                {
                    CompositionLabel="Credit",
                    Value=Sales.CreditPercentage
                }
            };

            OveralSalesAchievementChartOptions = new ApexChartOptions<AchievementValue>
            {
                Legend = new Legend()
                {
                    Position = LegendPosition.Bottom
                },
                DataLabels = new DataLabels()
                {
                    Enabled = true,

                    //Formatter= @"function(val){ return val+% }"
                },
                Colors = new List<string> { "#1ac734", "#c71a1a" }
            };
            SalesCompositionChartOptions = new ApexChartOptions<CompositionValue>
            {
                Legend = new Legend()
                {
                    Position = LegendPosition.Bottom
                },
                DataLabels = new DataLabels()
                {
                    Enabled = true,

                    //Formatter= @"function(val){ return val+% }"
                },
                Colors = new List<string> { "#1ac734", "#1a28c7" }
            };

        }

        private async void LoadItemCategoriesCombo()
        {
            Categories = new List<string>();
            for(int i = 0;i< combinedModel.MainCategories.Count; i++)
            {
                Categories.Add(combinedModel.MainCategories.ElementAt(i).Value);
            }
        }

        ItemCategoryDetails CategoryDetails;
        private async void LoadItemCategoryCharts()
        {
            CategoryDetails = await _dashboardManager.SalesAndProductivityDashboard_GetItemCategoryDetails(request) ?? new ItemCategoryDetails();
            //ItemCategoryDetails Details =SampleData.GetItemCategoryDetails();
            CategoryCompositionValues = new List<CompositionValue>()
            {
                new CompositionValue()
                {
                    CompositionLabel="Achievement",
                    Value=CategoryDetails.AchievedPercentage,
                },
                new CompositionValue()
                {
                    CompositionLabel="Balance",
                    Value=CategoryDetails.NonAchievedPercentage
                }
            };
            
            List<SubCategory> list =CategoryDetails.SubCategoryList;
            if(list!= null)
            {
                showLoadingSignForItemCategoryCharts = false;
                SubCategoryInfo = new List<List<SubCategoryForUI>>();
                foreach (SubCategory subCategory in list)
                {
                    SubCategoryInfo.Add(new List<SubCategoryForUI> {
                    new SubCategoryForUI() {
                        SubCategoryKey = subCategory.SubCategoryKey,
                        SubCategoryName = subCategory.SubCategoryName,
                        SalesQty= subCategory.SalesQty,
                        XValue=""
                    }
                });
                }
            }
            
            SubCategoryChartOptions= new ApexChartOptions<SubCategoryForUI>
            {
                Chart = new ApexCharts.Chart
                {
                    Stacked = true,
                },
                PlotOptions = new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        DataLabels = new PlotOptionsBarDataLabels
                        {
                            Total = new BarTotalDataLabels
                            {
                                Style = new BarDataLabelsStyle
                                {
                                    FontWeight = "800"
                                }
                            }
                        }
                    }
                },
            };
            CategoryWiseAchievmentChartOptions = new ApexChartOptions<CompositionValue>
            {
                Legend= new Legend() { 
                    Position= LegendPosition.Bottom
                },
                DataLabels= new DataLabels()
                {
                    Enabled= true,
                    
                    //Formatter= @"function(val){ return val+% }"
                },
                Colors = new List<string> { "#1ac734", "#c71a1a" }
            };
            StateHasChanged();
        }
        private async void LoadCharts()
        {
            combinedModel = await _dashboardManager.SalesAndProductivityDashboard_GetPrimaryChartDetails(request) ?? new SalesAndProductivityCombinedModelForMainCharts();
            LoadEffectiveBillCoverageChart();
            LoadBillProductivityChart();
            LoadOverallSalesAndCompositionCharts();
            LoadItemCategoriesCombo();
            StateHasChanged();
        }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            
            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition =  await _navManger.GetMenuUIElement(formrequest); //get ui element
                formDefinition.IsDebugMode = true;


                _interactionLogic = new Dictionary<string, EventCallback>();
                _modalDefinitions = new Dictionary<string, BLUIElement>();
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

                HookInteractions();
            }

            //request.FromDate = DateTime.Now;
            //request.ToDate = DateTime.Now;

            LoadOverallDetails();
            LoadCharts();


            
        }
        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 

            appStateService._AppBarName = "SalesAndProductivity"; //AppSettings.RefreshTopBar("Finance");
        }
        
        #region UI Support
        private void SetGoals(DataPoint<SubCategoryForUI> datapoint)
        {
            datapoint.Goals = new();
            var goal = new DataPointGoal { Name = "Target" };
            datapoint.Goals.Add(goal);

            switch (datapoint.X.ToString())
            {
                case "":
                    goal.StrokeColor = "#e3001b";
                    goal.StrokeDashArray = 5;
                    goal.StrokeHeight = 5;
                    goal.Value = Convert.ToDecimal(CategoryDetails.TargetQty);
                    //goal.Value = Convert.ToDecimal(SampleData.GetItemCategoryDetails().TargetQty);
                    break;
            }
        }
        public bool _isOpen;

        public void ToggleOpen()
        {
            if (_isOpen)
                _isOpen = false;
            else {
                _isOpen = true;
                LoadRouteDetails();
            }
        }

        public bool _isOpenShopsPopup;

        public void ToggleOpenShopDetails()
        {
            if (_isOpenShopsPopup)
                _isOpenShopsPopup = false;
            else
            {
                _isOpenShopsPopup = true;
                LoadDetailsByShop();
            }
        }
        #endregion 

        #region On click actions

        private async void OnCategoryChanged(string value)
        {
            request.ItemCategory3Key = combinedModel.MainCategories.FirstOrDefault(x => x.Value==value).Key;
        }

        private void ClearAllCharts()
        {
            billCoverage=null;
            combinedModel = null;
            billProductivity = null;
            AchievementValues = null;
            CompositionValues = null;
            Categories = null;
            CategoryCompositionValues = null;

            SubCategoryInfo = new List<List<SubCategoryForUI>>();
        }
        private void ClearCategoryCharts()
        {
            CategoryCompositionValues = null;
            SubCategoryInfo = new List<List<SubCategoryForUI>>();
        }
        private async void OnPrimaryLoadButtonClick(UIInterectionArgs<Object> args)
        {
            ClearAllCharts();
            showLoadingSignForItemCategoryCharts = false;
            StateHasChanged();
            LoadCharts();

        }
        private async void OnClickCategoryLoadButton()
        {
            ClearCategoryCharts();
            showLoadingSignForItemCategoryCharts = true;
            StateHasChanged();
            LoadItemCategoryCharts();
        }
        private async void OnRoutesChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            request.AddressCategory3 = args.DataObject;
        }
        private async void OnSalesRepChange(UIInterectionArgs<AddressResponse> args)
        {
            request.SalesRepresentative = args.DataObject;
        }
        private async void OnFromDateChange(UIInterectionArgs<DateTime?> args)
        {
            request.FromDate = args.DataObject;
        }
        private async void OnToDateChange(UIInterectionArgs<DateTime?> args)
        {
            request.ToDate = args.DataObject;
        }
        private void RowClickEvent(TableRowClickEventArgs<SummaryInfo> tableRowClickEventArgs)
        {
            routeKey = tableRowClickEventArgs.Item.RouteKey;
            ToggleOpenShopDetails();
        }


        #endregion

        #region Object helpers
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
        #endregion
    }
}
