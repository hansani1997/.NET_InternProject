using BL10.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities.MaterData;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL10.CleanArchitecture.Domain.Entities.ProjectManagement
{
    public class Process : TaskResponse
    {

        

        public ProjectResponse ProcessProject { get; set; }

        public string TaskSNamw { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal LineNumber { get; set; }
        public decimal LineNumber2 { get; set; }

        public int OrderDetKey { get; set; } // Need to Model to a class
        //public BLUIElement UiObjectRef { get; set; }
        public int PrcsObjKy { get; set; } // Need to clarify

        public ItemResponse Item { get; set; }

        public decimal AnlysisQuantity { get; set; }
        public decimal Quantity { get; set; }

        public decimal TransactionQauntity { get; set; }

        public UnitResponse TransactionUnit { get; set; }

        public decimal Rate { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal TransactionPrice { get; set; }

        public decimal PlanPercentage { get; set; }

        public decimal PlanQuantity { get; set; }
        public decimal ActualQuantity { get; set; }

        public decimal CumulativePlanQuantity { get; set; }

        public decimal ProgressPercentage { get; set; }

        public decimal ProgressQuantity { get; set; }

        public decimal BillQuantity { get; set; }
        public decimal BillPercentage { get; set; }

        public decimal AprovedBillPercentage { get; set; }
        public decimal ApprovedBillQuantity { get; set; }

        public decimal TotalBudgetTime { get; set; }

        public decimal TotalExecutedTime { get; set; }




        public bool IsCompleted { get; set; }
        public Process ParentProcess { get; set; }
        public Process ParentProcess2 { get; set; }

        public IList<Process> SubProcess { get; set; }

        public bool IsParent { get; set; }

        public DateTime WorkHours { get; set; }

        public CodeBaseResponse PorcessCategory { get; set; }

        public CodeBaseResponse Priority { get; set; }

        public CodeBaseResponse ApproveStatus { get; set; }

        public CodeBaseResponse ProcessType { get; set; }

        public AddressResponse Lead { get; set; }

        public AddressResponse CurrentResponsible { get; set; }

        public CodeBaseResponse TypeOfAction { get; set; }
        public CodeBaseResponse Milestone { get; set; }
        public long TaskSortOrder { get; private set; }
        public DateTime RequestDate { get; set; }

        public string RequestDateStr
        {
            get
            {
                return RequestDate.ToString("dd-MMM");
            }
        }

       // public ProcessScheduleDetail ScheduleDetail { get; set; }

        public decimal Weight { get; set; }
        public decimal CostPrice { get; set; }
        public IList<ProcessComponent> Resources { get; set; }


        public ItemResponse TaskFinishedProduct { get; set; }

        public bool HasDocument { get; set; }
        public bool HasComments { get; set; }

        public bool IsSprint { get; set; }
        public bool IsProgram { get; set; }
        public bool IsCorporate { get; set; }
        public bool IsStandUp { get; set; }
        public bool IsClients { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsConsultant { get; set; }
        public int ObjKy { get; set; }
        public int PrcsDetCat2Ky { get; set; }
        public int Anl2Ky { get; set; }
        public int BillNumberKey { get; set; } = 1;
        public string BillNumber { get; set; } = "";
        public decimal TotalBOQRate { get; set; }

        public string ItemNumber { get; set; } = "";


        public string DangerZoneClass
        {
            get
            {
                double daysLeft = RequestDate.Subtract(DateTime.Now).TotalDays;
                if (daysLeft > 7)
                {
                    return "non-danger";
                }
                else if (daysLeft > 2)
                {
                    return "on-warning";
                }
                else
                {
                    return "on-danger";

                }

            }


        }

        public string Title { get => TaskId; set => TaskId = value; } 
        public DateTime Start { get => this.ScheduleDetail.StartDate; set => this.ScheduleDetail.StartDate = value; }
        public DateTime End { get => this.ScheduleDetail.EndDate; set => this.ScheduleDetail.EndDate = value; }
        public decimal PercentComplete { get => this.ProgressPercentage; set => this.ProgressPercentage = value; }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }


        public decimal GOH { get; set; }
        public decimal DOH { get; set; }
        public decimal ProfitPer { get; set; }


        public bool HasMapping { get; set; } = false;

        public int MasterScheduleKey { get; set; } = 1;
        public string RecurrenceRule { get; set; } = "";
        public bool IsRecurrence { get; set; }
        public bool IsGeneric { get; set; }

        public decimal ResourceRequirementTotal { get; set; }
        public string Remarks { get; set; } = "";
        public string Remarks2 { get; set; } = "";
        public string TaskSNm { get; set; } = "";
        public int No1 { get; set; }
        public int No2 { get; set; }
        public int AddressKey { get; set; }
        public string AddressName { get; set; } = "";
        public string Excellent { get; set; } = "";
        public string VeryGood { get; set; } = "";
        public string Good { get; set; } = "";
        public string Fair { get; set; } = "";
        public string Poor { get; set; } = "";

        public long MenuObjectKey = 1;
        public ProcessScheduleDetail ScheduleDetail { get; set; }
        public decimal Rating
        {
            get
            {
                return (Weight * Convert.ToDecimal(No2)) / 100;
            }
        }
        public IList<Base64Document> Base64Documents { get; set; } = new List<Base64Document>();
        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }

        public IList<ProcessRemark> Comments { get; set; }
        public IList<CheckList> CheckList { get; set; }
        public int SubTaskCount { get; set; }=0;
        public bool IsPlaying { get; set; }
        public string ElapsedTime { get; set; } = "00:00:00";
        public DateTime TimerStartTime { get; set; }
        public System.Timers.Timer Timer { get; set; }
        public Process()
        {
            SubProcess = new List<Process>();
            ScheduleDetail = new ProcessScheduleDetail();
            RequestDate = new DateTime(1900, 1, 1);
            Lead = new AddressResponse();
            CurrentResponsible = new AddressResponse();
            PorcessCategory = new CodeBaseResponse();
            Priority = new CodeBaseResponse();
            ApproveStatus = new CodeBaseResponse();
            ProcessType = new CodeBaseResponse();
            TaskFinishedProduct = new ItemResponse();
            Resources = new List<ProcessComponent>();
            RecurrenceRule = "";
            TypeOfAction=new CodeBaseResponse();
            Milestone=new CodeBaseResponse();
            CreatedBy = new User();
            UpdatedBy = new User();
            Comments = new List<ProcessRemark>();
            Item = new ItemResponse();
            // UiObjectRef= new BLUIElement();
            TransactionUnit = new UnitResponse();
            ProcessProject= new ProjectResponse();
            CheckList= new List<CheckList>();

        }
        public Process(int ProcessKey)
        {
            SubProcess = new List<Process>();
           // ScheduleDetail = new ProcessScheduleDetail();
            RequestDate = new DateTime(1900, 1, 1);
            Lead = new AddressResponse();
            CurrentResponsible = new AddressResponse();
            PorcessCategory = new CodeBaseResponse();
            Priority = new CodeBaseResponseExtended();
            ApproveStatus = new CodeBaseResponse();
            ProcessType = new CodeBaseResponse();
            TaskFinishedProduct = new ItemResponse();
            Resources = new List<ProcessComponent>();
            this.TaskKey = ProcessKey;
            ProcessProject= new ProjectResponse();
        }

        public override string ToString()
        {
            return string.Format("Task Id {0}, Unit {1} , Cost Rate {2}, Rate {3}", new string[] { TaskId, TransactionUnit.UnitName, CostPrice.ToString(), Rate.ToString() });
        }

        public decimal GetAmount()
        {
            return TransactionQauntity * Rate;
        }


        public void GetProcessSortValue()
        {
            try
            {
                string[] template = new string[] { "0", "0", "0", "0" };
                long outvalue;
                if (TaskId != null && long.TryParse(TaskId.Replace(".", ""), out outvalue))
                {
                    long value = 0;
                    // So This is a Sortable using Logic of 10s
                    string[] Numbers = TaskId.Split('.');

                    if (Numbers.Length > 0)
                    {
                        // Asign for a Template...  for each can be merged
                        for (int i = 0; i < Numbers.Length; i++)
                        {
                            template[i] = Numbers[i];
                        }

                        for (int i = 0; i < template.Length; i++)
                        {
                            value += Convert.ToInt64(template[i]) * Convert.ToInt64(Math.Pow(10, template.Length - (i + 1)));
                            template[i] = "0";

                        }
                    }

                    TaskSortOrder = value;
                }
            }
            catch (Exception exp)
            {

            }


        }


    }

    public class ProcessComponent : BaseEntity
    {
        public int ProcessComponentKey { get; set; }
     //   public Process OwnerProcess { get; set; }
        public ItemResponse ComponentProduct { get; set; }

        public ItemResponse FinishedProduct { get; set; }
        public decimal LineNo { get; set; }

        public AddressResponse ComponentAddress { get; set; }

        public string Description { get; set; } = "";
        public decimal Quantity { get; set; }
        public decimal RequestedQuantity { get; set; }

        public decimal Rate { get; set; }

        public decimal PlannedQuantity { get; set; }

        public decimal AnalysisQuantity { get; set; } = 1;

        public decimal WastePercentage { get; set; }

        public decimal UsagePercentage { get; set; }

        public decimal TransactionQuantity { get; set; }

        public UnitResponse TransactionUnit { get; set; }

        public decimal CoverstionRate { get; set; }

        public decimal TransactionRate { get; set; }

        public decimal CompmonentFacPer { get; set; }

        public bool IsLead { get; set; }
        public int PrcessKey { get; set; }

        public ProcessComponent()
        {
            ComponentProduct = new ItemResponse();
            FinishedProduct = new ItemResponse();
            ComponentAddress = new AddressResponse();
           // OwnerProcess = new Process();
            TransactionUnit = new UnitResponse();

        }




    }


  
    public class ProcessRequest
    {
        public ProcessRequest() 
        {
            ProjectKey = 1;
            LeadAdrKy = 1;
            CurAdrKy = 1;
            PrcsStpKy = 1;
            PrtyKy = 1;
            ObjKy = 1;
            AprStsKy = 1;
            PrjPrcsCat1Ky = 1;
            PrcsDetCat1 = 1;
            PrcsObjKy = 1;
            ParentKey = 1;
            SearchQuery = "";
            ProcessScheduleHeaderKey = 1;
            FromDate=DateTime.Now.AddYears(-123);
            ToDate = DateTime.Now.AddYears(800);
            TaskKey = 1;
            PrcsCmpKy = 1;
           // Priority = new CodeBaseResponse();
        }
        public string ConditionCode { get; set; } = "";
        public long ProjectKey { get; set; }
        public int TaskKey { get; set; }
        public int LeadAdrKy { get; set; }
        public int CurAdrKy { get; set; }
        public int PrcsStpKy { get; set; }
        public int PrtyKy { get; set; }
        public int ObjKy { get; set; }
        public int PrcsObjKy { get; set; }
        public int AprStsKy { get; set; }
        public int PrjPrcsCat1Ky { get; set; }
        public int PrcsDetCat1 { get; set; }
        public int ParentKey { get; set; }

        public bool IsSubTaksMode { get; set; }

        public int ProcessScheduleHeaderKey { get; set; }


        public bool IsSprint { get; set; }
        public bool IsProgram { get; set; }
        public bool IsCorporate { get; set; }

        public bool IsStandup { get; set; }
        public bool IsClients { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsConsultant { get; set; }
        public bool IsCEO{ get; set; }


        public string SearchQuery { get; set; } = "";
        public int MasterScheduleKey { get; set; } = 1;


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PrcsCmpKy { get; set; }
        //public CodeBaseResponse Priority { get; set; }
    }

    public class ProcessScheduleDetail : BaseEntity
    {
        public int ProcessScheduleDetailKey { get; set; }

        public ProcessSchedule Schedule { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }

        public decimal WorkHours { get; set; }
        public short IndentLevel { get; set; }

        public int PlannedWorkHours { get; set; }
        public decimal PlanQuantity { get; set; }
        public decimal PlanTransactionQuantity { get; set; }

        public decimal Progress { get; set; }
        public decimal ProgressPercentage { get; set; }
        public decimal ProgressQuantity { get; set; }
        public UnitResponse ScheduleUnit { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsMileStone { get; set; }

        public int LineNumber { get; set; }

        public string StartDateStr { get { return StartDate.ToString("dd-MMM-yyyy"); } }

        public double Duration
        {
            get
            {
                TimeSpan timeSpan = EndDate - StartDate;
                return timeSpan.TotalDays;
            }
        }

        public double EarlyStart { get; set; }
        public double EarlyFinish { get; set; }
        public double LatestStart { get; set; }
        public double LatestFinish { get; set; }


        public ProcessScheduleDetail()
        {
            Schedule = new ProcessSchedule();
            StartDate = new DateTime(1900, 1, 1);
            EndDate = new DateTime(1900, 1, 1);
        }
    }
    public class ProcessSchedule : BaseEntity
    {
        public int ProcessScheduleKey { get; set; }
        public CodeBase ScheduleType { get; set; }
        public string VersionNumber { get; set; } = "";
        public string YourReference { get; set; } = "";
        public string Description { get; set; } = "";
        public Project ScheduleProject { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ScheduleToDate { get; set; }

        public int ScheduleNumber { get; set; }
        public decimal TaskCount { get; set; }
        public decimal TaskDone { get; set; }

        public bool IsBaseLine { get; set; }

        public decimal DonePercentage
        {
            get
            {
                if (TaskDone > 0)
                {
                    return Math.Floor(((TaskDone / TaskCount) * 100) * 100) / 100;
                }
                else
                {
                    return 0;
                }

            }
        }


    }

    public class ProcessRemark
    {
        public int ProcessLogKey { get; set; }
        public string Remarks { get; set; }
        public AddressResponse RemarksAddress { get; set; }
        public string commentDateTime { get; set; } = "";

        public int TaskKey { get; set; }


        public ProcessRemark()
        {
            RemarksAddress= new AddressResponse();
        }


    }

    public class TaskInsertUpdate
    {
        public long PrcsDetKy { get; set; }
        public string TaskId { get; set; } = "";
        public string TaskName { get; set; } = "";
        public int PrcsTypKy { get; set; }
        public decimal TrnQty { get; set; }
        public decimal TrnPri { get; set; }
        public decimal TrnRate { get; set; }
        public long TrnUnitKy { get; set; }
        public long LeadAdrKy { get; set; }
        public long CurAdrKy { get; set; }
        public long PrjKy { get; set; }
        public int PrtyKy { get; set; }
        public decimal LiNo { get; set; }
        public int ObjKy { get; set; }
        public int AprStsKy { get; set; }
        public string ReqDt { get; set; } = "";
        public long PrntKy { get; set; }
        public int PrcsDetCat1Ky { get; set; }
        public int PrjPrcsCat1Ky { get; set; }
        public decimal InitalPrgs { get; set; }
        public decimal ScheduleProgress { get; set; }
        public string VersionNo { get; set; } = "";
        public string YurRef { get; set; } = "";
        public string FromDt { get; set; } = "";
        public string ToDt { get; set; } = "";
        public decimal WrkHrs { get; set; }

        public int PrcsObjKy { get; set; }
        public string Des { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Weight { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSprint { get; set; }
        public bool IsProgram { get; set; }
        public bool IsCorporate { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsClients { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsConsultant { get; set; }
        public bool IsStandUp { get; set; }
        public string RecurrenceRule { get; set; } = "";
        public int AdrKy { get; set; }
        public string Remarks { get; set; } = "";
        public int No1 { get; set; }
        public int No2 { get; set; }
        public int IsAct { get; set; }
        public string Remarks2 { get; set; } = "";
        public string TaskSNm { get; set; } = "";
        public int PrcsDetCat2Ky { get; set; }
        public int Anl2Ky { get; set; }

    }

    public class CheckList
    {
        public int ProcessDetCdKey { get; set; }
        public int TaskKey { get; set; }
        public int LineNumber { get; set; }
        public string Content { get; set; } = "";
        public string Description { get; set; } = "";
        public bool isActive { get; set; }
        public bool isChecked { get; set; }
        public string Date { get; set; } = "";
    }
}
