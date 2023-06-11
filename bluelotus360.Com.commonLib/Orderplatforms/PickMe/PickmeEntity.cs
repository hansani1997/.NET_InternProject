using BlueLotus360.CleanArchitecture.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe
{
    public class PickmeEntity
    {
        public enum PickmeEndpoints
        {
            [Description("PickMeItemUpdate")]
            PickMeItemUpdate,
            [Description("GetOrder")]
            GetOrder,
            [Description("PickMeiTemRead")]
            PickMeiTemRead,
            [Description("{pickme_item_id}")]
            PickMeItemID,
        }

        public class GetOrder 
        {
            [JsonProperty("params")]
            public Params Params { get; set; }
            [JsonProperty("data")]
            public List<data> Data { get; set; } = new List<data>();

        }

        public class Params
        {
            [JsonProperty("pagination")]
            public pagination Pagination { get; set; }
            [JsonProperty("from_timestamp")]
            public string FromTimestamp { get; set; }
            [JsonProperty("to_timestamp")]
            public string ToTimestamp { get; set; }
        }

        public class pagination
        {
            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("total_records")]
            public int TotalRecords { get; set; }
        }
        public class location
        {
            [JsonProperty("address")]
            public string Address { get; set; }
        }

        public class customer
        {
            [JsonProperty("contact_number")]
            public string ContactNumber { get; set; }
            [JsonProperty("location")]
            public location Location { get; set; }
        }

        public class outlet
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("contact_number")]
            public string ContactNumber { get; set; }
            [JsonProperty("location")]
            public location Location { get; set; }
        }

        public class Items
        {
            [JsonProperty("id")]
            public int ID { get; set; }
            [JsonProperty("ref_id")]
            public string RefID { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("qty")]
            public int Qty { get; set; }
            [JsonProperty("total")]
            public string Total { get; set; }
            [JsonProperty("sp_ins")]
            public string SpIns { get; set; }
            [JsonProperty("options")]
            public List<ItemsOptions> ItemsOptions { get; set; }
            [JsonProperty("price")]
            public string Price { get; set; }
        }

        public class ItemsOptions
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("items")]
            public List<Items> ItemOptionsItems { get; set; }


        }



        public class Order
        {
            [JsonProperty("items")]
            public List<Items> Items { get; set; }
            [JsonProperty("delivery_note")]
            public string DeliveryNote { get; set; }
        }

        public class payment
        {
            [JsonProperty("total")]
            public string Total { get; set; }
            [JsonProperty("method")]
            public string Method { get; set; }
        }

        public class status
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("updated_timestamp")]
            public string UpdatedTimestamp { get; set; }
        }

        public class ItemUpdate
        {
            [JsonProperty("merchant_status")]
            public int MerchantStatus { get; set; }
            [JsonProperty("price")]
            public decimal Price { get; set; }
        }

        public class data
        {
            [JsonProperty("pickme_job_id")]
            public string PickmeJobID { get; set; }

            [JsonProperty("customer")]
            public customer Customer { get; set; }
            [JsonProperty("outlet")]
            public outlet Outlet { get; set; }
            [JsonProperty("order")]
            public Order Order { get; set; }
            [JsonProperty("payment")]
            public payment Payment { get; set; }
            [JsonProperty("status")]
            public status Status { get; set; }
            [JsonProperty("delivery_mode")]
            public string DeliveryMode { get; set; }
            [JsonProperty("created_timestamp")]
            public string CreatedTimestamp { get; set; }
        }

        public class errors
        {
            [JsonProperty("code")]
            public string Code { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
        }

        public class PickmeResponse
        {
            [JsonProperty("success")]
            public string Success { get; set; }

            public List<errors> Error { get; set; }
        }

    }

    
}
