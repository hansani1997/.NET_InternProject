using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.WorkShopManagement
{
    public class InsurenceOrder : WorkOrder
    {
        public IList<IRNDetails> IRNDetail { get; set; }

		//public InsurenceOrder() 
  //      {
  //          IRNDetail = new List<IRNDetails>();

  //      }
    }
    public class IRNDetails 
    {
		public bool ShowDetails { get; set; }
		public string ServiceAdvisor { get; set; } = "";
		public string IRNNumber { get; set; } = "";
		public string VehicleNumber { get; set; } = "";
        public string Branch { get; set; } = "";
        public string IRNType { get; set; } = "";
        public DateTime CreationDateAndTime { get; set; } = DateTime.Now;
        public AccountResponse Insurence { get; set; } = new AccountResponse();
		public IList<Particulars> Particulars { get; set; } = new List<Particulars>();
	}
    public class Particulars 
    {
        public string ServiceOrMaterial { get; set; } = "";
        public decimal Amount { get; set; }

	}
}
