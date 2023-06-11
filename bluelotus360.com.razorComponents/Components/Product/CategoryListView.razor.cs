using BL10.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Product
{
    public partial class CategoryListView
    {
        [Parameter]
        public IList<CodeBase> Categories { get; set; }

        [Parameter]
        public EventCallback<CodeBase> OnCategoryClick { get; set; }



        private IList<CodeBase> filterdCategories;
        private string filterQuery = string.Empty;

        public async Task ProcessCategoryClick(CodeBase category)
        {

            if (OnCategoryClick.HasDelegate)
            {
                await OnCategoryClick.InvokeAsync(category);
            }
        }

        protected override async Task OnParametersSetAsync()
        {


            filterdCategories = Categories;
            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }


        public async Task OnFilterQueryChanged(string value)
        {
            filterQuery = value;
            if (filterQuery.Length > 2)
            {
                filterdCategories=Categories.Where(x=>x.CodeName.Contains(filterQuery,StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            else
            {

                filterdCategories = Categories;
            }
            await Task.CompletedTask;
        }

    }
}
