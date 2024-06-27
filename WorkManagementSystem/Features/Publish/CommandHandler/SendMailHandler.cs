using System.Net.Mail;

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

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtpServer);
            mail.From = new MailAddress(smtpUser);
            mail.To.Add(command.toEmail);
            mail.Subject = command.subject;
            mail.Body = command.body;

            if (command.FileNames.Count > 0)
            {
                var lst = new List<FileInfo>();

                var dirUpload = _config.GetValue("DirUpload", "");
                foreach (var item in command.FileNames)
                {
                    if (!Directory.Exists(dirUpload))
                    {
                        Directory.CreateDirectory(dirUpload);
                    }
                    try
                    {
                        var filePath = dirUpload + "\\" + item;
                        if (System.IO.File.Exists(filePath))
                        {

                            Attachment attachment = new Attachment(filePath);
                            mail.Attachments.Add(attachment);
                        }
                        else
                        {
                            throw new Exception("File không tồn tại: " + filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            return false;
        }
    }

}