using ApexCharts;
using BL10.CleanArchitecture.Domain.Entities.Theme;
using bluelotus360.com.razorComponents.MB.Settings;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.ThemeManagerTheme;
using bluelotus360.Com.commonLib.Setting;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using bluelotus360.com.mauiBlazor.Data;

namespace bluelotus360.com.razorComponents.Shared
{
    public partial class BLMainLayout : IDisposable
    {
        private MudTheme _currentTheme;
        private bool _rightToLeft = false;
        private bool _themeManagerOpen = false;
        private bool IsLoginSuccess = false;
		private bool _isDarkMode;
        bool isThemeChanged;
        ClientThemePreference? _theme = new ClientThemePreference();
        private Dictionary<string, ThemeManagerConfiguration> _themeManagerConfigurations = new();
        private KeyValuePair<string, ThemeManagerConfiguration> _themeManagerConfiguration = new();

		private async Task RightToLeftToggle(bool value)
        {
            _rightToLeft = value;
            await Task.CompletedTask;
        }
        private void ThemeToggle(bool value)
        {
            _isDarkMode = value;
        }
        protected override async Task OnInitializedAsync()
        {
            _currentTheme = new();

            _theme = await _preferenceManager.GetCurrentThemeAsync(new ClientThemePreference() { IsDefault=1});

			_currentTheme = _theme!=null ? BL10LookAndFeel.GetBlThemePreset(_theme) : BL10LookAndFeel.DefaultTheme;
            _themeManagerConfigurations = new()
		    {
			    {"Preset One", ThemeManagerPresetConfigurations.GetPresetConfigOne(_currentTheme)},
			    {"Preset Two", ThemeManagerPresetConfigurations.GetPresetConfigTwo()}
		    };

			_themeManagerConfiguration = _themeManagerConfigurations.First();
			isThemeChanged = true;
            await base.OnInitializedAsync();

        }

        void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        void UpdateTheme(MudTheme value)
        {
            _currentTheme = value;
            StateHasChanged();
        }
        private void ThemeManagerConfigChanged(string key)
		{
			_themeManagerConfiguration = _themeManagerConfigurations.FirstOrDefault(x => x.Key == key);
		}
		public void Dispose()
        {
            //throw new NotImplementedException();
        }

		void UpdateThemeBasedOnUser(ClientThemePreference value)
		{
			_currentTheme = _theme != null ? BL10LookAndFeel.GetBlThemePreset(value) : BL10LookAndFeel.DefaultTheme;
			_themeManagerConfigurations = new()
			{
				{"Preset One", ThemeManagerPresetConfigurations.GetPresetConfigOne(_currentTheme)},
				{"Preset Two", ThemeManagerPresetConfigurations.GetPresetConfigTwo()}
			};

			_themeManagerConfiguration = _themeManagerConfigurations.First();
			isThemeChanged = true;
			StateHasChanged();
		}
	}
}
