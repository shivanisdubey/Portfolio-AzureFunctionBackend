using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace PortfolioFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IConfiguration _config;

        public Function1(ILogger<Function1> logger,IConfiguration config)
        {
            _logger = logger;
            _config= config;
        }

        [Function("ContactEmailFunction")] //function Name
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req) //triggers on post request
        {
            var requestBody = await req.ReadAsStringAsync();

            // Deserialize to extract fields
            var data = JsonSerializer.Deserialize<ContactRequest>(requestBody);

            string messageContent = //plain text content if html body not supported
                $"Name: {data.Name}\n" +
                $"Email: {data.Email}\n" +
                $"Message:\n{data.Message}";

            string emailContent = //html body
                $@"<p><b>Name:</b> {data.Name}</p>
                <p><b>Email:</b>{data.Email}</p>
                <p><b>Message:</b><br/>{data.Message}</p>";

            _logger.LogInformation("Email: " + data.Email);

            await SendEmail(data.Email,messageContent,emailContent);

            // Return response
            var response = req.CreateResponse(HttpStatusCode.OK); //status 200 if success
            await response.WriteStringAsync("Mail sent successfully!");
            return response;
        }


        private async Task SendEmail(string userEmail, string messageContent,string emailContent)
        {
           
            // Read API key from environment variable
            var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            var client = new SendGridClient(apiKey);

            // Email details
            var from = new EmailAddress(_config["FromEmail"], "Portfolio Contact"); //verified mail from SendGrid (Setting->Sender authentication)
            var to = new EmailAddress(_config["FromEmail"]); 

            var msg = MailHelper.CreateSingleEmail(
           from, //email from (sender)
           to, //email to (receiver)
           "New Message from Your Portfolio", //Subject
           messageContent, //plain text (fallback if HTML not supported)
           emailContent //email body (html content)
           );

            msg.MailSettings = new MailSettings
            {
                SandboxMode = new SandboxMode { Enable = false }  // MUST be false
            };

            
            msg.ReplyTo = new EmailAddress(userEmail); //reply to mail -> user email
            var response = await client.SendEmailAsync(msg);  //send mail using sendgrid

            _logger.LogInformation("SendGrid Status: " + response.StatusCode);

            

        }

        public class ContactRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
        }
    }
}
