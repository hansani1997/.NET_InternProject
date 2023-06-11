using ApexCharts;
using BlueLotus360.CleanArchitecture.Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Chart.BarChart
{
    public partial class BLBarChart<T> :ComponentBase,IDisposable where T : class
    {
        private ApexChart<T> _detailsChart;
        private SelectedData<T> selectedData;
        private PropertyConversionResponse<T> conversionInfoX;
        private PropertyConversionResponse<T> conversionInfoY;
        [Parameter] public IList<T> ChartDetails { get; set; }
        [Parameter] public string XVal { get; set; }
        [Parameter] public string YVal { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Name { get; set; }
        private string xval;
        private decimal yval;
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            conversionInfoX = ChartDetails.GetPropObject<T>(XVal);
            if (conversionInfoX.IsConversionSuccess)
            {
                xval = conversionInfoX.Value.ToString();
            }

            conversionInfoY = ChartDetails.GetPropObject<T>(YVal);
            if (conversionInfoY.IsConversionSuccess)
            {
                yval = Convert.ToDecimal(conversionInfoY.Value);
            }
            return base.OnParametersSetAsync();

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        #region chart reigon
        private void DataPointsSelected(SelectedData<T> selectedData)
        {
            this.selectedData = selectedData;
            _detailsChart?.SetRerenderChart();
        }
        #endregion
    }
}
