using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile
{
    public class ItemSelectListRequestV3
    {
        public long ElementKey { get; set; } = 1;

        public int ItemTypeKey { get; set; } = 1;

        public int ObjectKey { get; set; } = 1;

        public int Dept { get; set; } = 1;

        public int Category { get; set; } = 1;

        public int FrmRow { get; set; } = 1;

        public int ToRow { get; set; } = 999999;

        public byte OnlyIsActive { get; set; } = 1;

        public string? ItemCode { get; set; } = "";

        public string? ItemName { get; set; } = "";

        public ItemSelectListRequestV3()
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

    public class ItemSelectListV3
    {
        public int ItmKy { get; set; } = 1;

        [Required]
        public string? ItemCode { get; set; } = "";
        [Required]
        public string? ItemName { get; set; } = "";
        public UnitResponse ItemUnit { get; set; } = new UnitResponse();  
        public CodeBaseResponse ItemType { get; set; } = new CodeBaseResponse();
        public ItemResponse ParentItem { get; set; } = new ItemResponse();
        public int PartNumber { get; set; } 
        public string? Description { get; set; } = "";
        public CodeBaseResponse ItemCategory1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Label { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Model { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Brand { get; set; } = new CodeBaseResponse();
        public string? OldItemCode { get; set; } = "";
        public string? SupplierWarranty { get; set; } = "";
        public CodeBaseResponse DefaultSupplier { get; set; } = new CodeBaseResponse();
        public bool IsActive { get; set; } = true;
        public bool IsApprove { get; set; } = true;
        public bool IsDelete { get; set; } = true;

        public ItemSelectListV3()
        {
            this.ItemUnit = new UnitResponse();
            this.ItemType = new CodeBaseResponse();
            this.ParentItem = new ItemResponse();
            this.ItemCategory1 = new CodeBaseResponse();
            this.ItemCategory2 = new CodeBaseResponse();
            this.ItemCategory3 = new CodeBaseResponse();
            this.ItemCategory4 = new CodeBaseResponse();
            this.Label = new CodeBaseResponse();
            this.Model = new CodeBaseResponse();
            this.Brand = new CodeBaseResponse();
            this.DefaultSupplier = new CodeBaseResponse();
        }
    }

    public class TabRequestV3
    {
        public int ItmKy { get; private set; }
    }
}
