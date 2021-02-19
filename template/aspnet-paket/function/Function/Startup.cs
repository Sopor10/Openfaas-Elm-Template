using System;
using Microsoft.Extensions.DependencyInjection;

namespace Function
{
	public class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<FunctionHandler>();
		}
	}
}
