using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms
{
    public class OrderMenuConfiguration
    {
        public OrderMenuConfiguration()
        {
            Location = new CodeBaseResponse();
            Item = new ItemResponse();
            Platforms = new AccountResponse();
        }
        public int PlatformConfigKey { get; set; }
        public CodeBaseResponse Location { get; set; }
        public ItemResponse Item { get; set; }
        public AccountResponse Platforms { get; set; }
    }
}
