using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.HR
{
    public class EmployeeCommonAttributes
    {
        private string employeeNo;
        private string employeeName;
        private string callingName;
        private string surName;
        private string initials;
        private string ePF;
        private string nIC;
        private string dateOfBirth;
        private int employeeKey;
        private bool isActive;

        public string EmployeeNo { get => employeeNo; set => employeeNo = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
        public string CallingName { get => callingName; set => callingName = value; }
        public string SurName { get => surName; set => surName = value; }
        public string Initials { get => initials; set => initials = value; }
        public string EPF { get => ePF; set => ePF = value; }
        public string NIC { get => nIC; set => nIC = value; }
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public int EmployeeKey { get => employeeKey; set => employeeKey = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public EmployeeCommonAttributes()
        {
            EmployeeNo = "";
            EmployeeName = "";
            CallingName = "";
            SurName = "";
            Initials = "";
            EPF = "";
            NIC = "";
            DateOfBirth = "01/Jan/1900";
            EmployeeKey = 0;

        }
    }

    public class ContactDetails
    {
        private string street;
        private string city;
        private string state;
        private string postalCode;
        private string mobile;
        private string emailAddress;
        private string ofcMobile;
        private string ofcEmail;
        private bool isHasData;
        private int adressDetKy;

        public string Street { get => street; set => street = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public string Mobile { get => mobile; set => mobile = value; }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public string OfcMobile { get => ofcMobile; set => ofcMobile = value; }
        public string OfcEmail { get => ofcEmail; set => ofcEmail = value; }
        public bool IsHasData { get => isHasData; set => isHasData = value; }
        public int AdressDetKy { get => adressDetKy; set => adressDetKy = value; }

        public ContactDetails()
        {
            Street = "";
            City = "";
            State = "";
            PostalCode = "";
            Mobile = "";
            EmailAddress = "";
            OfcMobile = "";
            OfcEmail = "";
        }

        public bool Validation()
        {
            bool IsHasData = false;
            if (Street == "" && City == "" && State == "" && PostalCode == "" && Mobile == "" && EmailAddress == "" && OfcMobile == "" && OfcEmail == "")
            {
                IsHasData = false;
            }
            else
            {
                IsHasData = true;
            }
            return IsHasData;
        }
    }
    public class EmployeeProperty : BaseEntity
    {
        public int Key { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int EmployeeCdDtKy { get; set; }

        public string KeyName { get; set; }

        public string ConCd { get; set; }
    }

    public class FundAllocation
    {
        private int fundAllocationKey;
        private string fundAllocationEffective;
        private string fundAllocationToDt;
        private string fundAllocationKeyName;
        private int fundEmpCdDtKy;

        public int FundAllocationKey { get => fundAllocationKey; set => fundAllocationKey = value; }
        public string FundAllocationEffective { get => fundAllocationEffective; set => fundAllocationEffective = value; }
        public string FundAllocationToDt { get => fundAllocationToDt; set => fundAllocationToDt = value; }
        public string FundAllocationKeyName { get => fundAllocationKeyName; set => fundAllocationKeyName = value; }
        public int FundEmpCdDtKy { get => fundEmpCdDtKy; set => fundEmpCdDtKy = value; }
    }

    public class EmployeeDate
    {
        private int empDate;
        private string empDateName;
        private string dt;
        private int empDtEmpCdDtKy;

        public int EmpDate { get => empDate; set => empDate = value; }
        public string EmpDateName { get => empDateName; set => empDateName = value; }
        public string Dt { get => dt; set => dt = value; }
        public int EmpDtEmpCdDtKy { get => empDtEmpCdDtKy; set => empDtEmpCdDtKy = value; }
    }

    public class JobModel
    {
        public IList<EmployeeProperty> Details { get; set; }
        private int departmentKey;
        private string departmentNm;
        private string deptEffectiveDt;
        private string deptToDt;
        private int designationKey;
        private string designation;
        private string desgEffectiveDt;
        private string desgToDt;
        private int locationKey;
        private string locEffectiveDt;
        private string locToDt;
        private int employeeTypeKey;
        private string empTypEffective;
        private string empTypToDt;
        private int employeeCalendarKey;
        private string employeeCalendarEffective;
        private string employeeCalendarToDt;
        private int salaryPrcsGrpKey;
        private string salaryPrcsGrpEffective;
        private string salaryPrcsGrpToDt;
        private int fundAllocationKey;
        private string fundAllocationEffective;
        private string fundAllocationToDt;
        private int reportingPersonKey;
        private string reportingPersonEffective;
        private string reportingPersonToDt;
        private int levelKey;
        private string levelEffective;
        private string levelToDt;
        private int salaryCodeKey;
        private string salaryCodeEffective;
        private string salaryCodeToDt;
        private int rosterKey;
        private string rosterEffective;
        private string rosterToDt;
        private string jointDate;
        private string appointmentDate;
        private string leftDate;
        private string head;
        public JobModel()
        {
            Details = new List<EmployeeProperty>();

        }

        public int DepartmentKey { get => departmentKey; set => departmentKey = value; }
        public string DepartmentNm { get => departmentNm; set => departmentNm = value; }
        public string DeptEffectiveDt { get => deptEffectiveDt; set => deptEffectiveDt = value; }
        public string DeptToDt { get => deptToDt; set => deptToDt = value; }
        public int DesignationKey { get => designationKey; set => designationKey = value; }
        public string Designation { get => designation; set => designation = value; }
        public string DesgEffectiveDt { get => desgEffectiveDt; set => desgEffectiveDt = value; }
        public string DesgToDt { get => desgToDt; set => desgToDt = value; }
        public int LocationKey { get => locationKey; set => locationKey = value; }
        public string LocEffectiveDt { get => locEffectiveDt; set => locEffectiveDt = value; }
        public string LocToDt { get => locToDt; set => locToDt = value; }
        public int EmployeeTypeKey { get => employeeTypeKey; set => employeeTypeKey = value; }
        public string EmpTypEffective { get => empTypEffective; set => empTypEffective = value; }
        public string EmpTypToDt { get => empTypToDt; set => empTypToDt = value; }
        public int EmployeeCalendarKey { get => employeeCalendarKey; set => employeeCalendarKey = value; }
        public string EmployeeCalendarEffective { get => employeeCalendarEffective; set => employeeCalendarEffective = value; }
        public string EmployeeCalendarToDt { get => employeeCalendarToDt; set => employeeCalendarToDt = value; }
        public int SalaryPrcsGrpKey { get => salaryPrcsGrpKey; set => salaryPrcsGrpKey = value; }
        public string SalaryPrcsGrpEffective { get => salaryPrcsGrpEffective; set => salaryPrcsGrpEffective = value; }
        public string SalaryPrcsGrpToDt { get => salaryPrcsGrpToDt; set => salaryPrcsGrpToDt = value; }
        public int FundAllocationKey { get => fundAllocationKey; set => fundAllocationKey = value; }
        public string FundAllocationEffective { get => fundAllocationEffective; set => fundAllocationEffective = value; }
        public string FundAllocationToDt { get => fundAllocationToDt; set => fundAllocationToDt = value; }
        public int ReportingPersonKey { get => reportingPersonKey; set => reportingPersonKey = value; }
        public string ReportingPersonEffective { get => reportingPersonEffective; set => reportingPersonEffective = value; }
        public string ReportingPersonToDt { get => reportingPersonToDt; set => reportingPersonToDt = value; }
        public int LevelKey { get => levelKey; set => levelKey = value; }
        public string LevelEffective { get => levelEffective; set => levelEffective = value; }
        public string LevelToDt { get => levelToDt; set => levelToDt = value; }
        public int SalaryCodeKey { get => salaryCodeKey; set => salaryCodeKey = value; }
        public string SalaryCodeEffective { get => salaryCodeEffective; set => salaryCodeEffective = value; }
        public string SalaryCodeToDt { get => salaryCodeToDt; set => salaryCodeToDt = value; }
        public int RosterKey { get => rosterKey; set => rosterKey = value; }
        public string RosterEffective { get => rosterEffective; set => rosterEffective = value; }
        public string RosterToDt { get => rosterToDt; set => rosterToDt = value; }
        public string JointDate { get => jointDate; set => jointDate = value; }
        public string AppointmentDate { get => appointmentDate; set => appointmentDate = value; }
        public string LeftDate { get => leftDate; set => leftDate = value; }
        public string Head { get => head; set => head = value; }

        //public JobModel()
        //{
        //    DepartmentKey = 1;
        //    DesignationKey = 1;
        //    LocationKey = 1;
        //    EmployeeTypeKey = 1;
        //    EmployeeCalendarKey = 1;
        //    SalaryPrcsGrpKey = 1;
        //    FundAllocationKey = 1;
        //    ReportingPersonKey = 1;
        //    LevelKey = 1;
        //    SalaryCodeKey = 1;
        //    RosterKey = 1;
        //    DeptEffectiveDt  = LocEffectiveDt=LevelEffective = DesgEffectiveDt = EmpTypEffective = EmployeeCalendarEffective = SalaryPrcsGrpEffective = FundAllocationEffective
        //        = ReportingPersonEffective  = SalaryCodeEffective = RosterEffective = DateTime.Now.ToString("dd/MMM/yyyy");

        //    DeptToDt = DesgToDt = LocToDt = EmpTypToDt = EmployeeCalendarToDt = SalaryPrcsGrpToDt = FundAllocationToDt = ReportingPersonToDt = LevelToDt = SalaryCodeToDt = RosterToDt = "01/Jan/1900";
        //}

        public bool Validation()
        {
            bool IsHasData = false;
            if (DepartmentKey == 1 && DesignationKey == 1 && LocationKey == 1 && EmployeeTypeKey == 1 &&
                EmployeeCalendarKey == 1 && SalaryPrcsGrpKey == 1 && FundAllocationKey == 1 &&
                ReportingPersonKey == 1 && LevelKey == 1 && SalaryCodeKey == 1 && RosterKey == 1 && JointDate == "" && AppointmentDate == "" && LeftDate == "")
            {
                IsHasData = false;
            }
            else
            {
                IsHasData = true;
            }

            return IsHasData;
        }
    }

    public class PersonalDetails
    {
        private int gender;
        private int bloodGrp;
        private int ethnicGrp;
        private int nationality;
        private int maritalStatus;
        private int religion;

        public int Gender { get => gender; set => gender = value; }
        public int BloodGrp { get => bloodGrp; set => bloodGrp = value; }
        public int EthnicGrp { get => ethnicGrp; set => ethnicGrp = value; }
        public int Nationality { get => nationality; set => nationality = value; }
        public int MaritalStatus { get => maritalStatus; set => maritalStatus = value; }
        public int Religion { get => religion; set => religion = value; }


        public PersonalDetails()
        {
            Gender = BloodGrp = EthnicGrp = Nationality = MaritalStatus = Religion = 1;
        }


        public bool Validation()
        {
            bool IsHasData = false;
            if (Gender == 1 && BloodGrp == 1 && EthnicGrp == 1 && MaritalStatus == 1 && Religion == 1)
            {
                IsHasData = false;
            }
            else
            {
                IsHasData = true;
            }

            return IsHasData;
        }

    }

    public class EditPersonalDetails
    {
        public int EmpCdKy { get; set; }
        public int CdKy { get; set; }
        public string ConCd { get; set; }
        public bool IsProcessed { get; set; }
    }
    public class Emergency
    {
        private string emerName;
        private int emerRelationship;
        private string emerMobile;
        private string emerAddress;
        private string emerRelationshipName;
        private int emerEmpCdKy;
        private int cdKy;
        private string cdNm;
        private int adrKy;
        private int adrDetKy;
        private string street;
        private string adrNm;
        public string EmerName { get => emerName; set => emerName = value; }
        public int EmerRelationship { get => emerRelationship; set => emerRelationship = value; }
        public string EmerMobile { get => emerMobile; set => emerMobile = value; }
        public string EmerAddress { get => emerAddress; set => emerAddress = value; }
        public string EmerRelationshipName { get => emerRelationshipName; set => emerRelationshipName = value; }
        public int EmerEmpCdKy { get => emerEmpCdKy; set => emerEmpCdKy = value; }
        public int CdKy { get => cdKy; set => cdKy = value; }
        public string CdNm { get => cdNm; set => cdNm = value; }
        public int AdrKy { get => adrKy; set => adrKy = value; }
        public int AdrDetKy { get => adrDetKy; set => adrDetKy = value; }
        public string Street { get => street; set => Street = value; }
        public Emergency()
        {
            EmerName = EmerMobile = EmerAddress = "";
            EmerRelationship = 1;
        }
    }

    public class Education
    {
        private int qualification;
        private string institute;
        private string commenceDate;
        private string completedDate;
        private string remarks;
        private string qualificationName;
        private int empCdKy;

        public int Qualification { get => qualification; set => qualification = value; }
        public string Institute { get => institute; set => institute = value; }
        public string CommenceDate { get => commenceDate; set => commenceDate = value; }
        public string CompletedDate { get => completedDate; set => completedDate = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public string QualificationName { get => qualificationName; set => qualificationName = value; }
        public int EmpCdKy { get => empCdKy; set => empCdKy = value; }

        public Education()
        {
            Institute = Remarks = "";
            CommenceDate = DateTime.Now.ToString("dd/MMM/yyyy");
            CompletedDate = "";
            Qualification = 1;
            EmpCdKy = 1;
        }
    }

    public class WorkExperience
    {
        private int cdKy;
        private string companyName;
        private string fromDate;
        private string toDate;
        private int weDepartmentKy;
        private int weDesignationKy;
        private string weDepartmentName;
        private string weDesignationName;
        private int wrkEmpCdKy;

        public int CdKy { get => cdKy; set => cdKy = value; }
        public string CompanyName { get => companyName; set => companyName = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public string ToDate { get => toDate; set => toDate = value; }
        public int WeDepartmentKy { get => weDepartmentKy; set => weDepartmentKy = value; }
        public int WeDesignationKy { get => weDesignationKy; set => weDesignationKy = value; }
        public string WeDepartmentName { get => weDepartmentName; set => weDepartmentName = value; }
        public string WeDesignationName { get => weDesignationName; set => weDesignationName = value; }
        public int WrkEmpCdKy { get => wrkEmpCdKy; set => wrkEmpCdKy = value; }

        public WorkExperience()
        {
            CdKy = 1;
            CompanyName = "";
            WeDepartmentKy = WeDesignationKy = 1;
        }
    }

    public class PromotionDetails
    {
        private int proDesignation;
        private string proDate;
        private string proRemarks;
        private string proDesignationName;
        private int proEmpCdDtKy;
        private int cdKy;
        private string cdNm;
        public int ProDesignation { get => proDesignation; set => proDesignation = value; }
        public string ProDate { get => proDate; set => proDate = value; }
        public string ProRemarks { get => proRemarks; set => proRemarks = value; }
        public string ProDesignationName { get => proDesignationName; set => proDesignationName = value; }
        public int ProEmpCdDtKy { get => proEmpCdDtKy; set => proEmpCdDtKy = value; }
        public int CdKy { get => cdKy; set => cdKy = value; }
        public string CdNm { get => cdNm; set => cdNm = value; }
        public PromotionDetails()
        {
            ProDesignation = 1;
            ProDate = DateTime.Now.ToString("dd/MMM/yyyy");
            ProRemarks = "";
        }

        public bool validation()
        {
            bool IsHasData = false;

            if (ProDesignation == 1)
            {
                IsHasData = false;
            }
            else
            {
                IsHasData = true;
            }
            return IsHasData;
        }
    }

    public class Transfer
    {
        private int trnDepartment;
        private string trnDate;
        private string trnRemarks;
        private string trnDepartmentName;
        private int trnEmpCdDtKy;
        private int cdKy;
        private int empCdDtKy;
        private string cdNm;
        private string des;
        public int TrnDepartment { get => trnDepartment; set => trnDepartment = value; }
        public string TrnDate { get => trnDate; set => trnDate = value; }
        public string TrnRemarks { get => trnRemarks; set => trnRemarks = value; }
        public string TrnDepartmentName { get => trnDepartmentName; set => trnDepartmentName = value; }
        public int TrnEmpCdDtKy { get => trnEmpCdDtKy; set => trnEmpCdDtKy = value; }
        public int CdKy { get => cdKy; set => cdKy = value; }
        public int EmpCdDtKy { get => empCdDtKy; set => empCdDtKy = value; }
        public string CdNm { get => cdNm; set => cdNm = value; }
        public string Des { get => des; set => des = value; }
        public Transfer()
        {
            TrnDepartment = 1;
            TrnDate = DateTime.Now.ToString("dd/MMM/yyyy");
            TrnRemarks = "";

        }

        public bool validation()
        {
            bool IsHasData = false;

            if (TrnDepartment == 1)
            {
                IsHasData = false;
            }
            else
            {
                IsHasData = true;
            }
            return IsHasData;
        }
    }
    public class BasicSalaryModel
    {
        private int basicSalary;
        private string basFromDate;
        private string basToDate;
        private decimal bSAmt;
        private string basicSalaryName;
        private int bSEmpCdDtKy;

        public int BasicSalary { get => basicSalary; set => basicSalary = value; }
        public string BasFromDate { get => basFromDate; set => basFromDate = value; }
        public string BasToDate { get => basToDate; set => basToDate = value; }
        public decimal BSAmt { get => bSAmt; set => bSAmt = value; }
        public string BasicSalaryName { get => basicSalaryName; set => basicSalaryName = value; }
        public int BSEmpCdDtKy { get => bSEmpCdDtKy; set => bSEmpCdDtKy = value; }

        public BasicSalaryModel()
        {
            BasicSalary = 1;
            BasFromDate = DateTime.Now.ToString("dd/MMM/yyyy");
            BasToDate = "";
            BSAmt = 0;
        }

        public bool Validation()
        {
            bool IsHasData = false;
            if (BasicSalary > 11)
            {
                IsHasData = true;
            }
            else
            {
                IsHasData = false;
            }

            return IsHasData;
        }
    }

    public class WorkLocationModel
    {
        private int workPlace;
        private string fromDate;
        private string toDate;
        private string workPlaceName;
        private int workPlaceEmpCdDtKy;

        public int WorkPlace { get => workPlace; set => workPlace = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public string ToDate { get => toDate; set => toDate = value; }
        public string WorkPlaceName { get => workPlaceName; set => workPlaceName = value; }
        public int WorkPlaceEmpCdDtKy { get => workPlaceEmpCdDtKy; set => workPlaceEmpCdDtKy = value; }



        public WorkLocationModel()
        {
            WorkPlace = 1;
            FromDate = DateTime.Now.ToString("dd/MMM/yyyy");
            ToDate = "";
        }



        public bool Validation()
        {
            bool IsHasData = false;
            if (WorkPlace > 11)
            {
                IsHasData = true;
            }
            else
            {
                IsHasData = false;
            }

            return IsHasData;
        }
    }

    public class OTStsModel
    {
        private int oTSts;
        private string oTStsFromDate;
        private string oTStsToDate;
        private string oTStsName;
        private int oTStsEmpCdDtKy;




        public OTStsModel()
        {
            OTSts = 1;
            OTStsFromDate = DateTime.Now.ToString("dd/MMM/yyyy");
            OTStsToDate = "";
        }

        public int OTSts { get => oTSts; set => oTSts = value; }
        public string OTStsFromDate { get => oTStsFromDate; set => oTStsFromDate = value; }
        public string OTStsToDate { get => oTStsToDate; set => oTStsToDate = value; }
        public string OTStsName { get => oTStsName; set => oTStsName = value; }
        public int OTStsEmpCdDtKy { get => oTStsEmpCdDtKy; set => oTStsEmpCdDtKy = value; }

        public bool Validation()
        {
            bool IsHasData = false;
            if (OTSts > 11)
            {
                IsHasData = true;
            }
            else
            {
                IsHasData = false;
            }

            return IsHasData;
        }
    }


    public class AdditionAftrNet
    {

        private int addAftrAllowance;
        private string addAftrFromDate;
        private string addAftrToDate;
        private decimal addAftrAmt;
        private string addAftrAllowanceName;
        private int addAftrAppliedTo;
        private string addAftrAppliedToName;
        private int addAftrEmpCdDtKy;
        private int addAftrEmpKy;
        private int isHold;

        public int AddAftrAllowance { get => addAftrAllowance; set => addAftrAllowance = value; }
        public string AddAftrFromDate { get => addAftrFromDate; set => addAftrFromDate = value; }
        public string AddAftrToDate { get => addAftrToDate; set => addAftrToDate = value; }
        public decimal AddAftrAmt { get => addAftrAmt; set => addAftrAmt = value; }
        public string AddAftrAllowanceName { get => addAftrAllowanceName; set => addAftrAllowanceName = value; }
        public int AddAftrAppliedTo { get => addAftrAppliedTo; set => addAftrAppliedTo = value; }
        public string AddAftrAppliedToName { get => addAftrAppliedToName; set => addAftrAppliedToName = value; }
        public int AddAftrEmpCdDtKy { get => addAftrEmpCdDtKy; set => addAftrEmpCdDtKy = value; }
        public int AddAftrEmpKy { get => addAftrEmpKy; set => addAftrEmpKy = value; }
        public int IsHold { get => isHold; set => isHold = value; }
    }

    public class AftrNetDeductionModel
    {
        private int dedAftrDeduction;
        private string dedAftrFromDate;
        private string dedAftrToDate;
        private decimal dedAftrAmt;
        private string dedAftrDeductionName;
        private int dedAftrAppliedTo;
        private string dedAftrAppliedToName;
        private int dedAftrEmpCdDtKy;
        private int dedAftrEmpKy;
        private int isHold;

        public int DedAftrDeduction { get => dedAftrDeduction; set => dedAftrDeduction = value; }
        public string DedAftrFromDate { get => dedAftrFromDate; set => dedAftrFromDate = value; }
        public string DedAftrToDate { get => dedAftrToDate; set => dedAftrToDate = value; }
        public decimal DedAftrAmt { get => dedAftrAmt; set => dedAftrAmt = value; }
        public string DedAftrDeductionName { get => dedAftrDeductionName; set => dedAftrDeductionName = value; }
        public int DedAftrAppliedTo { get => dedAftrAppliedTo; set => dedAftrAppliedTo = value; }
        public string DedAftrAppliedToName { get => dedAftrAppliedToName; set => dedAftrAppliedToName = value; }
        public int DedAftrEmpCdDtKy { get => dedAftrEmpCdDtKy; set => dedAftrEmpCdDtKy = value; }
        public int DedAftrEmpKy { get => dedAftrEmpKy; set => dedAftrEmpKy = value; }
        public int IsHold { get => isHold; set => isHold = value; }
    }

    public class DeductionModel
    {
        private int deduction;
        private string deductFromDate;
        private string deductToDate;
        private decimal deductAmt;
        private string deductionName;
        private int deductAppliedTo;
        private string deductAppliedToName;
        private int deductEmpCdDtKy;
        private int dedEmpKy;
        private int isHold;
        private string adrNm;
        public int Deduction { get => deduction; set => deduction = value; }
        public string DeductFromDate { get => deductFromDate; set => deductFromDate = value; }
        public string DeductToDate { get => deductToDate; set => deductToDate = value; }
        public decimal DeductAmt { get => deductAmt; set => deductAmt = value; }
        public string DeductionName { get => deductionName; set => deductionName = value; }
        public int DeductAppliedTo { get => deductAppliedTo; set => deductAppliedTo = value; }
        public string DeductAppliedToName { get => deductAppliedToName; set => deductAppliedToName = value; }
        public int DeductEmpCdDtKy { get => deductEmpCdDtKy; set => deductEmpCdDtKy = value; }
        public int DedEmpKy { get => dedEmpKy; set => dedEmpKy = value; }
        public int IsHold { get => isHold; set => isHold = value; }
        public string AdrNm { get => adrNm; set => adrNm = value; }
    }
    public class Addition
    {

        private int allowance;
        private string alwFromDate;
        private string alwToDate;
        private decimal alwAmt;
        private string allowanceName;
        private int alWAppliedTo;
        private string alWAppliedToName;
        private int alWEmpCdDtKy;
        private int alwEmpKy;
        private int isHold;

        public int Allowance { get => allowance; set => allowance = value; }
        public string AlwFromDate { get => alwFromDate; set => alwFromDate = value; }
        public string AlwToDate { get => alwToDate; set => alwToDate = value; }
        public decimal AlwAmt { get => alwAmt; set => alwAmt = value; }
        public string AllowanceName { get => allowanceName; set => allowanceName = value; }
        public int AlWAppliedTo { get => alWAppliedTo; set => alWAppliedTo = value; }
        public string AlWAppliedToName { get => alWAppliedToName; set => alWAppliedToName = value; }
        public int AlWEmpCdDtKy { get => alWEmpCdDtKy; set => alWEmpCdDtKy = value; }
        public int AlwEmpKy { get => alwEmpKy; set => alwEmpKy = value; }
        public int IsHold { get => isHold; set => isHold = value; }
    }

    public class ReportingPerson
    {
        public int ReportingPersonKey { get; set; }
        public string ReportingPersonKeyName { get; set; }

        public string ReportingPersonEffective { get; set; }
        public string ReportingPersonToDt { get; set; }

        public int RPEmpCdDtKy { get; set; }
        public int RPCdKy { get; set; }


        public ReportingPerson()
        {
            ReportingPersonKey = 1;
            ReportingPersonEffective = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public bool Validation()
        {
            bool IsHasData = false;
            if (ReportingPersonKey > 11 && ReportingPersonEffective != "")
            {
                IsHasData = true;
            }

            else
            {
                IsHasData = false;
            }

            return IsHasData;
        }
    }

    public class OTEntryRetrival
    {
        private int employeeKey;
        private string fromDate;
        private string toDate;
        private int pageNumber;
        private int pageSize;

        public int EmployeeKey { get => employeeKey; set => employeeKey = value; }
        public string FromDate { get => fromDate; set => fromDate = value; }
        public string ToDate { get => toDate; set => toDate = value; }
        public int PageNumber { get => pageNumber; set => pageNumber = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }

        public OTEntryRetrival()
        {
            EmployeeKey = 1;
            FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            ToDate = DateTime.Now.ToString("yyyy/MM/dd");
            PageNumber = 1;
            PageSize = 10;
        }
    }

    public class MonthlyEntryRetrival
    {
        private int employeeKey;
        private string entryDate;
        private int pageNumber;
        private int pageSize;
        public int EmployeeKey { get => employeeKey; set => employeeKey = value; }
        public string EntryDate { get => entryDate; set => entryDate = value; }
        public int PageNumber { get => pageNumber; set => pageNumber = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }

        public MonthlyEntryRetrival()
        {
            EmployeeKey = 1;
            EntryDate = DateTime.Now.ToString("yyyy/MM/dd");
            PageNumber = 1;
            PageSize = 10;
        }
    }

    public class OverTime
    {
        private int oTTyp;
        private string oTTypName;
        private string eftvDt;
        private double oTHour;
        private int employeeKey;
        private int empCdDtKy;

        public int OTTyp { get => oTTyp; set => oTTyp = value; }
        public string OTTypName { get => oTTypName; set => oTTypName = value; }
        public string EftvDt { get => eftvDt; set => eftvDt = value; }
        public double OTHour { get => oTHour; set => oTHour = value; }
        public int EmployeeKey { get => employeeKey; set => employeeKey = value; }
        public int EmpCdDtKy { get => empCdDtKy; set => empCdDtKy = value; }
    }

    public class EmployeeGrade
    {

        public int Grade { get; set; }
        public string GradeFromDate { get; set; }
        public string GradeToDate { get; set; }
        public string GradeName { get; set; }
        public int GradeEmpCdDtKy { get; set; }


    }

    public class GradeRevision
    {

        public int Grade { get; set; }
        public string EffectiveDate { get; set; }
        public string GradeToDate { get; set; }
        public string GradeName { get; set; }
        public int CdMasDtKy { get; set; }
        public decimal Amount { get; set; }


    }

    public class ProfilePicture
    {
        public int DocKy { get; set; }
        public string FileName { get; set; }
        public DateTime InsertDt { get; set; }
        public int FileSize { get; set; }
        public string CdNm { get; set; }
        public string OurCd { get; set; }
        public string FileContent { get; set; }

    }

    public class QualificationDetails
    {
        public string CdNm { get; set; }
        public int EmpCdKy { get; set; }
        public int CdKy { get; set; }
        public string EftvDt { get; set; }
        public string ToDt { get; set; }
        public string Designation { get; set; }

    }

    public class SalaryHistory
    {
        private int empKy;
        private string salaryDate;
        private string fromDt;
        private string toDt;
        private int salTypKy;
        private int salPrcsGrpKy;

        public int EmpKy { get => empKy; set => empKy = value; }
        public string SalaryDt { get => salaryDate; set => salaryDate = value; }
        public string FromDt { get => fromDt; set => fromDt = value; }
        public string ToDt { get => toDt; set => toDt = value; }
        public int SalTypKy { get => salTypKy; set => salTypKy = value; }
        public int SalPrcsGrpKy { get => salPrcsGrpKy; set => salPrcsGrpKy = value; }

    }

    public class PaySlipDetails
    {

        private int empKy;
        private int cKy;
        private string empNo;
        private string empNm;
        private string epfNo;
        private string nic;
        private string salaryDate;
        private decimal amt;
        private string code;
        private string cdNo;
        private int conNo1;
        private string conCd;
        private string partNm;
        private string buNm;
        private string cNm;
        private string desgNm;
        private int cdKy;
        private string ourCd;
        private long accNo;
        private string brNm;
        private string bnkNm;
        private int soNo;
        private int roNo;
        private int noodDay;
        private int noOfDays;
        private string location;

        public int EmpKy { get => empKy; set => empKy = value; }
        public string SalaryDt { get => salaryDate; set => salaryDate = value; }
        public int CKy { get => cKy; set => cKy = value; }
        public string EmpNo { get => empNo; set => empNo = value; }
        public string EmpNm { get => empNm; set => empNm = value; }
        public string EpfNo { get => epfNo; set => epfNo = value; }
        public string NIC { get => nic; set => nic = value; }
        public decimal Amt { get => amt; set => amt = value; }
        public string Code { get => code; set => code = value; }
        public string CdNo { get => cdNo; set => cdNo = value; }
        public int ConNo1 { get => conNo1; set => conNo1 = value; }
        public string ConCd { get => conCd; set => conCd = value; }
        public string PartNm { get => partNm; set => partNm = value; }
        public string BuNm { get => buNm; set => buNm = value; }
        public string CNm { get => cNm; set => cNm = value; }
        public string DesgNm { get => desgNm; set => desgNm = value; }
        public int CdKy { get => cdKy; set => cdKy = value; }
        public string OurCd { get => ourCd; set => ourCd = value; }
        public long AccNo { get => accNo; set => accNo = value; }
        public string BrNm { get => brNm; set => brNm = value; }
        public string BnkNm { get => bnkNm; set => bnkNm = value; }
        public int SoNo { get => soNo; set => soNo = value; }
        public int RoNo { get => roNo; set => roNo = value; }
        public int NoodDay { get => noodDay; set => noodDay = value; }
        public int NoOfDays { get => noOfDays; set => noOfDays = value; }
        public string Location { get => location; set => location = value; }
    }

    public class FixedAllowance
    {
        private int fxAlEmpCdDtKy;
        private decimal fxAmt;
        private string fromDt;
        private string toDt;
        private string adrNm;
        private string cdNm;
        private int cdKy;
        private int isStop;

        public int FxAlEmpCdDtKy { get => fxAlEmpCdDtKy; set => fxAlEmpCdDtKy = value; }
        public decimal FxAmt { get => fxAmt; set => fxAmt = value; }
        public string FromDt { get => fromDt; set => fromDt = value; }
        public string ToDt { get => toDt; set => toDt = value; }
        public string AdrNm { get => adrNm; set => adrNm = value; }
        public string CdNm { get => cdNm; set => cdNm = value; }
        public int CdKy { get => cdKy; set => cdKy = value; }
        public int IsStop { get => isStop; set => isStop = value; }
    }

    public class LoanSelfService
    {
        private int cdKy;
        private string cdNm;
        private string fromDt;
        private decimal loanAmt;
        private decimal paidCapitalAmt;
        private decimal outStandingCapital;
        private decimal intrRate;
        private int isStop;
        private int isIntStop;
        private decimal interestPaid;

        public int CdKy { get => CdKy; set => CdKy = value; }
        public string CdNm { get => cdNm; set => cdNm = value; }
        public string FromDt { get => fromDt; set => fromDt = value; }
        public decimal LoanAmt { get => loanAmt; set => loanAmt = value; }
        public decimal PaidCapitalAmt { get => paidCapitalAmt; set => paidCapitalAmt = value; }
        public decimal OutStandingCapital { get => outStandingCapital; set => outStandingCapital = value; }
        public decimal IntrRate { get => intrRate; set => intrRate = value; }
        public int IsStop { get => isStop; set => isStop = value; }
        public int IsIntStop { get => isIntStop; set => isIntStop = value; }
        public decimal InterestPaid { get => interestPaid; set => interestPaid = value; }
    }

}
