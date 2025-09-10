using Product.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core;

namespace Product.Application.Notifications
{
    public class ProductDeletedNotification : BaseNotification
    {
        public Product.Core.Entities.Product Product { get; set; }

        public ProductDeletedNotification(Core.Entities.Product product)
        {
            Product = product;
        }
    }
    public class ProductDeletedNotificationHandler : INotificationHandler<ProductDeletedNotification>
    {
        public Task Handle(ProductDeletedNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
