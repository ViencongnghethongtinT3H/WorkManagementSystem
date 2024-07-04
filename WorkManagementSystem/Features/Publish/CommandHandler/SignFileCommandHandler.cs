
namespace WorkManagementSystem.Features.Publish.CommandHandler
{
    public class SignFileCommandHandler : ICommandHandler<SignFileCommand, string>
    {
        public async Task<string> ExecuteAsync(SignFileCommand command, CancellationToken ct)
        {
            var url = command.FileUrl;
            var outputName = PdfSigner.AppendSuffixToFileName(command.FileName, "_signed");
            var outputDirectory = @"C:\Project\FileManagerService\Output\2023\file\signature";
            var outputFilePath = Path.Combine(outputDirectory, outputName);
            var signatureImage = Path.Combine(outputDirectory, "sign.png");
            // Tạo thư mục output nếu chưa tồn tại
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            try
            {
                await Task.Run(() => PdfSigner.InsertSignatureImage(url, outputFilePath, signatureImage));
                return $"https://file-manager.digins.vn/Output/2023/file/signature/{outputName}";
            }
            catch (Exception ex)
            {
                throw;
            }
          

        }
    }
}
