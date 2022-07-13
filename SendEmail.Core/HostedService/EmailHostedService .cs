using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendEmail.Core.Common.EmailModel;
using SendEmail.Core.Common.EmailProvider;
using SendEmail.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SendEmail.Core.HostedService
{
    public class EmailHostedService : IHostedService, IDisposable
    {
        private Task? _sendTask;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly BufferBlock<EmailModel> _mailQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMailSender _mailSender;

        public EmailHostedService(IServiceScopeFactory serviceScopeFactory)
        {

            _mailSender = new MailJetService(); ;
            _serviceScopeFactory = serviceScopeFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _mailQueue = new BufferBlock<EmailModel>();
        }
        private void DestroyTask()
        {
            try
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = null;
                }
                Console.WriteLine("[EMAIL HOSTED SERVICE] DESTROY SERVICE");
            }
            catch
            {

            }
        }
        public void Dispose()
        {
            DestroyTask(); throw new NotImplementedException();
        }

        private async Task BackgroundSendEmailAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var email = await _mailQueue.ReceiveAsync();
                    await _mailSender.SendEmail(email);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[BACKGROUND EMAIL SERVICE] {ex.Message}", "EmailHostedService");
                }
                Console.WriteLine("[BACKGROUND EMAIL SERVICE] END SEND", "EmailHostedService");
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _sendTask = BackgroundSendEmailAsync(_cancellationTokenSource!.Token);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DestroyTask();
            await Task.WhenAny(_sendTask!, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        //can o dasu thi goi ham nay
        public async Task SendEmailAsync(EmailModel emailWithAddress)
        {
            await _mailQueue.SendAsync(emailWithAddress);
            Console.WriteLine($"SEND {emailWithAddress.EmailAddress}");
        }

    }
}
