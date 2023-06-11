using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Shared.Models
{
    public class CompanyResponse
    {
        public int CompanyKey { get; set; }
        public string CompanyCode { get; set; } = "";
        public string CompanyName { get; set; } 

        public override string ToString()
        {
            return  $"{CompanyCode} - {CompanyName}";
        }

    }
}
