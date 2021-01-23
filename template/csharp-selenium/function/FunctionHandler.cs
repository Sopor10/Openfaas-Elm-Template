using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using PuppeteerSharp;

namespace Function
{
    public class FunctionHandler
    {
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var reader = new StreamReader(request.Body); 
            var input = JsonSerializer.Deserialize<Input>(await reader.ReadToEndAsync());
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox" }
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://www.google.com");
            string outputFile = null;
            await page.ScreenshotAsync(outputFile);
            return (200, outputFile);
        }
    }
    public record Input
    {
        public string Url { get; init; }
    }
}