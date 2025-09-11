using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.ServiceDefaults
{
    public class ConsulSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Scheme { get; set; }
        public ConsulDiscoverySettings Discovery { get; set; }
    }
}
