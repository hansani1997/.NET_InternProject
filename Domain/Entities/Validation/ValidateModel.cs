using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Validation
{
    public class ValidateModel
    {
        public int CdKy { get; set; } = 1;
        public string? Message { get; set; } = "";
        public bool HasError { get; set; }
        public int AprRsnKy { get; set; } = 1;
        public bool isLock { get; set; } = false;
    }
}
