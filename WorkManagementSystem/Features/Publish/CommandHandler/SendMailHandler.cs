using System.Net.Mail;
using System.Net;

namespace WorkManagementSystem.Features.Publish.CommandHandler
{
    public class SendMailHandler : ICommandHandler<SendEmailCommand, bool>
    {
        public async Task<bool> ExecuteAsync(SendEmailCommand command, CancellationToken ct)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; // Cổng SMTP thường là 587 hoặc 25
            string smtpUser = "hoangpham19112002@gmail.com";
            string smtpPass = "jdsgjorovsxmjjro";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("your mail@gmail.com");
                mail.To.Add("to_mail@gmail.com");
                mail.Subject = "Test Mail - 1";
                mail.Body = "mail with attachment";

                if (command.Files.Count > 0)
                {
                    var lst = new List<FileInfo>();

                    var dirUpload = @"C:\Project\FileManagerService\Output\2023\file";
                    foreach (var item in command.Files)
                    {
                        if (!Directory.Exists(dirUpload))
                        {
                            Directory.CreateDirectory(dirUpload);
                        }
                        var filePath = dirUpload + "\\" + item.FileName;
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }
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
                Console.WriteLine("Failed to send email. Error: " + ex.Message);
                throw;
            }
        }



    }



}