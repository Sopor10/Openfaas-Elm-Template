using Microsoft.AspNetCore.Http;
using System.IO;
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
            using IWebDriver driver = new FirefoxDriver(System.Environment.CurrentDirectory,firefoxOptions);

            driver.Navigate().GoToUrl(input.Url);
            
            return (200, driver.PageSource);
        }
    }
    public record Input
    {
        public string Url { get; init; }
    }
}