using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.WorkShopManagement
{
    public class InsuranceModuleValidator : IInsuranceModuleValidator
    {
        private WorkOrder _insurenceOrder;
        public UserMessageManager UserMessages { get; set; }
        public InsuranceModuleValidator(WorkOrder InsurenceOrder)
        {
            this._insurenceOrder = InsurenceOrder;
            UserMessages = new UserMessageManager();
        }
        public bool CanAddToGridItem()
        {
            throw new NotImplementedException();
        }

        public bool CanCreateIRNOrder()
        {
            UserMessages.UserMessages.Clear();

            //if (_insurenceOrder.Insurance != null && _insurenceOrder.Insurance.ItemKey < 10)
            //{
            //    UserMessages.AddErrorMessage("Please Select Insurance !");
            //}
            if (_insurenceOrder.OrderCategory2 != null && _insurenceOrder.OrderCategory2.CodeKey < 10)
            {
                UserMessages.AddErrorMessage("Please Select IRN Type !");
            }
            //if (_insurenceOrder.MeterReading > 0)
            //{
            //    UserMessages.AddErrorMessage($"Current Milage can't be less than 0!");
            //}

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSaveWorkOrder()
        {
            throw new NotImplementedException();
        }
    }
}
