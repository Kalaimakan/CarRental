using CarRental.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CarRental.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail = "info.easyriderentals@gmail.com";
        private readonly string _appPassword = "ihrqtxcyqzwdxnnf";
        public void SendEmail(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_fromEmail, _appPassword),
                EnableSsl = true
            };

            smtp.Send("info.easyriderentals@gmail.com", to, subject, body);
        }
    }
}
