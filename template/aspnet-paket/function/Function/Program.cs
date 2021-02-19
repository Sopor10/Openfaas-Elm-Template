using Function;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

WebHost.CreateDefaultBuilder(args)
	.ConfigureServices(Startup.ConfigureServices)
	.UseUrls("http://localhost:5000")
	.Configure(app =>
	{
		app.UseRouting();

		app.UseEndpoints(e =>
		{
			var handler = e.ServiceProvider.GetRequiredService<FunctionHandler>();

			e.MapPost("/", async c => await c.Response.WriteAsJsonAsync(await handler.Handle(c.Request)));
		});
	})
	.Build()
	.Run();