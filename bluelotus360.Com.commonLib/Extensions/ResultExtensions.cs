using BlueLotus360.Com.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Extensions
{
	internal static class ResultExtensions
	{
		internal static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
		{
			var responseAsString = await response.Content.ReadAsStringAsync();
			var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				ReferenceHandler = ReferenceHandler.Preserve
			});
			return responseObject;
		}

		internal static async Task<IResult> ToResult(this HttpResponseMessage response)
		{
			var responseAsString = await response.Content.ReadAsStringAsync();
			var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				ReferenceHandler = ReferenceHandler.Preserve
			});
			return responseObject;
		}

		internal static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
		{
			var responseAsString = await response.Content.ReadAsStringAsync();
			var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			return responseObject;
		}


		public static string GetDescription(this Enum e)
		{
			var attribute =
				e.GetType()
					.GetTypeInfo()
					.GetMember(e.ToString())
					.FirstOrDefault(member => member.MemberType == MemberTypes.Field)
					.GetCustomAttributes(typeof(DescriptionAttribute), false)
					.SingleOrDefault()
					as DescriptionAttribute;

			return attribute?.Description ?? e.ToString();
		}

	}
}
