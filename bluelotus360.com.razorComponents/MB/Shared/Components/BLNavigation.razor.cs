using BlueLotus.Com.Domain.Entity;
using bluelotus360.com.razorComponents.Pages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Item_Profile_Mobile_V3;
using bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent;
using BlueLotus.Com.Domain.Entity;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLNavigation
    {
        [Parameter]
        public EventCallback<int> Itemdatacallback { get; set; }

        private int[] ItemKeys = { 1530452, 1530451, 1530450, 1530449, 1530448 };
        private int selecteditem;

        protected override async Task OnInitializedAsync()
        {
          
        }

        public async void OnPaginationClick(int index)
        {
            selecteditem = index;
            int selectedValue = ItemKeys[selecteditem];

            await Itemdatacallback.InvokeAsync(selectedValue);

        }
    }
}



