using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.Sections;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.ThemeManagerTheme;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.commonLib.Setting;
using BL10.CleanArchitecture.Domain.Entities.Theme;

namespace bluelotus360.com.razorComponents.MB.Settings
{
    public partial class BLThemeManager
    {
        private ThemeManagerTheme _themeManagerTheme = new();

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public int Elevation { set; get; } = 1;

        [Parameter]
        public Anchor Anchor { get; set; } = Anchor.End;

        [Parameter]
        public bool DisableOverlay { get; set; } = true;

        [Parameter]
        public MudTheme Theme { get; set; } = new();

        [Parameter]
        public EventCallback<MudTheme> ThemeChanged { get; set; }

        [Parameter]
        public ThemeManagerConfiguration Configuration { get; set; } = new();

        //public ThemeManagerTheme ThemePalette { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            if (Configuration!=null)
            {
                _themeManagerTheme.PresetThemes = Configuration.DefaultPresetThemeSelected;
                _themeManagerTheme.Mode = Configuration.DefaultMode;
                _themeManagerTheme.Palette.SetThemeManagerThemePalette(Theme.Palette);
                _themeManagerTheme.LayoutProperties = Theme.LayoutProperties;

                // TODO: Maybe move that somewhere else?

                if (int.TryParse(_themeManagerTheme.LayoutProperties.DefaultBorderRadius.Replace("px", ""),
                    out var defaultBorderRadiusAsInt))
                    _themeManagerTheme.DefaultBorderRadiusAsInt = defaultBorderRadiusAsInt;

                await UpdateTheme();
            }
            
        }

        private async Task OnPresetThemeChanged(string presetTheme)
        {
            _themeManagerTheme.PresetThemes = presetTheme switch
            {
                PresetThemes.Custom => PresetThemes.Custom,
                PresetThemes.MuiDark => PresetThemes.MuiDark,
                _ => PresetThemes.Custom
            };

            await UpdateTheme();
        }

