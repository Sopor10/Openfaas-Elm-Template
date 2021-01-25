using System;
using System.Threading.Tasks;

namespace Function
{
	public class TypedHandler
	{
		public async Task<Output> Handle(Input input)
		{
			input = input ?? throw new ArgumentNullException();
			return new Output() { Data = "Your input was: " + input.Data };
		}
	}
}