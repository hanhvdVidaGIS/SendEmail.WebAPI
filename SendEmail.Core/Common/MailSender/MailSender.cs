using Mailjet.Client;
using SendEmail.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Core.Common.MailSender
{
    public abstract class MailSender : IMailSender
    {
        public static MailjetClient CreateMailJetV3Client()
        {
            return new MailjetClient("a41177c23f85f72e038691079da11fed", "2daa83293610589526a160f14391ec9f");
        }
        protected abstract Task Send(EmailModel.EmailModel email);
        public Task SendEmail(string address, string subject, string text)
        {
            return SendEmail(new EmailModel.EmailModel { EmailAddress = address, Subject = subject, Body = text });
        }

        public Task SendEmail(EmailModel.EmailModel email)
        {
            return Send(email);
        }
    }

}
