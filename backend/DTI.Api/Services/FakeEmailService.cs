using System;
using System.IO;

namespace DTI.Api.Services
{
    public class FakeEmailService
    {
        private readonly string _notificationsPath;

        public FakeEmailService()
        {
            _notificationsPath = Path.Combine(Directory.GetCurrentDirectory(), "Notifications");

            if (!Directory.Exists(_notificationsPath))
                Directory.CreateDirectory(_notificationsPath);
        }

        public void SendEmail(string to, string subject, string body)
        {
            var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_email.txt";
            var filePath = Path.Combine(_notificationsPath, fileName);

            var content = $"To: {to}\nSubject: {subject}\n\n{body}";
            File.WriteAllText(filePath, content);
        }
    }
}
