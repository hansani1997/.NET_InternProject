using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.MasterData
{
    public class AddressMasterValidator : IAddressMasterValidator
    {
        private AddressMaster addressMaster;
        public UserMessageManager ValidationMessages { get; set; }



        public AddressMasterValidator(AddressMaster addressMaster)
        {
            this.addressMaster = addressMaster;
            ValidationMessages = new UserMessageManager();
        }

        public bool IsValidAddress()
        {
            ValidationMessages.UserMessages.Clear();

            if (addressMaster == null)
            {
                ValidationMessages.AddErrorMessage("Invalid Data");

            }

            else
            {
                if (string.IsNullOrEmpty(addressMaster.AddressID))
                {
                    ValidationMessages.AddErrorMessage("ID cannot be empty");
                }
                if (string.IsNullOrEmpty(addressMaster.AddressName))
                {
                    ValidationMessages.AddErrorMessage("Name cannot be empty");
                }
                //if (string.IsNullOrEmpty(addressMaster.Email))
                //{
                //    ValidationMessages.AddErrorMessage(" Email cannot be empty");
                //}
                //if (string.IsNullOrEmpty(addressMaster.NIC))
                //{
                //    ValidationMessages.AddErrorMessage(" NIC cannot be empty");
                //}
                //if (string.IsNullOrEmpty(addressMaster.City))
                //{
                //    ValidationMessages.AddErrorMessage("City cannot be empty");
                //}
                //if (string.IsNullOrEmpty(addressMaster.Street))
                //{
                //    ValidationMessages.AddErrorMessage("Street cannot be empty");
                //}

                if (ValidationMessages.IsValidForm())
                {
                  

                }

            }

            return ValidationMessages.IsValidForm();

        }
    }
}
