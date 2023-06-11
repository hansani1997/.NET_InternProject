using BL10.CleanArchitecture.Domain.Entities.Report;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;

namespace bluelotus360.com.razorComponents.Components.ReportDashboard
{
    public partial class ReportModels
    {
       
        //private string[] Titles = new string[] { "Title 01", "Title 02", "Title 03", "Title 04", "Title 05", "Title 06" };
        private string ExpandedTitle = "";
        private string filterText = "";
        //private string SearchString = "";
        private bool visible;
        private string SearchString { get; set; }
        
        //IList<ReportModuleItem> Childlist=new List<ReportModuleItem>();
        IList<ReportSubModule> Childsublist = new List<ReportSubModule>();
        [Parameter]
        public IList<ReportModuleItem> Childlist { get; set; }
        [Parameter]
        public EventCallback<ReportModuleItem> OnReportCardClicked { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //await GetReportModulesAsync();
        }


        //    private void Submit()
        //{
        //    visible = false;
        //    SearchString = "";
        //    StateHasChanged();
        //}

        //private DialogOptions dialogOptions = new() { FullScreen = true };

        //private List<string> GetSubTitles(string title)
        //{
        //    List<string> subTitles = new List<string>();
        //    for (int i = 1; i <= 6; i++)
        //    {
        //        subTitles.Add($"{title} - Report{i}");
        //    }
        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        subTitles = subTitles.Where(s => s.Contains(SearchString, StringComparison.OrdinalIgnoreCase)).ToList();
        //    }
        //    return subTitles;
        //}


        //private async void ExpandCollapse(ReportModuleItem title)
        //{
        //    await GetReportSubModulesAsync(title.ObjKy);
        //    visible = true;
        //    if (ExpandedTitle == title.ObjCaptn)
        //    {
        //        ExpandedTitle = "";
        //    }
        //    else
        //    {
        //        ExpandedTitle = title.ObjCaptn;
        //    }
        //    OnReportCardClicked.InvokeAsync(title);
        //    StateHasChanged();
        //}

        //private void SearchSubTitles(ReportModuleItem title)
        //{
        //    ExpandCollapse(title);
        //}



        //private void OnSearchInputChange(ChangeEventArgs e)
        //{
        //    SearchString = e.Value.ToString();
        //}


        //private async Task GetReportModulesAsync()
        //{



        //    Childlist = await _navManger.GetReportModuleMenus();
        //    Console.WriteLine(Childlist);


        //}

        //private async Task GetReportSubModulesAsync(int prntKy)
        //{
        //    SubModuleRequest request= new SubModuleRequest();
        //    request.ParentKey = prntKy;
        //    Childsublist = await _navManger.GetReportSubModuleMenus(request);
        //    Console.WriteLine(Childlist);

        //}

        //public async Task NavigateToNewTab(string URL)
        //{
        //    if (!string.IsNullOrEmpty(URL))
        //    {
        //        string url = URL;

        //        _navigationManager.NavigateTo(url);
        //    }
        //}

        private async void ModuleOnClick(ReportModuleItem title)
        {
            if (OnReportCardClicked.HasDelegate) {
                OnReportCardClicked.InvokeAsync(title);
            }

            StateHasChanged();
             

        }

    }
}
