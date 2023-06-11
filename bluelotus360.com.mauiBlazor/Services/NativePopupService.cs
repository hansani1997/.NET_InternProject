using bluelotus360.com.mauiBlazor.MauiPages;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.ServiceInterfaces;

namespace bluelotus360.com.mauiBlazor.Services
{
    public class NativePopupService : INativePopupService
    {
        public async Task<object> showYesNoDialog(string txt)
        {
            return await App.Current.MainPage.ShowPopupAsync(new YesNoPopup(txt));
        }
    }
}
