using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.DTO.RequestDTO
{
    public class InventoryDTO
    {
    }

    public class InventoryFindDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public CodeBaseResponse BusinessUnit { get; set; }
        public int ObjectKey { get; set; } = 1;

        public InventoryFindDTO()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            BusinessUnit = new CodeBaseResponse();
        }
    }

    public class InVentoryDTFind : InventoryFindDTO
    {
        public StockValuation Brand { get; set; }

        public InVentoryDTFind()
        {
            Brand= new StockValuation();    
        }
    }
}
