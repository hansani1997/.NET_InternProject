using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Collections.Generic;
using BL10.CleanArchitecture.Domain;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.CleanArchitecture.Domain;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid
{
	public partial class BLTelerikKendoGrid<TItem> : ComponentBase, IBLUIOperationHelper
	{
		[Parameter]
		public IList<TItem> DataObject { get; set; }

		[Parameter]
		public BLUIElement FormObject { get; set; }

		[Parameter]
		public IDictionary<string, EventCallback> InteractionLogics { get; set; }

		[Parameter]
		public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

		[Parameter]
		public string Height { get; set; }
		private int filterDebounceDelay { get; set; } = 200;
		TelerikGrid<TItem> GridRef { get; set; }
		public BLUIElement LinkedUIObject => throw new System.NotImplementedException();
		List<TItem> dataList { get; set; }
		private IDictionary<string, decimal> TotalValues { get; set; }
		private decimal[] GridVal;
		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			TotalValues = new Dictionary<string, decimal>();
			//ReArrangeElements();
		}

		protected override void OnInitialized()
		{
			dataList = new List<TItem>();
			dataList = DataObject.ToList<TItem>();
			base.OnInitialized();
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{

			EventCallback callBack;

			if (InteractionLogics.TryGetValue("OnGridInitilize_" + FormObject._internalElementName, out callBack))
			{
				if (callBack.HasDelegate)
				{
					callBack.InvokeAsync();
				}
			}

			return base.OnAfterRenderAsync(firstRender);


		}

		private RenderFragment<object> GetFooterTemplate(string propName)
		{
			RenderFragment<object> ColumnTemplate = context => __builder =>
			{
				PropertyInfo propertyInfo = context.GetType().GetProperty(propName);

				var propType = propertyInfo.PropertyType;

				var propValue = propertyInfo.GetValue(context);

				if (propType == typeof(int))
				{

				}
				else
				{

				}
			};

			return ColumnTemplate;
		}

		private string SetGridValues(Object obj, string name)
		{
			string column = "";
			if (name == null || name.Trim().Length < 2)
			{
				column = String.Empty;

			}

			Type type = obj.GetType();

			foreach (string part in name.Split('.'))
			{
				if (obj == null) { column = String.Empty; }


				PropertyInfo info = type.GetProperty(part);

				if (info == null)
				{
					column = String.Empty;
				}

				else if ((info?.GetValue(obj, null)).IsNumericType())
				{
					column = decimal.Parse(info?.GetValue(obj, null).ToString()).ToString("N2");
				}
				else
				{
					column = info?.GetValue(obj, null).ToString();
				}


				type = info?.PropertyType;
				obj = info?.GetValue(obj, null);
			}
			return column;
		}

		private Type GetBlGridColumnType(string propName, object context)
		{
			Type t = typeof(string);

			if (!string.IsNullOrEmpty(propName))
			{
				PropertyInfo propertyInfo = context?.GetType()?.GetProperty(propName);

				if (propertyInfo != null)
				{
					t = propertyInfo.PropertyType;

					var propValue = propertyInfo.GetValue(context);


				}
			}



			return t;
		}

		private void ReArrangeElements()
		{
			var childsHash = FormObject.Children.ToLookup(elem => elem.ParentKey);
			foreach (var child in FormObject.Children)
			{
				child.Children = childsHash[child.ElementKey].ToList();
			}
			BLUIElement form = FormObject.Children.Where(x => x.ElementKey == FormObject.ElementKey).FirstOrDefault();
			FormObject = form;

		}

		private void OnStateInit(GridStateEventArgs<TItem> args)
		{
			//    args.GridState.GroupDescriptors = new List<TItem>()
			//{
			//    new GroupDescriptor()
			//    {
			//        Member = nameof(ProductDto.CategoryId),
			//        MemberType = typeof(int)
			//    }
			//};
		}

		public void ResetToInitialValue()
		{
			throw new System.NotImplementedException();
		}

		public void UpdateVisibility(bool IsVisible)
		{
			throw new System.NotImplementedException();
		}

		public void ToggleEditable(bool IsEditable)
		{
			throw new System.NotImplementedException();
		}

		public async Task Refresh()
		{
			GridRef?.Rebind();
			StateHasChanged();
			await Task.CompletedTask;
		}

		public Task FocusComponentAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task SetValue(object value)
		{
			throw new System.NotImplementedException();
		}

	}
}
