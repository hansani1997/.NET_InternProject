using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.DTO.MasterData
{
    public class AddressMaster : AddressResponse
    {
        public string Email { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string Country { get; set; } = "";
        public string NIC { get; set; } = "";
        public CodeBaseResponse AddressPrefix { get; set; }
        public decimal VAT { get; set; }
        public decimal SVAT { get; set; }
        public string Address { get; set; } = "";
        public AddressResponse ExistingCustomer { get; set; } = new AddressResponse();

        //Vehicle Details
        public AddressResponse ExistingVehicle { get; set; } = new AddressResponse();
        public long ElementKey { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public CodeBaseResponse Province { get; set; } = new CodeBaseResponse();
        public string RegistraionNumber { get; set; } = "";
        public string ChassiNumber { get; set; } = "";
        public CodeBaseResponse Make { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Model { get; set; } = new CodeBaseResponse();
        public string MakeYear { get; set; } = "";
        public CodeBaseResponse Category { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse SubCategory { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse MaintainPackage { get; set; } = new CodeBaseResponse();
        public string Message { get; set; } = "";
        public bool HasError { get; set; }
        public string Mobile { get; set; } = "";
        public string PostalAddress { get; set; } = "";
        public int IsActive { get; set; }

        public AddressMaster()
        {
            AddressPrefix = new CodeBaseResponse();
            Province = new CodeBaseResponse();
            Make = new CodeBaseResponse();
            Model = new CodeBaseResponse();
            Category = new CodeBaseResponse();
            SubCategory = new CodeBaseResponse();
            MaintainPackage = new CodeBaseResponse();
            HasError = false;
            ExistingCustomer = new AddressResponse();
            ExistingVehicle = new AddressResponse();    
        }

    }

    public class AddAdvAnl:AddressResponse
    {
        public CodeBaseResponse AdvAnlTyp { get; set; }//codebaseresponse??
        public string Email { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string VATNo { get; set; } = "";
        public string SVATNo { get; set; } = "";
        public AddAdvAnl()
        {
            AdvAnlTyp=new CodeBaseResponse();
        }
    }
}
