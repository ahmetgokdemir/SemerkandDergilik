using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using System.Net.Mail;

namespace Technosoft_Project.Helper
{
 
        public class PasswordReset
        {
            private readonly EmailOptions _emailOptions;


            public PasswordReset(IOptions<EmailOptions> options)
            {
                _emailOptions = options.Value;

            }

            public string Send(string link, string emailAdress)
            {
                //string code = _twoFactorService.GetCodeVerification().ToString();

                Execute(link, emailAdress);
                return link;
            }

            private async Task Execute(string link, string emailAdress)
            {
                var client = new SendGridClient(_emailOptions.SendGrid_ApiKey);
                var from = new EmailAddress("ahmetgokdemirtc@gmail.com");
                var subject = "Şifre sıfırlama";
                var to = new EmailAddress(emailAdress);
                var htmlContent = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
                htmlContent += $"<a href='{link}'>şifre yenileme linki</a>";
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
       

        /*
        public static void PasswordResetSendEmail(string link, string email)
        // link HomeController'da ResetPassword action'da oluşturulacak..
        {
            //MailMessage mail = new MailMessage();

            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // parametre host firmasından alınır..
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.EnableSsl = true;
            //smtpClient.Credentials = new System.Net.NetworkCredential("twittercavalryman@gmail.com", "twitter<*4063");

            //mail.From = new MailAddress("twittercavalryman@gmail.com"); // host'a bağlı email adresiniz ve şifreniz..
            // mail.To.Add(email); // birden fazla kullanıcıya send edilebilir

            //mail.Subject = $"Şifre sıfırlama";
            //mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            //mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            //mail.IsBodyHtml = true;
            //smtpClient.Port = 587;
            //smtpClient.Credentials = new System.Net.NetworkCredential("twittercavalryman@gmail.com", "twitter<*4063");
            // smtpClient.EnableSsl = true;
            // smtpClient.Host = "smtp.gmail.com";
            // smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            // smtpClient.UseDefaultCredentials = false;

            //smtpClient.Send(mail);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("twittercavalryman@gmail.com"); // host'a bağlı email adresiniz ve şifreniz..
            mail.To.Add(email); // birden fazla kullanıcıya send edilebilir
            mail.Subject = $"Şifre sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("twittercavalryman@gmail.com", "twitter<*4063");
            // smtp.Port = 587;
            //Or your Smtp Email ID and Password
            smtp.Send(mail);

         
            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",    
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,   // varsayılan yetkilendirme ayarlarını kullanmayacağız.. Onun yerine aşağıdaki kodu customize ettik..
                Credentials = new NetworkCredential("ahmetgokdemirtc@gmail.com", "mdn<*4063")        // using System.Net;
            };

            MailMessage mail = new MailMessage();
                       

            mail.From = new MailAddress("yms34243423@gmail.com"); // host'a bağlı email adresiniz ve şifreniz..
            mail.To.Add(email);

            mail.Subject = $"Şifre sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            
            smtp.Send(mail);  // Mesaj gönderimi..
        
        }
        */
    }
}
