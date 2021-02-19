using System;
using Microsoft.Extensions.DependencyInjection;

namespace Function
{
	public static class Startup
	{
		//Install your custom dependencies here
		public static void ConfigureServices(this IServiceCollection services)
		{
			services.AddTransient<TypedHandler>();
		}

	}
}