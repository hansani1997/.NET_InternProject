using bluelotus360.com.mauiBlazor.MauiPages;
using bluelotus360.com.razorComponents.ServiceInterfaces;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor.Services
{
    internal class BarcodeService : IBarcodeService
    {
        public async Task<object> ReadBarcode()
        {
            return await App.Current.MainPage.ShowPopupAsync(new BarcodePopup());
        }
    }
}
