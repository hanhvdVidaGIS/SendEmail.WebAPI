using SendEmail.Core.Common.EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Core.Interfaces
{
    public interface IMailSender
    {
        Task SendEmail(string address, string subject, string text);
        Task SendEmail(EmailModel email);
    }
}
