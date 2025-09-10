using BuildingBlocks.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Entities
{
    public class User : BaseEntity
    {
        public User(Guid id)
        {
          base.Id = id;
        }
    }
}
