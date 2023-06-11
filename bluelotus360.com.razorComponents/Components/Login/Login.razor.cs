using ApexCharts;
using BL10.CleanArchitecture.Domain.Entities.Theme;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.ThemeManagerTheme;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Login
{
    public partial class Login : ComponentBase
    {
        [Parameter] public EventCallback<ClientThemePreference> ThemeChanged { get; set; }

		private MudTextField<string> pwdSingleLineReference=new MudTextField<string>();
        private MudTextField<string> usrnmSingleLineReference = new MudTextField<string>();
        private TokenRequest _tokenModel = new();
        private bool IsLoginSuccessFull = false;
        private string Message = "Login with your Credentials.";
        private string className = "";
        protected override async Task OnInitializedAsync()
        {
			var state = await _stateProvider.GetAuthenticationStateAsync();
			if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
			{
                _navigationManager.NavigateTo("/");
                await _jsRuntime.InvokeVoidAsync("SetDomTitle", "Blue Lotus 360");
            }

			
		}


        private async Task SubmitAsync()
        {
            Message = "Login with your Credentials.";
            className = "";
            StateHasChanged();
            if (string.IsNullOrEmpty(_tokenModel.UserName) || string.IsNullOrEmpty(_tokenModel.Password)){
                _snackBar.Add("Username or Password Empty!");
            }
            else {
                try
                {
                    var result = await _authenticationManager.Login(_tokenModel);

                    if (result.IsSuccess)
                    {
                        bool isKeyExists = await _localStorage.ContainKeyAsync("Theme");
                        if (isKeyExists)
                        {
                            await _authenticationManager.GetUserInfoOffline();
                            //_localStorage.RemoveItemsAsync("Theme");
                            await _localStorage.RemoveItem("Theme");

                            ClientThemePreference req = new ClientThemePreference() { IsDefault = 1, ThemeType = -1 };
                            ClientThemePreference pref = await _preferenceManager.GetCurrentThemeAsync(req);

                            if (pref != null) { await ThemeChanged.InvokeAsync(pref); }

                        }
                    }
                    else
                    {
                        _snackBar.Add("Username or Password Incorrect!");
                    }
                }
                catch(Exception ex)
                {
                    _snackBar.Add("Internet Connection may dropped!");
                }
            }
            StateHasChanged();
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;

            }
        }

        private async void OnUsernameInputKeyDown(KeyboardEventArgs e)
        {
            if (e!=null && e.Code!=null)
            {
                if (e.Code.Equals("Enter") || e.Code.Equals("NumpadEnter"))
                {
                    if (!string.IsNullOrEmpty(_tokenModel.Password))
                    {
                        await SubmitAsync();
                    }
                    else
                    {
                        await pwdSingleLineReference.FocusAsync();
                    }

                }
            }
        }

        private async void OnPasswordInputKeyDown(KeyboardEventArgs e)
        {
            if (e != null && e.Code != null)
            {
                if (e.Code.Equals("Enter") || e.Key.Equals("Enter") || e.Code.Equals("NumpadEnter") || e.Key.Equals("NumpadEnter"))
                {
                    if (!string.IsNullOrEmpty(_tokenModel.UserName))
                    {
                        await SubmitAsync();
                    }
                    else
                    {
                        await usrnmSingleLineReference.FocusAsync();
                    }

                }
            }
                


        }

		
	}
}
