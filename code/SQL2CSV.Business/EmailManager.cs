using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SQL2CSV.Business
{
    public class EmailManager:SQL2CSV.Interfaces.IEmailManager
    {
        private readonly Interfaces.Models.EmailSettings _Settings;
        private readonly SmtpClient _SmtpServer;

        public EmailManager(Interfaces.Models.EmailSettings settings)
        {
            _Settings = settings;
            _SmtpServer = new SmtpClient();
        }

        public void SendEmail(string fileName, string compressedFilename)
        {
            if (!_Settings.SendMail)
            {
                return;
            }

            MailMessage mail = new MailMessage();
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.To.Add(_Settings.MailTo);
            mail.Subject = _Settings.MailSubjectPrefix + fileName;
            mail.Body = _Settings.MailBody;
            mail.Attachments.Add(new Attachment(compressedFilename));

            _SmtpServer.Send(mail);

            Console.WriteLine("Mail sent");
        }
    }
}
