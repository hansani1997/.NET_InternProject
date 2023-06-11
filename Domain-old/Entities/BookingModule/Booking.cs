using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Booking
{
    public class Booking 
    {
        
    }
    public class BookingDetails : BaseEntity
    {
        public long ElementKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Id { get; set; }
        public int ProcessDetailsKey { get; set; }
        public int ProjectKey { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime RequestedDate { get; set; }
        public AddressResponse Registration { get; set; }
        public int CustomerAddressKey { get; set; }
        public string CustomerName { get; set; }
        public CodeBaseResponse ServiceType { get; set; }
        public decimal Milage { get; set; }
        public string Remark { get; set; }
        public BookingVehicleDetails VehicleDetails { get; set; }
        public BookingTabDetails TabDetails { get; set; }
        public IList<CustomerDetailsByVehicle> CustomersDetails { get; set; }
        public AddressResponse SelectedCustomer { get; set; }
        public BookingInsertUpdate BookingInsertUpdateDetails { get; set; }
        public BookingDetails() 
        {
            Id = 0;
            ProcessDetailsKey = 0;
            ProjectKey = 0;
            TaskID = 0;
            TaskName = string.Empty;
            RequestedDate = DateTime.Now;
            Registration = new AddressResponse();
            CustomerAddressKey = 0;
            CustomerName = string.Empty;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            ServiceType = new CodeBaseResponse();
            Milage = 0;
            Remark = string.Empty;
            VehicleDetails = new BookingVehicleDetails();
            TabDetails = new BookingTabDetails();
            CustomersDetails = new List<CustomerDetailsByVehicle>();
            SelectedCustomer = new AddressResponse();
            BookingInsertUpdateDetails = new BookingInsertUpdate();
        }

        public void CopyFrom(BookingDetails source)
        {
            source.CopyProperties(this);

        }
    }

    public class BookingVehicleDetails : BaseEntity
    {
        public long ElementKey { get; set; }
        public AddressResponse Registration { get; set; }
        public int ModelKey { get; set; }
        public string MakeModelName { get; set; }
        public string OwnerName { get; set; }
        public string Code { get; set; }
        public string ContactNumber { get; set; }

        public BookingVehicleDetails() 
        {
            Registration = new AddressResponse();
            ModelKey = 0;
            MakeModelName = "";
            OwnerName = "";
            Code = "";
            ContactNumber = "";
        }

        public void CopyFrom(BookingVehicleDetails source)
        {
            source.CopyProperties(this);

        }
    }

    public class BookingInsertUpdate
    {
        public long ElementKey { get; set; }
        public int ProcessDetailsKey { get; set; }
        public int ProjectKey { get; set; }//
        public AddressResponse Registration { get; set; }
        public CodeBaseResponse ServiceType { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal Milage { get; set; }//
        public string Remark { get; set; }

        public BookingInsertUpdate() 
        {
            ProcessDetailsKey = 0;
            ProjectKey = 0;
            Registration = new AddressResponse();
            ServiceType = new CodeBaseResponse();
            BookingTime = new DateTime();
            Milage = 0;
            Remark = String.Empty;
        }
    }
    public class BookingTabDetails
    {
        public int ProcessDetailsKey { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string VehicleID { get; set; }
        public decimal Milage { get; set; }
        public string Make { get; set; }
        public int Model { get; set; }
        public string ChassiNumber { get; set; }
        public string EngineNumber { get; set; }
        public string Fuel { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal PreMilage { get; set; }
        public string CustomerName { get; set; }
        public string NIC { get; set; }
        public string MobileBusiness { get; set; }
        public string EmailBusiness { get; set; }
        public string Address { get; set; }

        public BookingTabDetails() 
        {
            ProcessDetailsKey = 0;
            TaskID = 0;
            TaskName = String.Empty;
            VehicleID = String.Empty;
            Milage = 0;
            Make = String.Empty;
            Model = 0;
            ChassiNumber = String.Empty;
            EngineNumber = String.Empty;
            Fuel = String.Empty;
            Category = String.Empty;
            SubCategory = String.Empty;
            DeliveryDate = new DateTime();
            PreMilage = 0;
            CustomerName = String.Empty;
            NIC = String.Empty;
            MobileBusiness = String.Empty;
            EmailBusiness = String.Empty;
            Address = String.Empty;
        }
    }

    public class CustomerDetailsByVehicle
    {
        public string Code { get; set; }
        public AddressResponse Customer { get; set; }
        public string Mobile { get; set; }

        public CustomerDetailsByVehicle() 
        {
            Code = "";
            Customer = new AddressResponse();
            Mobile = "";
        }
    }

    public class AppointmentDto
    {
        public int Id { get; set; } 

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public string ManagerName { get; set; } //field that matches the resource declaration Field
    }

    public class Resource
    {
        public string Value { get; set; }
        public string Color { get; set; } // must be a valid CSS string
    }
}
