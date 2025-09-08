using MediatR;
using Product.Application.Notifications;
using Product.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Notifications
{
    public class ProductCreatedNotification : BaseNotification
    {
        public Product.Core.Entities.Product Product { get; set; }

        public ProductCreatedNotification(Core.Entities.Product product)
        {
            Product = product;
        }     
    }
    public class ProductCreatedNotificationHandler : INotificationHandler<ProductCreatedNotification>
    {
        public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
