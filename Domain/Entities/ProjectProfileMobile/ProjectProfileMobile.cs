using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile
{
    public class ProjectProfileRequest
    {
        public long ElementKey { get; set; }
    }
    public class ProjectProfileList : BaseEntity
    {
        public int PrjKy { get; set; }
        public string PrjNo { get; set; }
        public string PrjID { get; set; }
        public string PrjNm { get; set; }
        public CodeBaseResponse PrjTyp { get; set; }
        public int PrntKy { get; set; }
        public string PrntPrjId { get; set; }
        public string PrntPrjNm { get; set; }
        public CodeBaseResponse ProjectStatus { get; set; }
        public bool IsAct { get; set; }
        public bool IsApr { get; set; }
        public bool IsAlwTrn { get; set; }
        public bool IsPrnt { get; set; }
        public DateTime PrjStDt { get; set; }
        public DateTime FinDt { get; set; }
        public ItemResponse Item { get; set; }
        public string Alias { get; set; }

        public ProjectProfileList() 
        {
            PrjKy = 0;
            PrjNo = "0";
            PrjID = string.Empty;
            PrjNm = string.Empty;
            
            PrntKy = 1;
            PrntPrjId = string.Empty;
            PrntPrjNm = string.Empty;

            ProjectStatus = new CodeBaseResponse();
            PrjTyp = new CodeBaseResponse();

            IsAct = true;
            IsApr = true;
            IsAlwTrn = true;
            IsPrnt = true;

            PrjStDt = DateTime.Now;
            FinDt = DateTime.Now;

            Item = new ItemResponse();

            Alias = string.Empty;
        }

        public void CopyFrom(ProjectProfileList source)
        {
            source.CopyProperties(this);

        }
    }
}
