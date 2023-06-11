using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities
{
    public class UberMenuItems
    {
        public class PartnerMenuItem
        {

            public string ItemImage { get; set; } //need
            public string ItemImageUrl { get; set; } //need
            public string ItemName { get; set; } //need
            public long ItemKey { get; set; }
            public string ItemCode { get; set; } //need
            public string ItemShortName { get; set; }
            public string CategoryName { get; set; } //need
            public string CategoryCode { get; set; } //need
            public string EAN { get; set; }
            public string Description { get; set; } //need
            public UnitResponse ItemUnit { get; set; }
            public UnitResponse ServiceUnit { get; set; }
            public string Remarks { get; set; }
            public decimal CostPrice { get; set; }
            public decimal SalesPrice { get; set; } //need
            public decimal OptionalSalesPrice { get; set; }
            public bool IsModifierItem { get; set; }
            public bool IsCompositeItem { get; set; }
            public bool IsPayemntType { get; set; }
            public decimal MaximumDiscount { get; set; }
            public decimal VatPercentage { get; set; }
            public bool IsDiscontinued { get; set; } //need
            public bool IsAgeVerification { get; set; } //need
            public bool IsPartnerItem { get; set; } //need
            public bool IsAvailableInPartnerSide { get; set; } //need
            public PartnerMenuItem ParentItem { get; set; }
            public CodeBaseResponse Brand { get; set; }
            public DateTime ExpireDate { get; set; }
            public int ValueForProjectKey { get; set; }
            public int FoodTypeId { get; set; } = 1;
            public string FoodTypeName { get; set; } = "";
            public int UrbanPiperFoodTypeId { get; set; } = 1;
            public bool IsRecommended { get; set; }
            public decimal SupplierWarranty { get; set; }
            public decimal CustomerWarranty { get; set; }
            public string ItemComboTitle
            {
                get
                {
                    return ItemCode + " - " + ItemName;
                }
            }
            public string PartNumber { get; set; }
            //public CodeBase Model { get; set; }
            public bool IsSerialNumber { get; set; }
            public decimal ReOrderLevel { get; set; }
            public decimal ReOrderQuantity { get; set; }
            public int SortingOrder { get; set; }

            public List<ProductGroup> productGroups { get; set; }
            public List<ProductGroup> productGroupsItem { get; set; }
            public PartnerMenuItem()
            {
                productGroups = new List<ProductGroup>();
                productGroupsItem = new List<ProductGroup>();
            }

        }

        public class ProductGroup : BaseEntity
        {
            private int productGroupId;
            private string groupName;
            private int groupNo;
            private IList<ProductGroupDetail> grupedProducts;
            private Company company;
            public int ProductGroupId { get => productGroupId; set => productGroupId = value; }
            public string GroupName { get => groupName; set => groupName = value; }
            public int GroupNo { get => groupNo; set => groupNo = value; }
            public IList<ProductGroupDetail> GrupedProducts { get => grupedProducts; set => grupedProducts = value; }
            public Company Company { get => company; set => company = value; }
            public string ProductItemCode { get; set; }
            public decimal Price { get; set; }
            public int UrbanPiperFoodType { get; set; }
            public decimal MinumumQuantity { get; set; }
            public decimal MaximumQuantity { get; set; }

            public List<ProductGroup> NestedGroups { get; set; }
            public ProductGroup()
            {
                grupedProducts = new List<ProductGroupDetail>();
                NestedGroups = new List<ProductGroup>();
            }
        }


        public class ProductGroupDetail : BaseEntity
        {
            private int groupDetailId;
            private Product product;
            private decimal defaultQuantiy;
            private int gruopHeaderId;
            private decimal price;





            public int GroupDetailId { get => groupDetailId; set => groupDetailId = value; }
            public Product Product { get => product; set => product = value; }
            public decimal DefaultQuantiy { get => defaultQuantiy; set => defaultQuantiy = value; }
            public int GruopHeaderId { get => gruopHeaderId; set => gruopHeaderId = value; }
            public decimal Price { get => price; set => price = value; }


            public ProductGroupDetail()
            {
                Product = new Product();
            }
        }

        public enum MenuUploadProcessStatus
        {
            IsHold = 0,
            IsSuccess = 1,
            IsError = 2
        }

       
    }
}
