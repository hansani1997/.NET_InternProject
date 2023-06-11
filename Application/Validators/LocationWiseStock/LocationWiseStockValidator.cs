using BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStockValidator;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStock
{
   public class LocationWiseStockValidator:ILocationWiseStockValidator
    {
        private LocationViseStockRequest _location_wise_stock;
        public UserMessageManager UserMessages { get; set; }

        public LocationWiseStockValidator(LocationViseStockRequest location_wise_stock)
        {
            this._location_wise_stock = location_wise_stock;
            UserMessages = new UserMessageManager();
        }
        public bool CanLoadChart()
        {
            UserMessages.UserMessages.Clear();

            if (_location_wise_stock.AsAtDate == null)
            {
                UserMessages.AddErrorMessage("Transaction Date can not be null ");
            }

            if (_location_wise_stock.Item ==null || _location_wise_stock.Item.ItemKey==1)
            {
                UserMessages.AddErrorMessage("Please Enter Any Item");
            }

            return UserMessages.UserMessages.Count == 0;
        }
    }
}
