using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Account_Profile.Components
{
    public partial class BLAccountProfileHeader
    {
        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public BLUIElement FormObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public string Class { get; set; } = "default-class";

        public BLUIElement LinkedUIObject { get; private set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}
