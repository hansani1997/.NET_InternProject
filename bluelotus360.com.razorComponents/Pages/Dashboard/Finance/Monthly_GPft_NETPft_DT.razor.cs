using ApexCharts;
using bluelotus360.com.razorComponents.MB.Shared.Components.Chart;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Dashboard.Finance
{
    public partial class Monthly_GPft_NETPft_DT : ComponentBase
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter] public IList<GPft_NetPft_DT> Monthly_GPft_NETPft { get; set; }

        private ApexChart<GPft_NetPft_DT> _chart;
        EmptyChart empty_chart;
        private ApexChartOptions<GPft_NetPft_DT> options;

        protected override async Task OnParametersSetAsync()
        {
            empty_chart = new();
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            options = new ApexChartOptions<GPft_NetPft_DT>
            {
                Colors = new List<string> { "rgb(255, 0, 117)", "rgb(23, 39, 116)", "rgb(0, 122, 255)", "rgb(240, 165, 0)", "rgb(251, 54, 64)", "rgb(40, 167, 69)" },
                Markers = new Markers { Shape = ShapeEnum.Circle, Size = 5 },
                Stroke = new Stroke { Curve = Curve.Smooth },
            };
            options.Yaxis = new List<YAxis>();
            options.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            RefreshChart();
            LoadChart();
            await base.OnInitializedAsync();
        }


        private async void RefreshChart()
        {
            _chart = new();

            await Task.CompletedTask;
        }

        private async void LoadChart()
        {
            _chart?.SetRerenderChart();
        }

        private void Back()
        {
            MudDialog?.Close();
        }
    }
}
