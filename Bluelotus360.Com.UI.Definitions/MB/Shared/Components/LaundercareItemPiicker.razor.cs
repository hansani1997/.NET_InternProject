using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class LaundercareItemPiicker
    {
        [Parameter]
        public IList<CodeBaseResponse> Services { get; set; }

        [Parameter]
        public IList<CodeBaseResponse> HumanTypes { get; set; }

        [Parameter]
        public IList<ItemResponse> Items { get; set; }

        [Parameter]
        public CodeBaseResponse ItemCategory2 { get; set; }

        [Parameter]
        public CodeBaseResponse ItemCategory1 { get; set; }

        [Parameter]
        public ItemResponse SelectedItem { get; set; }


        [Parameter]
        public EventCallback<UIInterectionArgs<CodeBaseResponse>> OnServiceTypeChanged { get; set; }


        [Parameter]
        public EventCallback<UIInterectionArgs<CodeBaseResponse>> OnHumanTypeChanged { get; set; }


        [Parameter]
        public EventCallback<UIInterectionArgs<ItemResponse>> OnSelectedItemChanged { get; set; }

        private IList<List<ItemResponse>> _items;

        string className = "";

        private MudCarousel<object> _refSlider;

        private bool IsImagesLoaded = false;
        private bool IsDisable=false;
        public LaundercareItemPiicker()
        {
            Services = new List<CodeBaseResponse>();
            HumanTypes = new List<CodeBaseResponse>();
            Items = new List<ItemResponse>();
        }


        protected override async Task OnParametersSetAsync()
        {

            _items = Items.ChunkBy<ItemResponse>(11);

            if (_refSlider != null)
            {
                _refSlider.MoveTo(0);
            }
           await base.OnParametersSetAsync();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        public void DisableServiceTypes(CodeBaseResponse codeBaseResponse)
        {
            if (Convert.ToBoolean(codeBaseResponse.AddtionalData["IsKiloWash"]))
            {
                foreach (CodeBaseResponse c in Services)
                {
                    if (Convert.ToBoolean(c.AddtionalData["IsKiloWash"])|| Convert.ToBoolean(c.AddtionalData["IsCommon"]))
                    {
                        c.AddtionalData["IsDisabled"] = false;
                    }
                    else
                    {
                        c.AddtionalData["IsDisabled"] = true;
                    }
                }

            }
            else if (codeBaseResponse.CodeKey==1)
            {
                if (Services != null && Services.Count() > 0) { Services.ToList().ForEach(x => x.AddtionalData["IsDisabled"] = false); }
            }
            else
            {
                if (!Convert.ToBoolean(codeBaseResponse.AddtionalData["IsCommon"]))
                {
                    if (Services != null)
                    {
                        CodeBaseResponse c = Services.Where(x => Convert.ToBoolean(x.AddtionalData["IsKiloWash"])==true).FirstOrDefault();
                        if (c != null)
                        {
                            c.AddtionalData["IsDisabled"] = true;
                        }
                    }

                }


            }
        }

        public void SelectService(CodeBaseResponse codeBaseResponse)
        {
            ItemCategory2 = codeBaseResponse;

            DisableServiceTypes(codeBaseResponse);

            if (OnServiceTypeChanged.HasDelegate)
            {
                UIInterectionArgs<CodeBaseResponse> args = new UIInterectionArgs<CodeBaseResponse>();
                args.DataObject = ItemCategory2;
                OnServiceTypeChanged.InvokeAsync(args);
            }
            StateHasChanged();
        }

        public void SelectHumanType(CodeBaseResponse codeBaseResponse)
        {
            ItemCategory1 = codeBaseResponse;
            if (OnHumanTypeChanged.HasDelegate)
            {
                UIInterectionArgs<CodeBaseResponse> args = new UIInterectionArgs<CodeBaseResponse>();
                args.DataObject = ItemCategory1;
                OnHumanTypeChanged.InvokeAsync(args);
            }
            StateHasChanged();
        }

        public void SelectItem(ItemResponse item)
        {
            SelectedItem = item;
            if (OnSelectedItemChanged.HasDelegate)
            {
                UIInterectionArgs<ItemResponse> args = new UIInterectionArgs<ItemResponse>();
                args.DataObject = item;
                OnSelectedItemChanged.InvokeAsync(args);
            }
            StateHasChanged();
        }

        public async Task RequestItemImages()
        {
            if(BaseResponse.IsValidData(ItemCategory1) && BaseResponse.IsValidData(ItemCategory1))
            {
                if (Items != null)
                {
                   
                    StateHasChanged();
                }
                
            }
        }


        public string GetClassName(ItemResponse response)
        {
            if (response == null)
            {
                return "mt-4";

            }

            if (SelectedItem == null)
            {
                return "mt-4";
            }

            if (SelectedItem.ItemKey == response.ItemKey)
            {
                return "mt-4 bg-primary";

            }


            return "mt-4";
        }
    }
}
