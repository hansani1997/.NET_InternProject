using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.WorkShopManagement
{
    public class VehicleSearch
    {
        public long ObjectKey { get; set; }
        public AddressResponse VehicleRegistration { get; set; }
        public ItemSerialNumber VehicleSerialNumber { get; set; }
        public AddressResponse RegisteredCustomer { get; set; }
        public AddressResponse RegisterNIC { get; set; }
        public VehicleSearch() 
        { 
            VehicleRegistration = new AddressResponse();
            RegisteredCustomer = new AddressResponse();
            RegisterNIC = new AddressResponse();
            VehicleSerialNumber = new ItemSerialNumber();
        }
    }
    public class WorkOrder : Order
    {
        public int TrnKy { get; set; }
        public Vehicle SelectedVehicle { get; set; }
        public decimal PrincipalPercentage { get; set; }
        public decimal PrincipalValue { get; set; }
        public decimal CarmartPercentage { get; set; }
        public decimal CarmartValue { get; set; }
        public CodeBaseResponse Department { get; set; }
        public Estimation WorkOrderSimpleEstimation { get; set; }
        public IList<OrderItem> CustomerComplains { get; set; }
        public IList<OrderItem> WorkOrderMaterials { get; set; }
        public IList<OrderItem> WorkOrderServices { get; set; }
        public IList<OrderItem> OtherServices { get; set; }
        public BLTransaction WorkOrderTransaction { get; set; }
        public bool IsRowVisible { get; set; }
        public bool IsInWorkOrderEditMode { get; set; } = false;
        

        public WorkOrder()
        {
            SelectedVehicle = new Vehicle();
            Department = new CodeBaseResponse();
            WorkOrderSimpleEstimation = new Estimation();
            CustomerComplains = new List<OrderItem>();
            WorkOrderMaterials = new List<OrderItem>();
            WorkOrderServices = new List<OrderItem>();
            OtherServices = new List<OrderItem>();
            WorkOrderTransaction=new BLTransaction();
        }

        public void WorkOrderClear()
        {
            //OrderLocation = new CodeBaseResponse();
            OrderPaymentTerm = new CodeBaseResponse();
            OrderCustomer = new AddressResponse();
            OrderRepAddress = new AddressResponse();
            OrderDocumentNumber = "";
            OrderItems = new List<OrderItem>();
            OrderType = new CodeBaseResponse();
            OrderAccount = new AccountResponse();
            SelectedOrderItem = new OrderItem();
            OrderItems = new List<OrderItem>();
            EditingLineItem = new OrderItem();
            OrderProject = new ProjectResponse();
            OrderStatus = new CodeBaseResponse();
            OrderCategory1 = new CodeBaseResponse();
            OrderCategory2 = new CodeBaseResponse();
            OrderPaymentTerm = new CodeBaseResponse();
            Department = new CodeBaseResponse();
            CarmartPercentage = 0;
            CarmartValue = 0;
            PrincipalPercentage = 0;
            PrincipalValue = 0;
            WorkOrderMaterials.Clear();
            WorkOrderServices.Clear();
            OtherServices.Clear();
            CustomerComplains.Clear();
            OrderItems.Clear();
            WorkOrderSimpleEstimation = new Estimation();
            OrderKey = 1;
            OrderNumber = "";
            OrderDate  = DateTime.Now;
            OrderFinishDate = DateTime.Now;
            Cd1Ky= 1;
            Prefix = "";
            MeterReading = 0;
            OrderPrefix = new CodeBaseResponse();
            EnteredUser = new AddressResponse();
            BussinessUnit = new CodeBaseResponse();

		}
        public void CopyFrom(WorkOrder source)
        {
            source.CopyProperties(this);

        }
    }

    public class Vehicle : BaseEntity
    {
        public long ObjectKey { get; set; }
        public DateTime VehicleRegisterDate { get; set; }
        public ItemResponse VehicleRegistration { get; set; }
        public AddressResponse VehicleAddress { get; set; }
        public AddressMaster RegisteredCustomer { get; set; }
        public AccountResponse RegisteredAccount { get; set; }
        public ItemSerialNumber SerialNumber { get; set; }
        public CodeBaseResponse Category { get; set; }
        public CodeBaseResponse SubCategory { get; set; }
        public string Brand { get; set; } = "-";
        public string Model { get; set; } = "-";
        public Warranty VehicleWarrannty { get; set; }
        public string MaintenancePackage { get; set; } ="-";
        public decimal CurrentMilage { get; set; }
        public decimal PreviousMilage { get; set; }
        public string Fuel { get; set; }= "-";
        public BookingDetails LatestBook { get; set; }    
        public IList<WorkOrder> JobHistory { get; set; }
        public bool IsInsurence { get; set; } = false;
        public Vehicle()
        {
            VehicleWarrannty = new Warranty();
            VehicleRegistration = new ItemResponse();
            RegisteredCustomer = new AddressMaster();
            SerialNumber= new ItemSerialNumber();
            Category=new CodeBaseResponse();
            SubCategory=new CodeBaseResponse();
            VehicleAddress=new AddressResponse();
            JobHistory = new List<WorkOrder>();
            LatestBook = new BookingDetails();
            RegisteredAccount=new AccountResponse();
        }

        public void CopyFrom(Vehicle source)
        {
            source.CopyProperties(this);

        }
    }

    public class Warranty
    {
        public string WarranrtyStatus { get; set; } = "-";
        public DateTime WarrantyStartDate { get; set; } = DateTime.Now;
        public DateTime WarrantyEndDate { get; set; } = DateTime.Now;
    }
    public class Estimation
    {
        public IList<OrderItem> EstimatedMaterials { get; set; }
        public IList<OrderItem> EstimatedServices { get; set; }  
        public decimal TotalValue { get; set; }
        public Estimation()
        {
            EstimatedMaterials = new List<OrderItem>();
            EstimatedServices = new List<OrderItem>();    
        }

    }

    public class UserRequestValidation
    {
        public bool IsError { get; set; }
        public string? Message { get; set; }
    }

}
