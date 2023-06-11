using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.DTO.Object
{
    public class CodeBase:CodeBaseResponse
    {
        public string CodeExtraCharacter1 { get; set; } = String.Empty;
        public string CodeExtraCharacter2 { get; set; } = String.Empty;
        public int CodeInt1 { get; set; }
        public int CodeInt2 { get; set; }
        public int CodeInt3 { get; set; }
        public DateTime CodeDate1 { get; set; }
        public DateTime CodeDate2 { get; set; }
        public DateTime CodeDate3 { get; set; }
        public DateTime CodeDate4 { get; set; }

        public bool IsCode10On { get; set; }



        public string GetCodeExtraCharacter1()
        {
            if (string.IsNullOrWhiteSpace(CodeExtraCharacter1))
            {
                return "no-cat-image.png";
            }
            return CodeExtraCharacter1;
        }

        public CodeBase(int CodeKey = 1) : base(CodeKey)
        {

        }
    }
}
