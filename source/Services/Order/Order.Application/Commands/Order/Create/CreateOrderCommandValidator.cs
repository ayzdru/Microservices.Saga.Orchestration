using FluentValidation;
using Order.Application.Commands.Order.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Application.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty.")
                .Must(id => id != Guid.Empty)
                .WithMessage("UserId cannot be an empty GUID.");

            RuleFor(x => x.OrderItems)
                .NotNull()
                .WithMessage("OrderItems cannot be null.")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("Order must contain at least one OrderItem.")
                .Must(items => items.Select(i => i.ProductId).Distinct().Count() == items.Count)
                .WithMessage("Each ProductId in OrderItems must be unique.");

            RuleForEach(x => x.OrderItems)
                .ChildRules(orderItem =>
                {
                    orderItem.RuleFor(i => i.Count)
                        .GreaterThan(1)
                        .WithMessage("OrderItem Count must be greater than 1.");
                });
        }
    }
}
