using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus.Com.Domain.PartnerEntity
{
    //refer this for any clarification
    //https://developer.uber.com/docs/eats/api/v2/put-eats-stores-storeid-menus

    public class UberBaseMenu
    {
        public UberBaseMenu()
        {
            Title = new MultiLanguageText();
            Subtitle = new MultiLanguageText();
            Description = new MultiLanguageText();
        }
        public string Id { get; set; } = null;
        public string External_data { get; set; } = null;
        public MultiLanguageText Title { get; set; } = null;
        public MultiLanguageText Subtitle { get; set; } = null;
        public MultiLanguageText Description { get; set; } = null;
    }

    public class UberEatsMenu
    {
        public UberEatsMenu()
        {
            Menus = new List<UberEatsMenuItems>();
            Categories = new List<UberEatsCategoryItems>();
            Items = new List<UberEatsItemsForMenu>();
            Modifier_groups = new List<UberEatsModifierGroupsForMenu>();
            Display_options = new UberEatsDisplayOptions();
        }
        public List<UberEatsMenuItems> Menus { get; set; } = null;
        public List<UberEatsCategoryItems> Categories { get; set; } = null;
        public List<UberEatsItemsForMenu> Items { get; set; } = null;
        public List<UberEatsModifierGroupsForMenu> Modifier_groups { get; set; } = null;
        public UberEatsDisplayOptions Display_options { get; set; } = null;
        //public string Menu_type { get; set; } = null; //enum

    }

    public class UberEatsDisplayOptions
    {
        public bool Disable_item_instructions { get; set; }
    }

    public class UberEatsMenuItems : UberBaseMenu
    {
        public UberEatsMenuItems()
        {
            Service_availability = new List<UberEatsServiceAvailability>();
            Category_ids = new List<string>();
        }
        public List<UberEatsServiceAvailability> Service_availability { get; set; } = null;
        public List<string> Category_ids { get; set; } = null;
    }

    public class MultiLanguageText
    {
        public MultiLanguageText()
        {
            Translations = new TranslationsForUber();
        }
        //public object Translations { get; set; }
        public TranslationsForUber Translations { get; set; } = null;
    }

    public class TranslationsForUber
    {
        public string En_us { get; set; } = "";
    }

    public class UberEatsServiceAvailability
    {
        public UberEatsServiceAvailability()
        {
            Time_periods = new List<UberEatsTimePeriod>();
        }
        public string Day_of_week { get; set; } = ""; //enum
        public List<UberEatsTimePeriod> Time_periods { get; set; } = null;
    }

    public class UberEatsTimePeriod
    {
        public string Start_time { get; set; } = "";
        public string End_time { get; set; } = "";
    }

    public class UberEatsCategoryItems : UberBaseMenu
    {
        public UberEatsCategoryItems()
        {
            Entities = new List<MenuEntity>();
        }
        public List<MenuEntity> Entities { get; set; } = null;
    }

    public class MenuEntity
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = ""; //enum
    }

    public enum MenuEntityType
    {
        ITEM,
        MODIFIER_GROUP,
    }

    public class UberEatsItemsForMenu : UberBaseMenu
    {
        public UberEatsItemsForMenu()
        {
            Price_info = new UberEatsMenuPriceRules();
            //Quantity_info = new UberEatsMenuQuantityConstraintRules();
            Suspension_info = new UberEatsMenuSuspensionRules();
            Modifier_group_ids = new UberEatsMenuModifierGroupsRules();
            Tax_info = new UberEatsMenuTaxInfo();
            Nutritional_info = new UberEatsMenuNutritionalInfo();
            Dish_info = new UberEatsMenuDishInfo();
            //Visibility_info = new UberEatsMenuVisibilityInfo();
            //Tax_label_info = new UberEatsMenuTaxLabelsRuleSet();
            //Product_info = new UberEatsMenuProductInfo();
        }

        public string Image_url { get; set; } = null;
        public UberEatsMenuPriceRules Price_info { get; set; } = null;
        //public UberEatsMenuQuantityConstraintRules Quantity_info { get; set; } = null;
        public UberEatsMenuSuspensionRules Suspension_info { get; set; } = null;
        public UberEatsMenuModifierGroupsRules Modifier_group_ids { get; set; } = null;
        public UberEatsMenuTaxInfo Tax_info { get; set; } = null;
        public UberEatsMenuNutritionalInfo Nutritional_info { get; set; } = null;
        public UberEatsMenuDishInfo Dish_info { get; set; } = null;
        //public UberEatsMenuVisibilityInfo Visibility_info { get; set; } = null;
        //public UberEatsMenuTaxLabelsRuleSet Tax_label_info { get; set; } = null;
        //public UberEatsMenuProductInfo Product_info { get; set; } = null;
    }

    public class UberEatsMenuPriceRules
    {
        public UberEatsMenuPriceRules()
        {
            Overrides = new List<UberEatsMenuPriceOverride>();
        }
        public int Price { get; set; }
        public List<UberEatsMenuPriceOverride> Overrides { get; set; } = null;
    }

    public class UberEatsMenuBaseOverride
    {
        public string Context_type { get; set; } = null; //enum
        public string Context_value { get; set; } = null;
    }

    public class UberEatsMenuPriceOverride: UberEatsMenuBaseOverride
    {
        
        public int Price { get; set; }
    }

    public class UberEatsMenuQuantityConstraintRules
    {
        public UberEatsMenuQuantityConstraintRules()
        {
            Quantity = new UberEatsMenuQuantityConstraint();
            Overrides = new List<UberEatsMenuQuantityConstraintOverride>();
        }
        public UberEatsMenuQuantityConstraint Quantity { get; set; } = null;
        public List<UberEatsMenuQuantityConstraintOverride> Overrides { get; set; } = null;
    }

    public class UberEatsMenuQuantityConstraint
    {
        public int Min_permitted { get; set; }
        public int Max_permitted { get; set; }
        public bool Is_min_permitted_optional { get; set; }
        public int Charge_above { get; set; }
        public int Refund_under { get; set; }
        public int Min_permitted_unique { get; set; }
        public int Max_permitted_unique { get; set; }

    }

    public class UberEatsMenuQuantityConstraintOverride : UberEatsMenuBaseOverride
    {
        public UberEatsMenuQuantityConstraintOverride()
        {
            Quantity = new UberEatsMenuQuantityConstraint();
        }
        public UberEatsMenuQuantityConstraint Quantity { get; set; } = null;
    }

    public class UberEatsMenuSuspensionRules
    {
        public UberEatsMenuSuspensionRules()
        {
            Suspension = new UberEatsMenuSuspension();
            Overrides = new List<UberEatsMenuSupensionOverride>();
        }
        public UberEatsMenuSuspension Suspension { get; set; } = null;
        public List<UberEatsMenuSupensionOverride> Overrides { get; set; } = null;
    }

    public class UberEatsMenuSuspension
    {
        public long Suspend_until { get; set; }
        public string Reason { get; set; } = null;
    }

    public class UberEatsMenuSupensionOverride : UberEatsMenuBaseOverride
    {
        public UberEatsMenuSupensionOverride()
        {
            Suspension = new UberEatsMenuSuspension();
        }
        public UberEatsMenuSuspension Suspension { get; set; } = null;
    }

    public class UberEatsMenuModifierGroupsRules
    {
        public UberEatsMenuModifierGroupsRules()
        {
            Overrides = new List<UberEatsMenuModifierGroupsOverride>();
        }
        public string[] Ids { get; set; } = null;
        public List<UberEatsMenuModifierGroupsOverride> Overrides { get; set; } = null;
    }

    public class UberEatsMenuModifierGroupsOverride : UberEatsMenuBaseOverride
    {
        public string[] Ids { get; set; } = null;
    }

    public class UberEatsMenuTaxInfo
    {
        public float Tax_rate { get; set; }
        public float Vat_rate_percentage { get; set; }
    }

    public class UberEatsMenuNutritionalInfo
    {
        public UberEatsMenuNutritionalInfo()
        {
            Calories = new UberEatsMenuEnergyInfo();
            Kilojoules = new UberEatsMenuEnergyInfo();
        }
        public UberEatsMenuEnergyInfo Calories { get; set; } = null;
        public UberEatsMenuEnergyInfo Kilojoules { get; set; } = null;
    }

    public class UberEatsMenuEnergyInfo
    {
        public int Lower_range { get; set; }
        public int Upper_range { get; set; }
        public string Display_type { get; set; } = null; //enum
    }

    public class UberEatsMenuDishInfo
    {
        public UberEatsMenuDishInfo()
        {
            Classifications = new UberEatsMenuClassifications();
        }
        public UberEatsMenuClassifications Classifications { get; set; } = null;
    }

    public class UberEatsMenuClassifications
    {
        public UberEatsMenuClassifications()
        {
            Dietary_label_info = new UberEatsMenuDietaryLabelInfo();
        }
        public bool Can_serve_alone { get; set; }
        public bool Is_vegetarian { get; set; }
        public int Alcoholic_items { get; set; }
        public UberEatsMenuDietaryLabelInfo Dietary_label_info { get; set; } = null;
    }

    public class UberEatsMenuDietaryLabelInfo
    {
        public string[] Labels { get; set; } = null; //developer.uber.com/docs/eats/api/v2/put-eats-stores-storeid-menus#request-body-parameters-dietarylabels
    }

    public class UberEatsMenuVisibilityInfo
    {
        public UberEatsMenuVisibilityInfo()
        {
            Hours = new UberEatsMenuVisibilityHours();
        }
        public UberEatsMenuVisibilityHours Hours { get; set; } = null;
    }

    public class UberEatsMenuVisibilityHours
    {
        public UberEatsMenuVisibilityHours()
        {
            Hours_of_week = new UberEatsServiceAvailability();
        }
        public string Start_date { get; set; } = null;
        public string End_date { get; set; } = null;
        public UberEatsServiceAvailability Hours_of_week { get; set; } = null;
    }

    public class UberEatsMenuTaxLabelsRuleSet
    {
        public UberEatsMenuTaxLabelsRuleSet()
        {
            Default_value = new UberEatsMenuTaxLabelsInfo();
        }
        public UberEatsMenuTaxLabelsInfo Default_value { get; set; } = null;
    }

    //https://developer.uber.com/docs/eats/api/v2/put-eats-stores-storeid-menus#request-body-parameters-taxlabelsruleset
    public class UberEatsMenuTaxLabelsInfo
    {
        public string[] Labels { get; set; } = null; //enum
        public string Source { get; set; } = null; //enum
    }

    //https://developer.uber.com/docs/eats/api/v2/put-eats-stores-storeid-menus#request-body-parameters-productinfo
    public class UberEatsMenuProductInfo
    {
        public int Target_market { get; set; }
        public string Gtin { get; set; } = "";
        public string Plu { get; set; } = "";
        public string Merchant_id { get; set; } = "";
    }

    public class UberEatsModifierGroupsForMenu : UberBaseMenu
    {
        public UberEatsModifierGroupsForMenu()
        {
            Quantity_info = new UberEatsMenuQuantityConstraintRules();
            Modifier_options = new List<MenuEntity>();
        }
        public UberEatsMenuQuantityConstraintRules Quantity_info { get; set; } = null;
        public List<MenuEntity> Modifier_options { get; set; } = null;
        public string Display_type { get; set; } = null; //enum
    }

    public class UberEatsMenuUpdate
    {
        public UberEatsMenuUpdate()
        {
            Suspension_info = new UberEatsMenuSupensionOverride();
            //Price_info = new UberEatsMenuPriceRules();
        }

        //public UberEatsMenuPriceRules Price_info { get; set; } = null;
        public UberEatsMenuSupensionOverride Suspension_info { get; set; } = null;
        //public string Menu_type { get; set; } = null;
    }

}
