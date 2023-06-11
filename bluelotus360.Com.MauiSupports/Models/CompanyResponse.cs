using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class CompanyResponse
    {
        public int CompanyKey { get; set; }
        public string CompanyCode { get; set; } = "";
        public string CompanyName { get; set; }

        public override string ToString()
        {
            return $"{CompanyCode} - {CompanyName}";
        }
    }
}
