using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Shared.Constants.Storage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bluelotus360.com.razorComponents.Pages
{
    public partial class Index
    {
        [CascadingParameter(Name = "NavMenus")]
        protected MenuItem ChildMenu { get; set; }
        private Transition transition = Transition.Slide;
        private string FirstName { get; set; }
        private string ImageDataUrl { get; set; }
        private char FirstLetterOfName;
        private string CompanyName { get; set; }

        List<MenuItem> tiles = new List<MenuItem>();
        string greetings = "";
        long elementKey = 1;
        private bool showReminderAlert = true;

        protected override async Task OnInitializedAsync()
        {
            //greetings = await _jsRuntime.InvokeAsync<string>("GerGreetings", null);
            //return base.OnInitializedAsync();
            HookInteractions();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }


        }

        private void HookInteractions()
        {
            appStateService._AppBarName = "Home";
        }
        private async Task LoadDataAsync()
        {

			//var state = await ((CustomAuthenticationProvider)_stateProvider).GetAuthenticationStateAsync2();
			var state = await _stateProvider.GetAuthenticationStateAsync();
			ClaimsPrincipal user = new ClaimsPrincipal();
            if (state != null)
            {
                user = state.User;

                ClaimsPrincipal principal = state.User as ClaimsPrincipal;
                if (principal != null)
                {
                    Claim? claim = principal.FindFirst("FirstName");
                    if (claim != null && claim.Value != null)
                    {
                        FirstName = claim.Value.Split(".")[0];
                    }
                }


                if (this.FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }
                var UserId = FirstName;
                //var imageResponse = await _localStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
                var imageResponse = await _storageService.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
                if (!string.IsNullOrEmpty(imageResponse))
                {
                    ImageDataUrl = imageResponse;
                }

                //CompanyName = await _localStorage.GetItemAsync<string>(StorageConstants.Local.CompanyName);
                CompanyName = await _storageService.GetItemAsync<string>(StorageConstants.Local.CompanyName);
                StateHasChanged();
            }
        }
        private void CloseMe(bool value)
        {
            if (value)
            {
                showReminderAlert = false;
            }

        }

    }
}
