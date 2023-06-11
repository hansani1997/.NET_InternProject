using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities
{
    public class MenuFromUber
    {
       
        public class Calories
        {
            public int lower_range { get; set; }
            public int upper_range { get; set; }
            public string display_type { get; set; }
        }

        public class Category
        {
            public string id { get; set; }
            public Title title { get; set; }
            public Subtitle subtitle { get; set; }
            public List<Entity> entities { get; set; }
        }

        public class Classifications
        {
            public bool can_serve_alone { get; set; }
            public int alcoholic_items { get; set; }
            public DietaryLabelInfo dietary_label_info { get; set; }
            public object ingredients { get; set; }
            public object additives { get; set; }
        }

        public class DefaultValue
        {
            public List<string> labels { get; set; }
            public string source { get; set; }
        }

        public class Description
        {
            public Translations translations { get; set; }
        }

        public class DietaryLabelInfo
        {
            public object labels { get; set; }
        }

        public class DishInfo
        {
            public Classifications classifications { get; set; }
        }

        public class DisplayOptions
        {
            public bool disable_item_instructions { get; set; }
        }

        public class Entity
        {
            public string id { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public Title title { get; set; }
            public Description description { get; set; }
            public string image_url { get; set; }
            public PriceInfo price_info { get; set; }
            public TaxInfo tax_info { get; set; }
            public NutritionalInfo nutritional_info { get; set; }
            public DishInfo dish_info { get; set; }
            public TaxLabelInfo tax_label_info { get; set; }
            public ProductInfo product_info { get; set; }
            public object bundled_items { get; set; }
            public SuspensionInfo suspension_info { get; set; }
        }

        public class Kilojoules
        {
            public int lower_range { get; set; }
            public int upper_range { get; set; }
            public string display_type { get; set; }
        }

        public class Menu
        {
            public string id { get; set; }
            public Title title { get; set; }
            public Subtitle subtitle { get; set; }
            public List<ServiceAvailability> service_availability { get; set; }
            public List<string> category_ids { get; set; }
        }

        public class NutritionalInfo
        {
            public Calories calories { get; set; }
            public Kilojoules kilojoules { get; set; }
            public object allergens { get; set; }
        }

        public class PriceInfo
        {
            public int price { get; set; }
            public List<object> overrides { get; set; }
        }

        public class ProductInfo
        {
            public List<object> product_traits { get; set; }
            public object countries_of_origin { get; set; }
        }

        public class Root
        {
            public List<Menu> menus { get; set; }
            public List<Category> categories { get; set; }
            public List<Item> items { get; set; }
            public object modifier_groups { get; set; }
            public DisplayOptions display_options { get; set; }
        }

        public class ServiceAvailability
        {
            public string day_of_week { get; set; }
            public List<TimePeriod> time_periods { get; set; }
        }

        public class Subtitle
        {
            public Translations translations { get; set; }
        }

        public class Suspension
        {
            public object suspend_until { get; set; }
        }

        public class SuspensionInfo
        {
            public Suspension suspension { get; set; }
            public List<object> overrides { get; set; }
        }

        public class TaxInfo
        {
            public int tax_rate { get; set; }
            public int vat_rate_percentage { get; set; }
        }

        public class TaxLabelInfo
        {
            public DefaultValue default_value { get; set; }
        }

        public class TimePeriod
        {
            public string start_time { get; set; }
            public string end_time { get; set; }
        }

        public class Title
        {
            public Translations translations { get; set; }
        }

        public class Translations
        {
            public string en { get; set; }
        }


    }
}
