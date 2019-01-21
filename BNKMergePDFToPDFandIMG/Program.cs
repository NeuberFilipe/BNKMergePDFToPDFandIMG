using Syncfusion.Pdf.Parsing;
using System;
using System.IO;

namespace BNKMergePDFToPDFandIMG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Converting............!");
            var merge = new MergePDF();
            merge.PDF();
        }
    }
}
