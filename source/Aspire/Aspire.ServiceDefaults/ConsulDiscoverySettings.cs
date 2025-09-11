using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.ServiceDefaults
{
    public class ConsulDiscoverySettings
    {
        public string ServiceName { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
    }
}
