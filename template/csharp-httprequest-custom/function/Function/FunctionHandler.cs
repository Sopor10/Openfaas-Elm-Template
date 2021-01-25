using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Function
{
    public class FunctionHandler
    {
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var reader = new StreamReader(request.Body);
            var input = await reader.ReadToEndAsync();
            
            var deserialize = JsonSerializer.Deserialize<Input>(input);
            var output = await this.Handle(deserialize);
            return (200, JsonSerializer.Serialize(output ));
        }

        public async Task<Output> Handle(Input input) 
        {
	        input = input ?? throw new ArgumentNullException();
	        return new Output() {Data = "Your input was: " + input.Data};
        }
    }
}