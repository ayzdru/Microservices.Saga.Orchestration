using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(Constants.Product.NameMaximumLength)
                .MinimumLength(Constants.Product.NameMinimumLength)
                .NotEmpty();

            RuleFor(v => v.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than zero.");

            RuleFor(v => v.Stock)
                .GreaterThan(0)
                .WithMessage("Product stock must be greater than zero.");
        }
    }
}
