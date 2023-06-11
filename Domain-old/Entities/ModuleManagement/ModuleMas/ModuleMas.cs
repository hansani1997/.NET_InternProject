using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ModuleManagement.ModuleMas
{
    public class ModuleMas : BaseEntity
    {
        public int ModuleKey { get; set; }
        public int EditionKey { get; set; } = 1;
        public int ParentKey { get; set; } = 1;
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
    }
}
