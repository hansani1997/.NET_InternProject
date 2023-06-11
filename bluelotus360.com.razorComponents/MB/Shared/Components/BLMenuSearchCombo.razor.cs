using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLMenuSearchCombo
    {
        [Parameter]
        public MenuItem ComboDataObject { get; set; }

        [Parameter]
        public EventCallback<MenuItem> OnComboChanged { get; set; }

        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "top-search-combo-wrapper";
        private string combo_css = "";
        private MenuItem selectedMenu = new MenuItem();
        private List<MenuItem> blMenus = new List<MenuItem>();
        protected override async Task OnInitializedAsync()
        {
            selectedMenu = new MenuItem();
            if (ComboDataObject != null && ComboDataObject.SubMenus != null)
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

        private async void OnComboValueChangedTel(MenuItem serchingmenu)
        {
            if (serchingmenu!=null && serchingmenu.MenuId == 0)
            {
                serchingmenu.MenuId=1;
            }
            if (blMenus != null && serchingmenu!=null)
            {
                if (serchingmenu.MenuId > 1)
                {
                    selectedMenu = blMenus.Where(x => x.MenuId == serchingmenu.MenuId).FirstOrDefault();
                }
                else
                {
                    selectedMenu = new MenuItem();
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

        private async Task<IEnumerable<MenuItem>> OnComboSearch(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return blMenus;
            }
            await Task.CompletedTask;

            return blMenus.Where(x => x.MenuCaption.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


    }
}
