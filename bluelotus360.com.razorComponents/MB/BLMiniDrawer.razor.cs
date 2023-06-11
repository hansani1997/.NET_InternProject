using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Shared.Constants.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB
{
    public partial class BLMiniDrawer
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback<bool> OpenThemeManager { get; set; }
        [Parameter]
        public RenderFragment ChildReportContent { get; set; }

        [Parameter]
        public EventCallback<bool> OnDarkModeToggle { get; set; }

		[Parameter] public EventCallback<bool> OnRightToLeftToggle { get; set; }
		public bool IsdarkMode { get; set; }

		private bool _drawerOpen = false;

        private string CompanyName { get; set; }
        private string CurrentUserId { get; set; }
        private string ImageDataUrl { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private bool _rightToLeft = false;
        private string Search { get; set; }
        private bool IsLoading;
        //Loading load;
        private string ProfileAvatar;
        public MenuItem NavMenus { get; set; }
        public MenuItem PinnedMenus { get; set; }
        public MenuItem ReportPinnedMenus { get; set; }
        bool isSearchPopUpShown;
        IList<MenuItem> blMenus=new List<MenuItem>();
        private DialogOptions dialogOptions = new() { FullScreen = true};
        private async Task RightToLeftToggle()
        {
            _rightToLeft = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var user = await _authenticationManager.GetUserInformation();
            if (user == null)
            {
                await _authenticationManager.Logout();
                return;
            }
            if (user != null)
            {
                FirstName = user.AuthenticatedUser.UserID;
                if (!string.IsNullOrEmpty(FirstName))
                {
                    FirstLetterOfName = FirstName[0];
                }
                CompanyName = await _storageService.GetItemAsync<string>(StorageConstants.Local.CompanyName);

            }
            //CompanyName = await _localStorage.GetItemAsync<string>(StorageConstants.Local.CompanyName);
            this.appStateService.LoadStateChanged += this.OnStateChanged;

            IDictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
            menus = await _navManger.GetNavAndPinnedMenus();
            NavMenus = menus["nav-menu"];
            //PinnedMenus = menus["pin-menu"];
            //ReportPinnedMenus = menus["rpt-pin-menu"];

            
        }
        private void OnStateChanged()
            => this.InvokeAsync(StateHasChanged);
        public void Dispose()
            => this.appStateService.LoadStateChanged -= this.OnStateChanged;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
            if (appStateService._AppBarName.Equals("Home"))
            {
                await _jsRuntime.InvokeVoidAsync("SetDomTitle", "Blue Lotus 360");

            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("SetDomTitle", appStateService._AppBarName);
            }

        }

        private async Task LoadDataAsync()
        {
            if (!string.IsNullOrEmpty(FirstName))
            {
                FirstLetterOfName = FirstName[0];
            }
        }

        public void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }


        public async void UpdateHeaderTitle()
        {
            //StateHasChanged();
            await Task.CompletedTask;
        }

        private async void Logout()
        {
            await _authenticationManager.Logout();

        }

        private async void MenuSearchComboChanged(MenuItem menu)
        {

            if (menu != null)
            {
                string url = menu.GetPathURL();
                if (!string.IsNullOrEmpty(url))
                {
                    _navigationManager.NavigateTo(url);
                }

            }


            this.StateHasChanged();
        }

        async Task OpenMenuSearchInMobile() 
        {
            if (blMenus!=null)
            {
                blMenus.Clear();
            }
            isSearchPopUpShown = true;
            await Task.CompletedTask;
        }

       async Task FilterMenus(string search)
       {
            MenuSearchRequest req = new MenuSearchRequest()
            {
                IsBlLite = 1,
                IsPinned = 0,
                ObjectKey = 1,
                Text = search,
            };
            blMenus =await _navManger.SearchBlLiteMenu(req) ??new List<MenuItem>();
        }
        void OnRowClick(MenuItem menu)
        {
            if (menu != null)
            {
                string url = menu.GetPathURL();
                if (!string.IsNullOrEmpty(url))
                {
                    _navigationManager.NavigateTo(url);
                }
                isSearchPopUpShown = false;
            }

            StateHasChanged();
        }

        void OnBack()
        {
            isSearchPopUpShown = false;
        }
    }
}
