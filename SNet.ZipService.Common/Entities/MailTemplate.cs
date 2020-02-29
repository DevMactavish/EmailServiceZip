using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNet.ZipService.Common.Entities
{
    public class MailTemplate
    {
        public string Link { get; set; }
        public List<string> Emails { get; set; }
    }
}
