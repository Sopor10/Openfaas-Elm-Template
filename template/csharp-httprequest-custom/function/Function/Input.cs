using FluentValidation;

namespace Function
{
	public record Input
	{
		public string Data { get; init; }
	}

	public class InputValidator : AbstractValidator<Input>
	{
		public InputValidator()
		{
			RuleFor(x => x.Data).NotNull().NotEmpty();
		}
	}
}