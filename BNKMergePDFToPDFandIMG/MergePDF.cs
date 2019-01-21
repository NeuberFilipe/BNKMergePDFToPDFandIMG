using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.IO;
using System.Linq;

namespace BNKMergePDFToPDFandIMG
{
    public class MergePDF
    {
        public void PDF()
        {
            //Load the PDF document
            DirectoryInfo dir = new DirectoryInfo(@"XXXXXXX");
            FileStream docStream = new FileStream(@"XXXXXXX.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument doc = new PdfLoadedDocument(docStream);

            ConvertImgToPDFAndMergePDF(dir, ref doc);

            //Creating the stream object

            MemoryStream stream = new MemoryStream();

            //Save the document as stream

            doc.Save(stream);

            //If the position is not set to '0' then the PDF will be empty.

            stream.Position = 0;

            //Close the document.

            doc.Close(true);

            //Defining the ContentType for pdf file.

            string contentType = "application/pdf";

            //Define the file name.

            string fileName = "Output.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name.
            File.WriteAllBytes(@"XXXXXXX.pdf", stream.ToArray());
        }

        private void MergePDFToImg(ref PdfLoadedDocument doc)
        {
            //Get first page from document

            PdfLoadedPage page = doc.Pages[0] as PdfLoadedPage;

            //Create PDF graphics for the page

            PdfGraphics graphics = page.Graphics;

            //Load the image from the disk

            FileStream imageStream = new FileStream(@"XXXXXXX.png", FileMode.Open, FileAccess.Read);

            PdfBitmap image = new PdfBitmap(imageStream);

            //Draw the image

            doc.Pages.Add().Graphics.DrawImage(image, (page.Size.Width / 4), (page.Size.Height / 2) - image.Height, image.Width, image.Height);

            //graphics.DrawImage(image, (page.Size.Width / 4), (page.Size.Height / 2) - image.Height, image.Width, image.Height);
        }

        private void ConvertImgToPDFAndMergePDF(DirectoryInfo dir, ref PdfLoadedDocument doc)
        {
            object[] dobj;
            int count = 0;
            dobj = new object[dir.EnumerateFiles().Where(a => a.Extension.Equals(".pdf")).Count()];
            foreach (FileInfo f in dir.EnumerateFiles().Where(a => a.Extension != ".pdf").OrderBy(r => r.Name))
            {
                MergePDFToImg(ref doc);
            }

            foreach (FileInfo f in dir.EnumerateFiles().Where(a => a.Extension == ".pdf").OrderBy(r => r.Name)) // E diferente do idProtocolo
            {
                count = PreperMerge(dobj, count, f);
            }

            Merger(doc, dobj);
        }

        private static int PreperMerge(object[] dobj, int count, FileInfo f)
        {
            FileStream filePDF = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            dobj[count] = new PdfLoadedDocument(filePDF);
            count++;
            return count;
        }

        private static void Merger(PdfLoadedDocument doc, object[] dobj)
        {
            if (dobj != null && dobj.Length > 0)
                PdfDocumentBase.Merge(doc, dobj);
        }
    }
}
