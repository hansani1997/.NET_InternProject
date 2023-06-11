using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.OrderPages
{
    public partial class SalesOrder
    {
        private BLUIElement formDefinition;
        private long elementKey;
        private OrderComponent orderComponent;
        private IOrderValidator validator;
        Order order;
        //private TerlrikReportOptions _salesOrderReportOption;
        private IDictionary<string, bool> _conditionLogic;
        private IDictionary<string, EventCallback> _orderLogic;

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            orderComponent = new();
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);

            InitilalizeQuotation();

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
            }


            //_salesOrderReportOption.ReportName = "SalesOrder_MMN.trdp";
            //_salesOrderReportOption.ReportParameters = new Dictionary<string, object>();
        }

        private void InitilalizeQuotation()
        {
            order = new Order();
            _conditionLogic = new Dictionary<string, bool>();
            _orderLogic = new Dictionary<string, EventCallback>();
            validator = new SalesOrderValidator(order);
            //_salesOrderReportOption = new TerlrikReportOptions();
        }
    }
}
