using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using Microsoft.AspNetCore.Components.Forms;
using static MudBlazor.CategoryTypes;

namespace bluelotus360.com.razorComponents.Components.HomeScreen
{
    public partial class PinSelection
    {
        [Parameter] public MenuItem PinnedMenus { get; set; }
        [Parameter] public long ElementKey { get; set; }
        [Parameter] public EventCallback<MenuItem> OnAfterPinnedMenuClick { get; set; }
        List<MenuItem> pincards = new List<MenuItem>();
        MenuItem pinnedrequest = new MenuItem();
        private bool opt;
        private string searchString1 = "";
        private bool IsPinPopupShown;
        int pinnedIndex = 0;
        string[] color_arr = new string[12];

        MenuItem ctx;
        private bool fixed_header = true;
        private MenuItem selectedItem1 = null;
        private HashSet<MenuItem> selectedItems = new HashSet<MenuItem>();
        private IEnumerable<MenuItem> Elements = new List<MenuItem>();
        PopUpLayout overlay=new PopUpLayout();

        private MudTable<MenuItem> _table;
        MenuItem submenu= new MenuItem();


        protected override async Task OnInitializedAsync()
        {
            await LoadMenus();
            
            //return base.OnInitializedAsync();
        }

        private async Task LoadMenus()
        {
            pincards.Clear();
            //await Task.Delay(3000);

            
            if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
            {
                //pincards = submenu.SubMenus.Where(x => x.Ispinned == true).ToList();
                pincards = GetPinnedMenuItems(PinnedMenus.SubMenus);
                this.SetPinCardColor(pincards);
     
            }
            pincards.Add(new MenuItem() { MenuCaption = "Add Shortcuts" });
            
        }
        private async Task RefreshPanel()
        {
            MenuItem pinnedResponse = await _navManger.GetPinnedMenus() ?? new MenuItem();
            pincards = pinnedResponse.SubMenus.Where(x => x.Ispinned == true).ToList();
            pincards.Add(new MenuItem() { MenuCaption = "Add Shortcuts" });

            StateHasChanged();
        }
        private List<MenuItem> GetPinnedMenuItems(IList<MenuItem> menuItems)
        {
            List<MenuItem> pinnedItems = new List<MenuItem>();

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Ispinned)
                {
                    pinnedItems.Add(menuItem);
                }

                if (menuItem.SubMenus != null && menuItem.SubMenus.Count > 0)
                {
                    List<MenuItem> subMenuPinnedItems = GetPinnedMenuItems(menuItem.SubMenus.ToList());
                    pinnedItems.AddRange(subMenuPinnedItems);
                }
            }

