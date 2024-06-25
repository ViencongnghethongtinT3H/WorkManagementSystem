using System.Net.Mail;
using System.Net;

namespace WorkManagementSystem.Features.Publish.CommandHandler
{
    public class SendMailHandler : ICommandHandler<SendEmailCommand, bool>
    {
        private readonly IConfiguration _config;

        public SendMailHandler(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> ExecuteAsync(SendEmailCommand command, CancellationToken ct)
        {
            string smtpServer = _config.GetValue("EmailSettings:SmtpServer", "");
            var smtpPort = Int32.Parse(_config.GetValue("EmailSettings:SmtpPort", "")); // Cổng SMTP thường là 587 hoặc 25
            string smtpUser = _config.GetValue("EmailSettings:SmtpUser", "");
            string smtpPass = _config.GetValue("EmailSettings:SmtpPass", ""); 
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtpServer);
                mail.From = new MailAddress(smtpUser);
                mail.To.Add(command.toEmail);
                mail.Subject = command.subject;
                mail.Body = command.body;

                if (command.Files.Count > 0)
                {
                    var lst = new List<FileInfo>();

                    var dirUpload = _config.GetValue("DirUpload", "");
                    foreach (var item in command.Files)
                    {
                        if (!Directory.Exists(dirUpload))
                        {
                            Directory.CreateDirectory(dirUpload);
                        }
                        var filePath = dirUpload + "\\" + item.FileName;
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }
                }


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }



}