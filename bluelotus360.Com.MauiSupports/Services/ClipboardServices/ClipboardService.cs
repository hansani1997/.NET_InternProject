using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.ClipboardServices
{
    public class ClipboardService : IClipboardService
    {
        public async ValueTask<string> ReadTextAsync()
        {
           return await Clipboard.Default.GetTextAsync();
        }

        public async ValueTask WriteTextAsync(string text)
        {
            await Clipboard.Default.SetTextAsync(text);
        }
    }
}
