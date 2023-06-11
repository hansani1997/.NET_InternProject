using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Shared.Constants.Application;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace BlueLotus360.Com.UI.Definitions.MB.Shared
{
    public partial class BLMiniDrawer
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnDarkModeToggle { get; set; }

        [Parameter]
        public EventCallback<bool> OnRightToLeftToggle { get; set; }

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
        Loading load ;
        private string ProfileAvatar;
        public MenuItem NavMenus { get; set; }
        public MenuItem PinnedMenus { get; set; }
        private async Task RightToLeftToggle()
        {
           
            _rightToLeft = false;

        
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
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
            }

            CompanyName = await _localStorage.GetItemAsync<string>(StorageConstants.Local.CompanyName);
            // _rightToLeft = await _clientPreferenceManager.IsRTL();
            // _interceptor.RegisterEvent();
            //hubConnection = hubConnection.TryInitialize(_navigationManager, _localStorage);
            //AppSettings._miniDrawer = this;
            //  menuItem = await _navManger.GetNavigationMenu();
            //SetMenuLevel(menuItem.SubMenus,0);
            this.appStateService.LoadStateChanged += this.OnStateChanged;

            IDictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
            menus=await _navManger.GetNavAndPinnedMenus();
            NavMenus = menus["nav-menu"]; 
            PinnedMenus = menus["pin-menu"];
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
            
            if (menu!=null)
            {
                string url = menu.GetPathURL();
                if (!string.IsNullOrEmpty(url))
                {
                    await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
                }
                
            }


            this.StateHasChanged();
        }

    }
}
