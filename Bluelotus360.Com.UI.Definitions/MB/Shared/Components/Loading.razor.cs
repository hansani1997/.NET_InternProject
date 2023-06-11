using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public  partial class Loading
    {

        protected override void OnInitialized()
        => this.appStateService.LoadStateChanged += this.OnStateChanged;

        private void OnStateChanged()
            => this.InvokeAsync(StateHasChanged);

        public void Dispose()
            => this.appStateService.LoadStateChanged -= this.OnStateChanged;
    }
}
