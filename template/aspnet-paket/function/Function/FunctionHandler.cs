using System;
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


		public async Task<String> Handle(HttpRequest request)
		{
			return "Hello World";
		}
	}
}