using BL10.CleanArchitecture.Domain.Entities.ApplicationInformation;
using bluelotus360.com.razorComponents.ServiceInterfaces;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor.Services
{
    public class BLHybridAppInfoService: IBLHybridInfo
    {
        public  BlHybridInfo ReadAppInfo()
        {

            return new BlHybridInfo()
            {
                AppName = AppInfo.Current.Name,
                Package = AppInfo.Current.PackageName,
                Version = AppInfo.Current.VersionString,
                Build = AppInfo.Current.BuildString
            };
        }
    }
}
