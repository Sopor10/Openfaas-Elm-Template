using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Function
{
	public class FunctionHandler
	{
		public FunctionHandler()
		{
		}


		public async Task<IActionResult> Handle(HttpRequest request)
		{
			return new OkObjectResult("Hello World");
		}
	}
}