using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe
{
    public class PickmeMenu
    {
        [JsonProperty("params")]
        public Params Params { get; set; }
        [JsonProperty("data")]
        public IList<data> Data { get; set; } = new List<data>();

    }
    public class data
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("ref_id")]
        public string RefID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("availability")]
        public string Availability { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
    public class Params
    {
        [JsonProperty("outlet_name")]
        public string OutletName { get; set; }
        
    }
}
