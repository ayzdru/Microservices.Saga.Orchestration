using Orchestration.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orchestration.Core.Entities
{
    public class User : BaseEntity
    {
        public User(Guid id)
        {
            base.Id = id;
        }
    }
}
