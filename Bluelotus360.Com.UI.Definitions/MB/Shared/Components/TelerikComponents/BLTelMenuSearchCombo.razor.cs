using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelMenuSearchCombo 
    {

        [Parameter]
        public MenuItem ComboDataObject { get; set; }

        [Parameter]
        public EventCallback<MenuItem> OnComboChanged { get; set; }

        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "";
        private string combo_css = "";
        private MenuItem selectedMenu = new MenuItem();
        private List<MenuItem> blMenus=new List<MenuItem>();
        protected override async Task OnInitializedAsync()
        {
            selectedMenu = new MenuItem();
            if (ComboDataObject!=null && ComboDataObject.SubMenus!=null)
            {
                SetChildMenus(ComboDataObject);
            }
            
            await base.OnInitializedAsync();
        }

        private void SetChildMenus(MenuItem childmenu)
        {
            foreach (var menu in childmenu.SubMenus)
            {
                if ((menu.SubMenus != null && menu.SubMenus.Count == 0) && menu.ParentId > 0)
                {
                    blMenus.Add(menu);
                }
                else
                {
                    SetChildMenus(menu);
                }
            }
        }

        private async void OnComboValueChangedTel(int key)
        {
            if (key == 0)
            {
                key = 1;
            }
            if (blMenus != null)
            {
                if (key>1)
                {
                    selectedMenu = blMenus.Where(x => x.MenuId == key).FirstOrDefault();
                }
                else
                {
                    selectedMenu=new MenuItem();
                }
                
            }
            if (selectedMenu != null)
            {
                OnComboValueChanged(selectedMenu);
            }
            
        }

        private void OnComboValueChanged(MenuItem MenuResponse)
        {
            try
            {
                OnComboChanged.InvokeAsync(MenuResponse);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        public void ResetToInitialValue()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new NotImplementedException();
        }

        public Task Refresh()
        {
            throw new NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new NotImplementedException();
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public Task FetchData(bool useLocalstorage = false)
        {
            throw new NotImplementedException();
        }

        public Task SetDataSource(object DataSource)
        {
            throw new NotImplementedException();
        }
    }
}
