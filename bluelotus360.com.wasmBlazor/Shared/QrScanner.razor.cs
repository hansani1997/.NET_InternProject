using BlazorBarcodeScanner.ZXing.JS;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace bluelotus360.com.wasmBlazor.Shared
{
    public partial class QrScanner
    {
        int _currentVideoSourceIdx = 0;

        private BarcodeReader _reader;

        [Parameter]
        public EventCallback<UIInterectionArgs<string>> OnQRDetected { get; set; }
        //To prevent making JavaScript interop calls during prerendering
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (!firstRender) return;
            //await _jsRuntime.InvokeAsync<string>("zxing.start", true, DotNetObjectReference.Create(this));

        }

        [JSInvokable("invokeFromJS")]
        public void ChangeValue(string val)
        {
            if (OnQRDetected.HasDelegate)
            {
                UIInterectionArgs<string> args = new UIInterectionArgs<string>();
                args.DataObject = val;
                OnQRDetected.InvokeAsync(args);
                InvokeAsync(StateHasChanged);
            }


        }

        private void LocalReceivedBarcodeText(BarcodeReceivedEventArgs barargs)
        {
            if (OnQRDetected.HasDelegate)
            {
                UIInterectionArgs<string> args = new UIInterectionArgs<string>();
                args.DataObject = barargs.BarcodeText;
                OnQRDetected.InvokeAsync(args);
                InvokeAsync(StateHasChanged);
            }
        }

        public void ToggleVideoInputs()
        {
            var inputs = _reader.VideoInputDevices.ToList();

            if (inputs.Count == 0)
            {
                return;
            }

            _currentVideoSourceIdx++;
            if (_currentVideoSourceIdx >= inputs.Count)
            {
                _currentVideoSourceIdx = 0;
            }

            _reader.SelectVideoInput(inputs[_currentVideoSourceIdx]);
        }
    }
}
