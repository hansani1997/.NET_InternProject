using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus.Com.Domain.PartnerEntity
{
    public class UberStore
    {
        public UberStore()
        {
            Location = new UberStoreLocation();
            Pos_data = new UberProvisionData();
        }

        public string Name { get; set; }
        public string Store_id { get; set; }
        public UberStoreLocation Location { get; set; }
        public string[] Contact_emails { get; set; }
        public string Raw_hero_url { get; set; }
        public string Price_bucket { get; set; }
        public int Avg_prep_time { get; set; }
        public string Status { get; set; }
        public UberProvisionData Pos_data { get; set; }
        public string Merchant_store_id { get; set; }
        public string Timezone { get; set; }
    }

    public class UberProvisionData
    {
        public bool Integration_enabled { get; set; }
    }

    public class UberStoreLocation
    {
        public string Address { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postal_code { get; set; }
        public string State { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }

    public class UberStoreListResponse
    {
        public UberStoreListResponse()
        {
            Stores = new List<UberStore>();
        }
        public string Next_key { get; set; }
        public List<UberStore> Stores { get; set; }
    }

    public class UberStoreStatusSetting
    {
        public string Status { get; set; }
        public string Paused_until { get; set; }
        public string Reason { get; set; }
    }

    public class GettingUberStoreStatus
    {
        public string Status { get; set; }
        public string OfflineReason { get; set; }
    }

    public class UberStoreHolidayHoursSetting
    {
        public UberStoreHolidayHoursSetting()
        {
            Holiday_hours =  new Dictionary<string, UberEatsHolidayHours>  ();
        }
        public Dictionary<string, UberEatsHolidayHours> Holiday_hours {get; set;}
    }

    public class UberEatsHolidayHours
    {

        public UberEatsHolidayHours()
        {
            Open_time_periods = new List<UberEatsTimePeriod>();

        }
        public List<UberEatsTimePeriod> Open_time_periods { get; set; }
    }

}
