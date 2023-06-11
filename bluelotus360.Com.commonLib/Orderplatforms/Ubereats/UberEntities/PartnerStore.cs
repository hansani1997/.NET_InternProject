using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities
{
    public class PartnerStore
    {
        public PartnerStore()
        {
            UserLocationInOurSystem = new CodeBaseResponse();
            BU= new CodeBaseResponse();
        }
        public string PartnerStoreId { get; set; }
        public string PartnerStoreName { get; set; }
        public string PartnerStoreCode { get; set; }
        public CodeBaseResponse UserLocationInOurSystem { get; set; }
        public string[] ContactEmails { get; set; }
        public int AveragePreparationTime { get; set; }
        public bool IsProvisionEnabled { get; set; }
        public bool IsProvisionOnOff { get; set; }
        public CodeBaseResponse BU { get; set; }
    }
}
