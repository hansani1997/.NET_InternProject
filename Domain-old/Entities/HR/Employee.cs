using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.HR
{
    public class EmployeeModel
    {
        public EmployeeCommonAttributes EmployeeBasicDetails { get; set; }
        public JobModel EmployeeJobModel { get; set; }
        public ContactDetails EmployeeContactDetails { get; set; }
        public List<Emergency> EmployeeEmergencyContactDetails { get; set; }
        public List<Education> EmployeeEducationDetails { get; set; }
        public List<WorkExperience> EmployeeExperienceDetails { get; set; }
        public List<QualificationDetails> EmployeeQualifications { get; set; }
        public List<PromotionDetails> EmployeePromotions { get; set; }
        public List<Transfer> EmployeeTransferDetails { get; set; }
        public List<SalaryHistory> EmployeeSalaryHistory { get; set; }
        public List<BasicSalaryModel> EmployeeBasicSalary { get; set; }
        public List<FixedAllowance> EmployeeFixedAllowances { get; set; }
        public List<DeductionModel> EmployeeFixedDeductions { get; set; }
        public List<LoanSelfService> EmployeeLoanDetails { get; set; }
        public ProfilePicture ProfPicture { get; set; }
        public string Address { get; set; }
        public IList<LeaveSummary> LeaveSummary { get; set; }=new List<LeaveSummary>();

    }
}
