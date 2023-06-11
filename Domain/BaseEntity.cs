using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain
{
    public class BaseEntity
    {
        public byte IsActive { get; set; } = 1;
        public byte IsApproved { get; set; } = 1;
        public bool IsPersisted { get; set; }
        public bool IsDirty { get; set; }
        public bool IsMust { get; set; }
        public int RequestingObjectKey { get; set; } = 1;
        public IDictionary<string, object> AddtionalData { get; set; }

        public BaseEntity()
        {
            AddtionalData = new Dictionary<string, object>();
            
        }

    }
    public class DocumentRetrivaltDTO : BaseEntity
    {
        public int TransactionKey { get; set; }
        public long ItemKey { get; set; }
        public int OrderKey { get; set; }
        public int ProjectKey { get; set; }
        public int DocumentTypeKey { get; set; }
        public int EmployeeCodeKey { get; set; }
        public int EmployeeCodeDtKey { get; set; }
        public int ItemTransactionKey { get; set; }
        public int OrderDetailKey { get; set; }
        public int DocCategory1Key { get; set; }
        public int DocCategory2Key { get; set; }
        public int DocCategory3Key { get; set; }
        public int BUKey { get; set; }
        public int ProcessDetKey { get; set; }
        public int AdrKy { get; set; }
        public int CdKey { get; set; }
        public int ProjectStatusKey { get; set; }
        public DateTime NextActionDate { get; set; }
        public string KeyWord { get; set; }

        public int DocumentKey { get; set; }

        public DocumentRetrivaltDTO()
        {
            TransactionKey = 1;
            ItemKey = 1;
            OrderKey = 1;
            DocumentTypeKey = 1;
            ProjectKey = 1;
            EmployeeCodeDtKey = 1;
            EmployeeCodeKey = 1;
            ItemTransactionKey = 1;
            OrderDetailKey = 1;
            DocCategory1Key = 1;
            DocCategory2Key = 1;
            DocCategory3Key = 1;
            ProcessDetKey = 1;
            AdrKy = 1;
            CdKey = 1;
            NextActionDate = DateTime.Now;
            KeyWord = "";
            DocumentKey = 1;

        }
    }

    //public class Document : BaseEntity
    //{
    //    private int documentKey;
    //    private int companyKey;
    //    private string description;
    //    private string keyword;
    //    private string path;
    //    private string filename;
    //    private string versionNumber;
    //    private DateTime nextActionDate;
    //    private int projectKey;
    //    private CodeBaseResponse documentType;
    //    private CodeBaseResponse category;
    //    private long projectStatusKey;

    //    private int transactionKey;
    //    private string extention;
    //    private int employeeCodeKey;
    //    private int employeeCodeDtKey;

    //    private int buKey;

    //    public int DocumentKey { get => documentKey; set => documentKey = value; }
    //    public int CompanyKey { get => companyKey; set => companyKey = value; }
    //    public string Description { get => description; set => description = value; }
    //    public string Keyword { get => keyword; set => keyword = value; }
    //    public string Path { get => path; set => path = value; }
    //    public string Filename { get => filename; set => filename = value; }
    //    public string VersionNumber { get => versionNumber; set => versionNumber = value; }
    //    public DateTime NextActionDate { get => nextActionDate; set => nextActionDate = value; }

    //    public CodeBaseResponse Category { get => category; set => category = value; }
    //    public long ProjectStatusKey { get => projectStatusKey; set => projectStatusKey = value; }
    //    public CodeBaseResponse DocumentType { get => documentType; set => documentType = value; }
    //    public int TransactionKey { get => transactionKey; set => transactionKey = value; }
    //    public int OrderKey { get; set; }
    //    public int ItemKey { get; set; }
    //    public int ProjectKey { get => projectKey; set => projectKey = value; }
    //    public int AddressKey { get; set; }
    //    public int ItemTranKey { get; set; }
    //    public int ProcessDetKey { get; set; }
    //    public int OrderDetailKey { get; set; }
    //    public string Extention { get => extention; set => extention = value; }
    //    public int CdKey { get; set; }
      
    //    public long FileSize { get; set; }

      

    //    public int EmployeeCodeKey { get => employeeCodeKey; set => employeeCodeKey = value; }
    //    public int EmployeeCodeDtKey { get => employeeCodeDtKey; set => employeeCodeDtKey = value; }
    //    public int BuKey { get => buKey; set => buKey = value; }
    //}
    //public class BinaryDocument : Document
    //{
    //    public byte[] DocumentArray { get; set; }
    //}

    //public class Base64Document : Document
    //{
    //    public string Base64Source { get; set; }

    //}
}
