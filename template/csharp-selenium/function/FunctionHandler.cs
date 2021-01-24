using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Function
{
    public class FunctionHandler
    {
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var reader = new StreamReader(request.Body); 
            var input = JsonSerializer.Deserialize<Input>(await reader.ReadToEndAsync());
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArgument("-headless");
            using var driver = new FirefoxDriver(firefoxOptions);
            driver.Navigate().GoToUrl(input.Url);
            return (200, driver.PageSource);
        }

        private string GetGeckoDriverLink()
        {
	        var architecture = RuntimeInformation.OSArchitecture;

	        var fileLocation = architecture switch
	        {
		        Architecture.X64 => "/home/app/geckodriveramd64",
		        Architecture.Arm64 => "/home/app/geckodriverard64",
		        _ => throw new ArgumentOutOfRangeException()
	        };

	        return fileLocation;
        }
    }
    public record Input
    {
        public string Url { get; init; }
    }
}