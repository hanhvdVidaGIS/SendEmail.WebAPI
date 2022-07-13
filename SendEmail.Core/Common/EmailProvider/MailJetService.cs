using Mailjet.Client;
using Newtonsoft.Json.Linq;
using SendEmail.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail.Core.Common.EmailProvider
{
    internal class MailJetService : MailSender.MailSender, IMailSender
    {
        protected override async Task Send(EmailModel.EmailModel email)
        {
            try
            {
                JArray jArray = new JArray();
                JArray attachments = new JArray();
                if (email.Attachments != null && email.Attachments.Count() > 0)
                {

                    email.Attachments.ToList().ForEach(attachment =>
                    attachments.Add(
                        new JObject {
                            new JProperty("Content-type",attachment.ContentType),
                            new JProperty( "Filename",attachment.Name),
                            new JProperty("content",Convert.ToBase64String(attachment.Data))
                    }));
                }

                jArray.Add(new JObject {
                            {
                            "FromEmail",
                            "hanhvuvd@gmail.com"
                            }, {
                            "FromName",
                            "HanhVu"
                            },
                            {
                                "Recipients",
                                new JArray {
                                    new JObject {
                                    {
                                        "Email",
                                        email.EmailAddress
                                    }, {
                                        "Name",
                                       email.EmailAddress
                                    }
                                    }
                                }
                            }, 
                            {
                                "Subject",
                                email.Subject
                            }, 
                            {
                                "Text-part",
                                email.Body
                            }, 
                            {
                                "Html-part",
                                email.Body
                            },  
                            {
                                "Attachments",
                                attachments
                            }
                });

                var client = MailSender.MailSender.CreateMailJetV3Client();
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Mailjet.Client.Resources.Send.Resource,
                }
                 .Property(Mailjet.Client.Resources.Send.Messages, jArray);

                var response = await client.PostAsync(request);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
