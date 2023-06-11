using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.DTO
{
    public class BaseDTO
    {
        public int CompanyKey { get; set; }

        public int UserKey { get; set; }

        public string? Enviorement { get; set; } = "";

        public long ObjectKey { get; set; } = 1;

        public string? RequestId { get; set; } = "";

        public string? IntegrationId { get; set; } = "";

        public int RequestingUser { get; set; } = 1;
    }
}
