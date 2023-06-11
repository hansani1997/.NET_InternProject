using BL10.CleanArchitecture.Domain.Entities.Booking;
using BL10.CleanArchitecture.Domain.Entities.MaterData;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Principal;
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

        public CodeBaseResponse Department { get; set; }
        public Estimation WorkOrderSimpleEstimation { get; set; }
        public IList<OrderItem> CustomerComplains { get; set; }
        public IList<OrderItem> WorkOrderMaterials { get; set; }
        public IList<OrderItem> WorkOrderServices { get; set; }
        public IList<OrderItem> WorkOrderMiscellaneous { get; set; }
        public IList<OrderItem> OtherServices { get; set; }
        public BLTransaction WorkOrderTransaction { get; set; }
        public bool IsRowVisible { get; set; }
        public bool IsInWorkOrderEditMode { get; set; } = false;
        public bool IsAddMaterialsAndServiceMode { get; set; } = false;
        public int MaterialRequsitionKey { get; set; } = 1;
        public string? MaterialRequsitionNo { get; set; } = "";
        public int MaterialIssuedKey { get; set; } = 1;
        public string? MaterialIssuedNo { get; set; } = "";
        public int ServiceRequsitionKey { get; set; } = 1;
        public string ServiceRequsitionNo { get; set; } = "";
        public OrderItem SelectedServiceItem { get; set; }
        public OrderItem SelectedMaterialItem { get; set; }
        public OrderItem SelectedMiscellaneousItem { get; set; }
        public bool IsNeededToCreateNewWOForNewVehicle{get;set;}
        public bool IsOwnerWithDiscount { get; set; } = false;
        public bool IsOwnerWithoutDiscount { get; set; } = false;
        public bool IsInsuranceWithDiscount { get; set; } = false; 
        public bool IsInsuranceWithoutDiscount { get; set; } = false;

        public WorkOrder()
        {
            SelectedVehicle = new Vehicle();
            Department = new CodeBaseResponse();
            WorkOrderSimpleEstimation = new Estimation();
            CustomerComplains = new List<OrderItem>();
            WorkOrderMaterials = new List<OrderItem>();
            WorkOrderServices = new List<OrderItem>();
            WorkOrderMiscellaneous = new List<OrderItem>();
            OtherServices = new List<OrderItem>();
            WorkOrderTransaction=new BLTransaction();
            SelectedServiceItem = new OrderItem();
            SelectedMaterialItem = new OrderItem();
            SelectedMiscellaneousItem = new OrderItem();
            AddressCategory1 = new CodeBaseResponse();
            AddressCategory2 = new CodeBaseResponse();
            AddressCategory3 = new CodeBaseResponse();
            BaringHeaderPrincipleAccount = new AccountResponse();
            BaringHeaderCompanyAccount = new AccountResponse();
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
            CompanyPercentage = 0;
            CompanyAmount = 0;
            PrincipalPercentage = 0;
            PrincipalAmount = 0;
            CustomerAmount = 0;
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
            WorkOrderTransaction.Location = new CodeBaseResponse();
            WorkOrderTransaction.PaymentTerm = new CodeBaseResponse();
            WorkOrderTransaction.Address = new AddressResponse();
            WorkOrderTransaction.Account = new AccountResponse();
            WorkOrderTransaction.Rep = new AddressResponse();
            WorkOrderTransaction.InvoiceLineItems = new List<TransactionLineItem>();
            WorkOrderTransaction.YourReference = Guid.NewGuid().ToString().Substring(0, 8);
            WorkOrderTransaction.YourReferenceDate = DateTime.Now;
            WorkOrderTransaction.Code1 = new CodeBaseResponse();
            WorkOrderTransaction.SerialNumber = new ItemSerialNumber();
            WorkOrderTransaction.TransactionNumber = "";
            WorkOrderTransaction.TransactionKey= 1;
            WorkOrderTransaction.Prefix = "";
            WorkOrderTransaction.Prefix2 = "";
            WorkOrderTransaction.Remarks = "";
            WorkOrderTransaction.PreviewURL = "";
            WorkOrderTransaction.TransactionImageFilePath = "";
            WorkOrderTransaction.ApproveState= new CodeBaseResponse();
            BaringHeaderPrincipleAccount = new AccountResponse();
            BaringHeaderCompanyAccount= new AccountResponse();
            MaterialRequsitionKey = 1;
            ServiceRequsitionKey= 1;
            MaterialIssuedKey= 1;
            AddressCategory1 = new CodeBaseResponse();
            AddressCategory2 = new CodeBaseResponse();
            AddressCategory3 = new CodeBaseResponse();
        }
        public void CopyFrom(WorkOrder source)
        {
            source.CopyProperties(this);

        }

        public bool IsMaterialIssued()
        {
            return MaterialIssuedKey > 11;
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
        public CodeBaseResponse Brand { get; set; } 
        public CodeBaseResponse Model { get; set; } 
        public Warranty VehicleWarrannty { get; set; }
        public CodeBaseResponse MaintenancePackage { get; set; } 
        public decimal CurrentMilage { get; set; }
        public decimal PreviousMilage { get; set; }
        public string Fuel { get; set; }= "-";
        public BookingDetails LatestBook { get; set; }=new BookingDetails();
        public IList<WorkOrder> JobHistory { get; set; }
        public bool IsInsurence { get; set; } = false;
        public CodeBaseResponse Province { get; set; }
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
            Province = new CodeBaseResponse();
            Brand = new CodeBaseResponse();
            Model = new CodeBaseResponse();
            MaintenancePackage = new CodeBaseResponse();
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

        public bool GetWarrantyStatus()
        {
            if (DateTime.Compare(DateTime.Now, WarrantyEndDate)>0)
            {
                WarranrtyStatus = "Expired";
                return false;
            }
            else
            {
                WarranrtyStatus = "Available";
                return true;    
            }
        }
    }
    public class Estimation
    {
        public long EstimateKey { get; set; }
        public string EstimationNumber { get; set; } = "";
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

    public class CarOrdToOrdPostingRequest
    {
        public int FromOrderKey { get; set; }
        public int ToOrderKey { get; set; }
        public CodeBaseResponse ToOrderType { get; set; } = new CodeBaseResponse();
        public long ElementKey { get; set; }
    }

    public class WorkOrderResponse : OrderFindResults
    {
        public string? VehicleNumber { get; set; } = "";
        public ProjectResponse Project { get; set; }
        public AddressResponse ServiceAdvisor { get; set; }
        public AddressResponse VehicleAddress { get; set; }
        public int WorkOrderTransactionKey { get; set; } = 1;
        public int WorkOrderEstimationKey { get; set; } = 1;
        public int WorkOrderMaterialRequsitionKey { get; set; } = 1;
        public int WorkOrderServiceRequsitionKey { get; set; } = 1;
        public int MaterialIssueKey { get; set; } = 1;
        public WorkOrderResponse()
        {
            Project = new ProjectResponse();
            ServiceAdvisor = new AddressResponse();
            VehicleAddress = new AddressResponse();
        }
        public bool IsMaterialIssued()
        {
            return MaterialIssueKey > 11;
        }
    }

	public class OrderMultiTransactionPosting
	{
		public long ElementKey { get; set; } = 1;
		public long OrderKey { get; set; } = 1;
		public long TransactionTypeKey { get; set; }
		public long AccountKey { get; set; } = 1;
		public DateTime TransactionDate { get; set; } = DateTime.Now;

        //response

        public long TransactionKey { get; set; } = 1;
        public string TransactionNumber { get; set; } = "";
        public string TransactionTimeStamp { get; set; } = "";
        public CodeBaseResponse AccessLevel { get; set; }
        public CodeBaseResponse ConfidentialLevel { get; set; }

        public OrderMultiTransactionPosting()
        {
            AccessLevel = new CodeBaseResponse();
            ConfidentialLevel = new CodeBaseResponse();
        }
    }
}
