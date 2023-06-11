using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ApplicationInformation
{
    public class BlHybridInfo
    {
        public string AppName { get; set; } = "";
        public string Package{ get; set; } = "";
        public string Version { get; set; } = "";
        public string Build { get; set; } = "";
    }
}
