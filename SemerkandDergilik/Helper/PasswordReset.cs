using System.Net;
using System.Net.Mail;

namespace Semerkand_Dergilik.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link, string email)
        // link HomeController'da ResetPassword action'da oluşturulacak..
        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpClient = new SmtpClient(); // parametre host firmasından alınır..

            mail.From = new MailAddress("twittercavalryman@gmail.com"); // host'a bağlı email adresiniz ve şifreniz..
            mail.To.Add(email); // birden fazla kullanıcıya send edilebilir

            mail.Subject = $"Şifre sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("twittercavalryman@gmail.com", "twitter<*4063");
            // smtpClient.EnableSsl = true;
             smtpClient.Host = "smtp.gmail.com";
            // smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            // smtpClient.UseDefaultCredentials = false;

            smtpClient.Send(mail);

            //MailAddress senderEmail = new MailAddress("yms34243423@gmail.com");
            //MailAddress receiverEmail = new MailAddress(email);

            /*
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
            */
        }
    }
}
