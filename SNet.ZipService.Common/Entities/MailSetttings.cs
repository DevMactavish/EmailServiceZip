using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNet.ZipService.Common.Entities
{
    public class MailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public int Timeout { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
