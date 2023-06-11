using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.StateManagement
{
    public class BLBreakpoint
    {
        public Breakpoint BreakpointMargin { get; set; }  
    }

    //public enum ScreenBreakPoint
    //{
    //    XS,
    //    SM,
    //    MD,
    //    LG,
    //    XLG
    //}
    public class WindowSize
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class BreakPointState
    {
        public Breakpoint GetScreenWidth(double screen_width)
        {
            if (screen_width < 600)
            {
                return Breakpoint.Xs;
            }
            else if (600 <= screen_width && screen_width < 960)
            {
                return Breakpoint.Sm;
            }
            else if (960 <= screen_width && screen_width < 1280)
            {
                return Breakpoint.Md;
            }
            else if (1280 <= screen_width && screen_width < 1920)
            {
                return Breakpoint.Lg;
            }
            
            return Breakpoint.Xl;
            
        }
    }

}
