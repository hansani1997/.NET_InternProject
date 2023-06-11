using bluelotus360.com.razorComponents.ServiceInterfaces;
using bluelotus360.com.wasmBlazor.Shared;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace bluelotus360.com.wasmBlazor.Services
{
    internal class WASMBarcodeService : IBarcodeService
    {
        public readonly IDialogService _dialogService;

        public WASMBarcodeService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<object> ReadBarcode()
        {
            var parameters = new DialogParameters
            {


            };
            DialogOptions options = new DialogOptions();
            var dialog = _dialogService.Show<QrDialog>("QR Scanner", parameters, options);
            DialogResult dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                if (dialogResult.Data != null)
                {
                   return dialogResult.Data as string ??"";
                }
            }

            return "";
        }

    }
}
