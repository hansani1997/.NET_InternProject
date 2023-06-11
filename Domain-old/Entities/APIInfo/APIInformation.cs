using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.APIInfo
{
    public class APIInformation : BaseEntity
    {
        public int APIIntegrationKey { get; set; } = 1;

        public string IntegrationId { get; set; }

        public string SecretInstanceKey { get; set; }

        public int MappedUserKey { get; set; } = 1;
        public int MappedCompanyKey { get; set; } = 1;
        public int MappedLocationKey { get; set; } = 1;
        public string MappedLocationName { get; set; }

        public bool IsAllowedLocalOnly { get; set; }
        public bool IsRestrictedToIP { get; set; }
        public bool ValidateTokenOnly { get; set; }

        public string RestrictToIP { get; set; }

        public string Scheme { get; set; }

        public string Description { get; set; }
        public string BaseURL { get; set; }
        public string AlertnateBaseURL { get; set; }

        public long PartnerOrderTypeKey { get; set; }
        public string PartnerOrderTypeCode { get; set; }
        public string PartnerOrderTypeName { get; set; }

        public string EndPointName { get; set; }
        public string EndPointURL { get; set; }
        public string EndPointToken { get; set; }
        public DateTime TokenGeneratedTime { get; set; }
        public DateTime TokenValidTillTime { get; set; }

        public string Environment { get; set; }

    }

    public class APIRequestParameters
    {
        public string EndPointName { get; set; }
        public int APIIntegrationKey { get; set; }
        public string APIIntegrationName { get; set; }
        public int LocationKey { get; set; }
        public string APIName { get; set; }
    }
}
