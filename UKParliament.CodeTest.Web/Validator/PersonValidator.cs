#region Namespace References
using FluentValidation;
using System.Linq;
using UKParliament.CodeTest.Data.Model;
#endregion

namespace UKParliament.CodeTest.Web.Validator
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} should be not empty")
                .Must(IsLetter)
                .WithMessage("{PropertyName} should be all letters.");

            RuleFor(p => p.Email)
                .EmailAddress();

            RuleFor(p => p.Phone)
                .Must(IsNumber)
                .WithMessage("{PropertyName} should be numeric value only.");

            RuleFor(p=>p.Gender)
                .Must(IsLetter)
                .WithMessage("{PropertyName} should be all letters.");
        }

        private bool IsLetter(string value)
        {
            return value.All(char.IsLetter);
        }

        private bool IsNumber(string value)
        {
            return value.All(char.IsDigit);
        }
    }
}
