using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.MasterData
{
    public class CustomerValidator : ICustomerValidator
    {
        private AddressMaster newCustomer;
        public UserMessageManager ValidationMessages { get; set; }
        public CustomerValidator(AddressMaster newCustomer) 
        {
            this.newCustomer = newCustomer;
            ValidationMessages = new UserMessageManager();
        }
        public bool IsValidCustomer()
        {
            ValidationMessages.UserMessages.Clear();

            if (newCustomer == null)
            {
                ValidationMessages.AddErrorMessage("Invalid Data");
            }
            else 
            {
                if (string.IsNullOrEmpty(newCustomer.Address))
                {
                    ValidationMessages.AddErrorMessage("Customer Name cannot be empty");
                }
                if (string.IsNullOrEmpty(newCustomer.NIC))
                {
                    ValidationMessages.AddErrorMessage("NIC cannot be empty");
                }
            }

            return ValidationMessages.IsValidForm();
        }
    }
}
