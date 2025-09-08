using Product.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Notifications
{
    public class ProductUpdatedNotification : BaseNotification
    {
        public Product.Core.Entities.Product Product { get; set; }
        public ProductUpdatedNotification(Core.Entities.Product product)
        {
            Product = product;
        }        
    }
    public class ProductUpdatedNotificationHandler : INotificationHandler<ProductUpdatedNotification>
    {
        public Task Handle(ProductUpdatedNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