            return pinnedItems;
        }
        private void SetPinCardColor(List<MenuItem> pincards)
        {
            color_arr[0] = "#35495F";
            color_arr[1] = "#15A085";
            color_arr[2] = "#F39C13";
            color_arr[3] = "#2A80B9";
            color_arr[4] = "#E74C3D";
            color_arr[5] = "#35495F";
            color_arr[6] = "#15A085";
            color_arr[7] = "#F39C13";
            color_arr[8] = "#2A80B9";
            color_arr[9] = "#E74C3D";
            color_arr[10] = "#35495F";
            color_arr[11] = "#15A085";
        }
        private async void OpenPinpoup()
        {
            overlay.Show();
            submenu = await _navManger.GetPinnedMenus();
            
            StateHasChanged();
        }
        private async Task CloseDialogAsync()
        {
            
            overlay.Hide();
            //IsPinPopupShown = false;
            StateHasChanged();
        }
        async Task ChangeHandlerAsync(bool value, string menucaption)
        {
            if (submenu != null&& submenu.SubMenus!=null && submenu.SubMenus.Count() > 0)
            {
                var item = submenu.SubMenus.Where(x => x.MenuCaption == menucaption).FirstOrDefault();
                if (item != null)
                {
                    item.ChildKey = item.MenuId;
                    item.UserObjectKey = item.UserObjectKey;
                    if (value == true && submenu.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 12)
                    {
                        item.IsChanged = true;
                        item.Ispinned = value;
                    }
                    else if (value == false && submenu.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
                    {
                        item.IsChanged = true;
                        item.Ispinned = value;
                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Can't be added more than 12 shortcuts", Severity.Info);
                    }

                    await UpdatePinSection();

                }

            }

        }
        private async Task UpdatePinSection()
        {
            if (submenu.SubMenus != null && submenu.SubMenus.Count() > 0)
            {
                pinnedrequest.SubMenus = submenu.SubMenus.Where(x => x.IsChanged == true).ToList();

                if (pinnedrequest != null && pinnedrequest.SubMenus != null && pinnedrequest.SubMenus.Count() > 0)
                {
                    if (submenu.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
                    {
                        await _navManger.UpdatePinnedMenus(pinnedrequest);
                    }
                }

                await RefreshPanel();
            }

            this.StateHasChanged();
        }

        private async Task Unpin(string menucaption)
        {
            if (pincards != null && pincards.Count() < 14)
            {
                var item = pincards.Where(x => x.MenuCaption == menucaption).FirstOrDefault();
                MenuItem unpinRequest= new MenuItem();

                if (item!=null)
                {

                    item.IsChanged = true;
                    item.Ispinned = false;
                    item.ChildKey = item.MenuId;
                    item.UserObjectKey = item.UserObjectKey;

                    unpinRequest.SubMenus.Add(item);
                    await _navManger.UpdatePinnedMenus(unpinRequest);
                    await RefreshPanel();
                }
                
                this.StateHasChanged();
            }

            
        }
        

        public async Task NavigateToNewTab(MenuItem pincard)
        {
            if (pincard.ControllerName.Equals("Report"))
            {
                if (OnAfterPinnedMenuClick.HasDelegate)
                {
                    await OnAfterPinnedMenuClick.InvokeAsync(pincard);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(pincard.GetPathURL()))
                {
                    string url = pincard.GetPathURL();
                    //await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
                    _navigationManager.NavigateTo(url);
                }
            }
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
            submenu.SubMenus = await _navManger.SearchBlLiteMenu(req) ?? new List<MenuItem>();
        }
        async Task Dismiss()
        {
            await Task.Yield(); // In case this event triggers other things too (e.g., form edits), let that happen first
            overlay.Hide();
            
        }

        private void PageChanged(int i)
        {
            _table.NavigateTo(i - 1);
        }

        #region hold

        //async Task ChangeHandlerAsync(bool value, string menucaption)
        //{
        //    if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
        //    {
        //        var item = PinnedMenus.SubMenus.Where(x => x.MenuCaption == menucaption).FirstOrDefault();
        //        if (item != null)
        //        {

        //            if (value == true && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 12)
        //            {
        //                item.IsChanged = true;
        //                item.Ispinned = value;
        //                this.StateHasChanged();
        //            }
        //            else if (value == false && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
        //            {
        //                item.IsChanged = true;
        //                item.Ispinned = value;
        //                this.StateHasChanged();
        //            }
        //            else
        //            {
        //                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        //                _snackBar.Add("Can't be added more than 12 shortcuts", Severity.Info);
        //            }

        //            await UpdatePinSection();

        //        }

        //    }

        //}
        //private async Task Unpin(string menucaption)
        //{
        //    if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
        //    {
        //        var item = PinnedMenus.SubMenus.Where(x => x.MenuCaption == menucaption).FirstOrDefault();

        //        item.IsChanged = true;
        //        item.Ispinned = false;
        //        this.StateHasChanged();
        //    }

        //    await UpdatePinSection();
        //}

        //private async Task UpdatePinSection()
        //{
        //    if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
        //    {
        //        pinnedrequest.SubMenus = PinnedMenus.SubMenus.Where(x => x.IsChanged == true).ToList();

        //        if (pinnedrequest != null && pinnedrequest.SubMenus != null && pinnedrequest.SubMenus.Count() > 0)
        //        {
        //            if (PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
        //            {
        //                await _navManger.UpdatePinnedMenus(pinnedrequest);
        //            }
        //        }

        //        await LoadMenus();
        //        //IsPinPopupShown = false;
        //        //overlay.Hide();

        //    }

        //    this.StateHasChanged();
        //}

        //private bool FilterFunc1(MenuItem element) => FilterFunc(element, searchString1);

        //private bool FilterFunc(MenuItem element, string searchString)
        //{
        //    if (string.IsNullOrWhiteSpace(searchString))
        //        return true;
        //    if (element.MenuCaption.Contains(searchString, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    if ($"{element.MenuCaption}".Contains(searchString))
        //        return true;
        //    return false;
        //}
        #endregion
    }
}