        private async Task ToggleMode(Modes mode)
        {
            _themeManagerTheme.Mode = mode;
            ClientThemePreference preference =new ClientThemePreference();
            Palette plt = new Palette();

            if (await _localStorage.ContainKeyAsync("Theme"))
            {
                await _localStorage.RemoveItem("Theme");
                preference =await _preferenceManager.GetCurrentThemeAsync(new ClientThemePreference() { IsDefault = -1, ThemeType = (int)(_themeManagerTheme.Mode) });

            }

            if (_themeManagerTheme.Mode == Modes.Dark)
            {
                
                if (preference != null && preference.IsthemePersisted())
                {
                    plt = new Palette()
                    {
                        Primary = preference.PrimaryColor,
                        Secondary = preference.SecondaryColor,
                        AppbarBackground = "#161622",
                        Background = "#161622",
                    };
                }
                else
                {
                    plt = PresetThemes.GetDefaultDarkPalette();
                }
				
				_themeManagerTheme.Palette.Primary = plt.Primary.Value;
				_themeManagerTheme.Palette.Secondary = plt.Secondary.Value;
                _themeManagerTheme.Palette.AppbarBackground= plt.AppbarBackground.Value;
                _themeManagerTheme.Palette.Background= plt.Background.Value;
                _themeManagerTheme.LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "12px"};

			}
            else
            {
                if (preference!=null && preference.IsthemePersisted())
                {
					_themeManagerTheme.Palette.Primary = preference.PrimaryColor;
					_themeManagerTheme.Palette.Secondary = preference.SecondaryColor;
					_themeManagerTheme.Palette.AppbarBackground = "#ffffff";
					_themeManagerTheme.Palette.Background = "#ffffff";
					_themeManagerTheme.LayoutProperties = new LayoutProperties() { DefaultBorderRadius = preference.BorderRadius };
				}
                else
                {
                    plt = PresetThemes.GetDefaultLightPalette();

                    _themeManagerTheme.Palette.Primary = plt.Primary.Value;
                    _themeManagerTheme.Palette.Secondary = plt.Secondary.Value;
                    _themeManagerTheme.Palette.AppbarBackground = "#ffffff";
                    _themeManagerTheme.Palette.Background = "#ffffff";
                    _themeManagerTheme.LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "20px" };
                }

            }

			await UpdateTheme();

            if (preference != null)
            {
                preference.IsDefault = 1;
                preference.ThemeType = (int)(_themeManagerTheme.Mode);
                if (preference.IsthemePersisted())
                {
                    await _preferenceManager.UpdateCurrentThemeAsync(preference);
                }
                else
                {
                    await _preferenceManager.SetCurrentThemeAsync(preference);
                }
            }
        }

        private async Task UpdateColor(KeyValuePair<ColorTitles, ColorSectionOptions> colorSection)
        {
            switch (colorSection.Key)
            {
                case ColorTitles.Primary:
                    _themeManagerTheme.Palette.Primary = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Secondary:
                    _themeManagerTheme.Palette.Secondary = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Tertiary:
                    _themeManagerTheme.Palette.Tertiary = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Success:
                    _themeManagerTheme.Palette.Success = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Info:
                    _themeManagerTheme.Palette.Info = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Warning:
                    _themeManagerTheme.Palette.Warning = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Error:
                    _themeManagerTheme.Palette.Error = colorSection.Value.SelectedColor.Value;
                    break;
                case ColorTitles.Dark:
                    _themeManagerTheme.Palette.Dark = colorSection.Value.SelectedColor.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await UpdateTheme();
        }

        private async Task UpdateBorderRadius(int value)
        {
            _themeManagerTheme.DefaultBorderRadiusAsInt = value;
            _themeManagerTheme.LayoutProperties.DefaultBorderRadius = $"{value}px";
            await UpdateTheme();
        }

        private async Task UpdateTheme()
        {
            switch (_themeManagerTheme.PresetThemes)
            {
                case PresetThemes.Custom:
                    var palette = _themeManagerTheme.Mode == Modes.Dark
                        ? Configuration.DarkPalette
                        : Configuration.LightPalette;

                    palette.Primary = _themeManagerTheme.Palette.Primary;
                    palette.Secondary = _themeManagerTheme.Palette.Secondary;
                    palette.Info = _themeManagerTheme.Palette.Info;
                    palette.Success = _themeManagerTheme.Palette.Success;
                    palette.Warning = _themeManagerTheme.Palette.Warning;
                    palette.Error = _themeManagerTheme.Palette.Error;
                    palette.Dark = _themeManagerTheme.Palette.Dark;
                    palette.AppbarBackground = _themeManagerTheme.Palette.AppbarBackground;
                    palette.Background= _themeManagerTheme.Palette.Background;  

                    _themeManagerTheme.Palette.SetThemeManagerThemePalette(palette);
                    Theme.Palette = _themeManagerTheme.Palette.GetThemeManagerThemePalette();
                    Theme.LayoutProperties = _themeManagerTheme.LayoutProperties;

                    break;
                case PresetThemes.MuiDark:
                    Theme = PresetThemes.GetMuiDarkTheme();
                    break;
            }

            await ThemeChanged.InvokeAsync(Theme);
        }

        private async Task ResetTheme()
        {
            if (await _localStorage.ContainKeyAsync("Theme"))
            {
                ClientThemePreference preference = await _preferenceManager.GetCurrentThemeAsync(new ClientThemePreference() { IsDefault = 1 });
                long themeky = preference != null ? preference.ThemeKey : 1;
                await _localStorage.RemoveItem("Theme");

                _themeManagerTheme.Palette.SetThemeManagerThemePalette(BL10LookAndFeel.DefaultTheme.Palette);
                _themeManagerTheme.LayoutProperties = BL10LookAndFeel.DefaultTheme.LayoutProperties;

                if (int.TryParse(BL10LookAndFeel.DefaultTheme.LayoutProperties.DefaultBorderRadius.Replace("px", ""),
                    out var defaultBorderRadiusAsInt))
                    _themeManagerTheme.DefaultBorderRadiusAsInt = defaultBorderRadiusAsInt;

                await UpdateTheme();

                Theme.Palette = _themeManagerTheme.Palette.GetThemeManagerThemePalette();
                Theme.LayoutProperties = _themeManagerTheme.LayoutProperties;

                preference = MapClientPreference(Theme, themeky);

                if (preference != null)
                {
                    if (preference.IsthemePersisted())
                    {
                        await _preferenceManager.UpdateCurrentThemeAsync(preference);
                    }
                    else
                    {
                        await _preferenceManager.SetCurrentThemeAsync(preference);
                    }
                }
            }
            
           
        }

        private async Task SaveTheme()
        {
            appStateService.IsLoaded= true;            
            ClientThemePreference preference = await _preferenceManager.GetCurrentThemeAsync(new ClientThemePreference() {IsDefault=1 });
            if (_themeManagerTheme!=null)
            {
                Theme.Palette = _themeManagerTheme.Palette.GetThemeManagerThemePalette();
                Theme.LayoutProperties = _themeManagerTheme.LayoutProperties;

                preference=MapClientPreference(Theme, preference.ThemeKey);
                
                if (preference != null)
                {
                    if (preference.IsthemePersisted())
                    {
                        await _preferenceManager.UpdateCurrentThemeAsync(preference);
                    }
                    else
                    {
                        await _preferenceManager.SetCurrentThemeAsync(preference);
                    }
                }
            }

            appStateService.IsLoaded = false;
            StateHasChanged();

        }

        public ClientThemePreference MapClientPreference(MudTheme theme,long themeKy)
        {
            return new ClientThemePreference()
            {
                ThemeKey = themeKy,
                PrimaryColor = theme.Palette.Primary.Value,
                SecondaryColor = theme.Palette.Secondary.Value,
                BorderRadius = theme.LayoutProperties.DefaultBorderRadius,
                IsDefault = 1,
            };
        }
    }
}
