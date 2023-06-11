using bluelotus360.com.razorComponents.Pages.Reports.ReportDashboard;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.ReportDashboard
{
    public partial class RecentlyAccessed
    {
        private string[] Titles = new string[] { "Title 01", "Title 02", "Title 03", "Title 04", "Title 05", "Title 06" };
        private AppDbContext _recentlyAccessed= new AppDbContext();
        List<RecentlyAccessedPage> list = new List<RecentlyAccessedPage>();

        protected override async void OnInitialized()
        {
            list = await _recentlyAccessed.GetContents();
            //await GetRecentPages();
            StateHasChanged();
        }

        //private async Task GetRecentPages()
        //{
            
            
            
           
        //}

        //private async Task AddRecentPageAsync()
        //{
        //    string pageName = (string)document.getElementById("pageName").value;
        //    string pageUrl = (string)document.getElementById("pageUrl").value;
        //    await _recentlyAccessed.AddRecentPageAsync(pageName, pageUrl);
        //}

        


        private void ExpandCollapse(string title)
        {
            
        }

        public async Task NavigateToNewTab(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                //await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
                _navigationManager.NavigateTo(url);
            }
        }
    }
}
