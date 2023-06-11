using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Demo
{
    public class TelerikSimpleFormText
    {
        public class SimpleTelerikFormRequest
        {
            public long ElementKey { get; set; }

            public string Name { get; set; }

            public CodeBaseResponse Code {get; set;}

            public string Description { get; set; }

            public DateTime? Date { get; set; }

            public decimal Discount { get; set; }

            public SimpleTelerikFormRequest()
            { 
                Name = string.Empty;
                Code = new CodeBaseResponse();
                Description = string.Empty;
                Date = DateTime.Now;
                Discount = 0;
            }

        }
    }
}
