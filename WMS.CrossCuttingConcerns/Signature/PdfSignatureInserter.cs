using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;
using System.Security.Cryptography.X509Certificates;

public class PdfSigner
{
    public static void InsertSignatureImage(string inputPdfPath, string outputPdfPath, string signatureImagePath)
    {
        try
        {
            // Đọc tài liệu PDF từ file input
            using (PdfReader pdfReader = new PdfReader(inputPdfPath))
            using (FileStream outputStream = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, outputStream))
                {
                    // Lấy số trang cuối cùng
                    int lastPage = pdfReader.NumberOfPages;

                    // Lấy trang cuối cùng để chèn ảnh chữ ký
                    PdfContentByte pdfContentByte = pdfStamper.GetOverContent(lastPage);

                    // Đọc ảnh chữ ký từ file
                    Image signatureImage = Image.GetInstance(signatureImagePath);

                    // Lấy kích thước của trang cuối cùng
                    Rectangle pageSize = pdfReader.GetPageSize(lastPage);

                    // Thiết lập vị trí và kích thước của ảnh chữ ký (ở góc dưới bên trái)
                    float x = signatureImage.ScaledWidth - pageSize.Right + 10; // Cách lề phải 10 đơn vị
                    float y = pageSize.Bottom + 20; // Cách lề dưới 10 đơn vị
                    signatureImage.SetAbsolutePosition(x, y);
                    signatureImage.ScaleToFit(100, 50);// Kích thước ảnh chữ ký

                    // Chèn ảnh chữ ký vào tài liệu PDF
                    pdfContentByte.AddImage(signatureImage);
                }
            }
        }
        catch (Exception ex)
        {

            throw new Exception ("ky số lỗi " + ex.Message);
        }
        
    }

    public static string AppendSuffixToFileName(string originalFileName, string suffix)
    {
        // Lấy tên tệp mà không có phần mở rộng
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

        // Lấy phần mở rộng của tệp
        string extension = Path.GetExtension(originalFileName);

        // Tạo tên tệp mới với phần hậu tố và phần mở rộng
        string newFileName = fileNameWithoutExtension + suffix + extension;

        return newFileName;
    }
    public static void Main(string[] args)
    {
        string certPath = "myPersonalCertificate.pfx"; // Đường dẫn đến file PDF gốc
        string inputPdfPath = "input.pdf"; // Đường dẫn đến file PDF gốc
        string outputPdfPath = "output_signed.pdf"; // Đường dẫn đến file PDF sau khi chèn ảnh chữ ký
        string signatureImagePath = "signature.png"; // Đường dẫn đến file ảnh chữ ký

        InsertSignatureImage(inputPdfPath, outputPdfPath, signatureImagePath);
        Console.WriteLine("Chèn ảnh chữ ký vào PDF thành công.");
    }
}

