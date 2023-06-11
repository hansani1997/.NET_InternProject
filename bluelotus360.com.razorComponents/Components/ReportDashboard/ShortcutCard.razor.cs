using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.ReportDashboard
{
    public partial class ShortcutCard
    {
        [Parameter]
        public string CardName { get; set; }

        [Parameter]
        public string CardIcon { get; set; }

        [Parameter]
        public EventCallback OnDelete { get; set; }

        private async Task Delete()
        {
            await OnDelete.InvokeAsync();
        }


    }
}
