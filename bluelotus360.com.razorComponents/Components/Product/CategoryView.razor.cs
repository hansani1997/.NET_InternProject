using bluelotus360.com.razorComponents.StateManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Product
{
    public partial class CategoryView
    {
        [Parameter]
        public EventCallback OnCategoryBackButtonClick { get; set; }

        [Parameter]
        public BLUIElement BLUIElement { get; set; }

        private async Task GoBackToListView()
        {
            _orderState.SelectedCategory= null;
            _orderState.DisplayMode = WindowDisplayMode.CategoryListView;
            if (OnCategoryBackButtonClick.HasDelegate)
            {
              await OnCategoryBackButtonClick.InvokeAsync(); 
            }
            await Task.CompletedTask;
        }
    }



   
}
