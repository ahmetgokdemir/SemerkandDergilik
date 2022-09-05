using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;

namespace Semerkand_Dergilik.Helper
{
    public class EmailConfirmation
    {
        private readonly EmailOptions _emailOptions;
        

        public EmailConfirmation(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
            
        }

        public string Send(string link,string emailAdress)
        {
            //string code = _twoFactorService.GetCodeVerification().ToString();

            Execute(link,emailAdress);
            return link;
        }

        private async Task Execute(string link, string emailAdress)
        {
            var client = new SendGridClient(_emailOptions.SendGrid_ApiKey);
            var from = new EmailAddress("ahmetgokdemirtc@gmail.com");
            var subject = "Email doğrulama";
            var to = new EmailAddress(emailAdress);
            var htmlContent = "<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            htmlContent += $"<a href='{link}'>email doğrulama linki</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        /*
        public static void SendEmail(string link, string email)

        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpClient = new SmtpClient("mail.teknohub.net");

            mail.From = new MailAddress("admin@teknohub.net");
            mail.To.Add(email);

            mail.Subject = $"www.bıdıbı.com::Email doğrulama";
            mail.Body = "<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>email doğrulama linki</a>";
            mail.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("admin@teknohub.net", "Fatih1234");

            smtpClient.Send(mail);
        }*/
    }
}
