using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudBlazor;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;

namespace bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent
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
        string dummyImage = "data:image/jpeg;base64,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";

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
