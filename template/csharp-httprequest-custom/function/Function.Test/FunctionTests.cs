using System.Threading.Tasks;
using NUnit.Framework;

namespace Function.Test
{
	public class FunctionTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		async public Task Test1()
		{
			var sut = CreateTestObject();

			var result = await sut.Handle(new Input() {Data = "Test"});

			Assert.That(result, Is.EqualTo(new Output(){Data = "Your input was: Test"}));
		}

		private TypedHandler CreateTestObject()
		{
			return new TypedHandler();
		}
	}
}