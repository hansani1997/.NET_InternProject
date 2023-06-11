using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile
{
    public class ItemSelectListRequest
    {
        public long ElementKey { get; set; }

        public int ItmTypKy { get; set; } = 1;

        //public int Dept { get; set; }

        //public int Cat { get; set; }

        //public int FrmRow { get; set; }

        //public int ToRow { get; set; }

        //public byte OnlyisAct { get; set; }

        //public string ItmCd { get; set; }

        //public string ItmNm { get; set; }

        public ItemSelectListRequest()
        {
            //Dept = 1;
            //Cat = 1;
            //FrmRow = 0;
            //ToRow = 999999;
            //OnlyisAct = 1;
            //ItmCd = string.Empty;
            //ItmNm = string.Empty;
        }
    }

    public class ItemSelectList
    {
        public int ItmKy { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemCode { get; set; }
        public CodeBaseResponse ItemType { get; set; } = new CodeBaseResponse();
        public UnitResponse ItemUnit { get; set; } = new UnitResponse();
        public ItemResponse ParentItem { get; set; } = new ItemResponse();
        public bool IsActive { get; set; }
        public bool IsApprove { get; set; }

        public ItemSelectList()
        {
            this.ItmKy = 0;
            this.ItemCode = string.Empty;
            this.ItemName = string.Empty;
            this.ItemType = new CodeBaseResponse();
            this.ItemUnit = new UnitResponse();
            this.ParentItem = new ItemResponse();
            this.IsActive = true;
            this.IsApprove = true;
        }
    }

    public class TabRequest
    {
        public int ItmKy { get; private set; }
    }
}
