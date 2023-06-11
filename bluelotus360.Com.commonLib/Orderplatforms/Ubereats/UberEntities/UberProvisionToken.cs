using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus.Com.Domain.PartnerEntity
{
    public class UberProvisionToken
    {
    }

    public class UberTokenResponse
    {
        public string Access_token { get; set; }
        public long Expires_in { get; set; }
        public string Token_type { get; set; }
        public string Scope { get; set; }
        public string Refresh_token { get; set; }
        public int Last_authenticated { get; set; }
    }

    public class UberProvisionSetupForStore
    {
        //public bool Pos_integration_enabled { get; set; }
        //public string Store_configuration_data { get; set; }
        //public string Partner_store_id { get; set; }
        public string Merchant_store_id { get; set; }
        public string Integrator_store_id { get; set; }
        public bool Is_order_manager { get; set; }
        public bool Integration_enabled { get; set; }
    }


}

