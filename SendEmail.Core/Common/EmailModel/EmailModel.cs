using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Core.Common.EmailModel
{
    public class EmailModel
    {
        public string EmailAddress { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public IEnumerable<MyAttachment>? Attachments { get; set; }
    }
    public class MyAttachment
    {
        public string ContentType { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public byte[] Data { get; set; } = new byte[10];
    }
}
