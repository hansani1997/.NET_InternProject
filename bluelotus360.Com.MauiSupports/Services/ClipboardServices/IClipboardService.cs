using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.ClipboardServices
{
    public interface IClipboardService
    {
        public ValueTask<string> ReadTextAsync();
        public ValueTask WriteTextAsync(string text);
    }
}
