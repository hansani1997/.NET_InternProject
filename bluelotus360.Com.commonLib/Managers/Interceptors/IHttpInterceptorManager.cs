using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace bluelotus360.Com.commonLib.Managers.Interceptors
{
	public  interface IHttpInterceptorManager:IManager
	{
		void RegisterEvent();

		Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

		void DisposeEvent();
	}
}
