using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.HumanResource.Profile
{
    public partial class EmpProfile
    {
        [Parameter]
        public EmployeeModel Employee { get; set; } = new EmployeeModel();

        [Parameter]
        public string ImageDataUrl { get; set; } = "";

        [Parameter]
        public char FirstLetterOfName { get; set; }

        [Parameter]
        public string FirstName { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
    }
}
