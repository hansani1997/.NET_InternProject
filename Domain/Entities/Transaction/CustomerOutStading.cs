using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Transaction
{
    public class CustomerOutStanding
    {
        public long ObjectKey { get; set; }
        public DateTime AsAtDate { get; set; } = DateTime.Now;
        public CodeBaseResponse AddressCategory3 { get; set; } = new CodeBaseResponse();
        public AddressResponse Address { get; set; }=new AddressResponse();
        public CustomerOutStanding() 
        {
            AddressCategory3  = new CodeBaseResponse();
            Address = new AddressResponse();
        }
    }

    public class CustomerOutStadingDetails
    {
        public decimal BillAmount { get; set; }
        public decimal ChequeReturn { get; set; }
        public decimal Receipt { get; set; }
        public decimal TotaloutStanding { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditDates { get; set; }
        public CustomerOutStadingDetails()
        {

        }
    }
}
