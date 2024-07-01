using iTextSharp.text;
using iTextSharp.text.pdf;

public class PdfSignatureInserter
{
    public static void InsertSignatureImage(string inputPdfPath, string outputPdfPath, string signatureImagePath)
    {
        // Đọc tài liệu PDF từ file input
        PdfReader pdfReader = new PdfReader(inputPdfPath);
        using (FileStream outputStream = new FileStream(outputPdfPath, FileMode.Create))
        {
            PdfStamper pdfStamper = new PdfStamper(pdfReader, outputStream);

            // Lấy trang đầu tiên để chèn ảnh chữ ký (có thể thay đổi nếu cần)
            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(1);

            // Đọc ảnh chữ ký từ file
            Image signatureImage = Image.GetInstance(signatureImagePath);

            // Thiết lập vị trí và kích thước của ảnh chữ ký (có thể thay đổi nếu cần)
            signatureImage.SetAbsolutePosition(100, 100); // Vị trí (x, y) trên trang PDF
            signatureImage.ScaleToFit(200, 100); // Kích thước ảnh chữ ký

            // Chèn ảnh chữ ký vào tài liệu PDF
            pdfContentByte.AddImage(signatureImage);

            pdfStamper.Close();
        }
        pdfReader.Close();
    }

    public static void Main(string[] args)
    {
        string inputPdfPath = "input.pdf"; // Đường dẫn đến file PDF gốc
        string outputPdfPath = "output_signed.pdf"; // Đường dẫn đến file PDF sau khi chèn ảnh chữ ký
        string signatureImagePath = "signature.png"; // Đường dẫn đến file ảnh chữ ký

        InsertSignatureImage(inputPdfPath, outputPdfPath, signatureImagePath);
        Console.WriteLine("Chèn ảnh chữ ký vào PDF thành công.");
    }
}

