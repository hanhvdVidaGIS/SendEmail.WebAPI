using SendEmail.Core.Common.EmailModel;
using SendEmail.Core.HostedService;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add service
builder.Services.AddSingleton<EmailHostedService>();
builder.Services.AddHostedService(provider => provider.GetService<EmailHostedService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/test-email", async (EmailHostedService emailHostedService) =>
{
    await SendTestEmail(emailHostedService);
});

async Task SendTestEmail(EmailHostedService emailHostedService)
{
    await emailHostedService.SendEmailAsync(new EmailModel
    {
        EmailAddress = "hanhvd@vidagis.com",
        Subject = "Day la subject tu Hosted Service",
        Body = WebUtility.HtmlDecode("<h2>Email gui tu NET 6</h2>")
    });
}

app.Run();
