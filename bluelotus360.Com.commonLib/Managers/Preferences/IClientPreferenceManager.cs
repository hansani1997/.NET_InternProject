using BL10.CleanArchitecture.Domain.Entities.Theme;
using BlueLotus360.Com.Shared.Managers;
using BlueLotus360.Com.Shared.Settings;
using BlueLotus360.Com.Shared.Wrapper;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Preferences
{
	public interface IClientPreferenceManager :IManager
	{
		Task<ClientThemePreference> GetCurrentThemeAsync(ClientThemePreference theme);
        Task<ClientThemePreference> SetCurrentThemeAsync(ClientThemePreference currentTheme);
        Task<ClientThemePreference> UpdateCurrentThemeAsync(ClientThemePreference currentTheme);
        Task<bool> ToggleDarkModeAsync();
        Task SetPreference(IPreference preference);
        Task<IPreference> GetPreference();
        Task<IResult> ChangeLanguageAsync(string languageCode);
        Task SetThemeLocalAsync(ClientThemePreference currentTheme);
        Task<ClientThemePreference> GetThemeLocalAsync();
    }
}
