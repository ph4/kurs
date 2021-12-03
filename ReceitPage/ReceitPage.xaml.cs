using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using Color = MigraDoc.DocumentObjectModel.Color;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace kurs
{
    /// <summary>
    /// Interaction logic for ReceitPage.xaml
    /// </summary>
    public partial class ReceitPage : Page, INotifyPropertyChanged
    {
        private Document GetDocument()
        {
            var doc = new Document();
            doc.DefaultPageSetup.Orientation = Orientation.Portrait;
            doc.DefaultPageSetup.PageFormat = PageFormat.A4;
            doc.Info.Title = "Recit";
            var ordnum = 123;
            doc.Info.Title = $"Квитанция к заказу №${ordnum}";
            var s = doc.AddSection();
            var p = s.AddParagraph();
            //p.Format.Font.Color = Color.Parse("#aaddff");
            p.Format.Font.Size = 40;
            p.AddFormattedText("Hello, world", TextFormat.Bold);
            return doc;
        }
        public ReceitPage()
        {
            InitializeComponent();
            var user = Manager.CurrentUser;
            var order = Manager.CurrentUser.CurrentOrderGetOrCreate();
            var rd = new ReceitDocument();

            var doc = rd.CreateDocument(user.Fio, order.address.address1, order.cart1.cart_items);

            string ddl = DdlWriter.WriteToString(doc);
            DocumentView.Ddl = ddl;
            DocumentView.viewer.FitToHeight();
        }


        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            // An enum indicating whether to embed fonts or not.
            // This setting applies to all font programs used in the document.
            // This setting has no effect on the RTF renderer.
            // (The term 'font program' is used by Adobe for a file containing a font. Technically a 'font file'
            // is a collection of small programs and each program renders the glyph of a character when executed.
            // Using a font in PDFsharp may lead to the embedding of one or more font programms, because each outline
            // (regular, bold, italic, bold+italic, ...) has its own fontprogram)
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false)
            {
                // Associate the MigraDoc document with a renderer
                Document = GetDocument(),
            };


            // Layout and render document to PDF
            pdfRenderer.RenderDocument();


            // Save the document...
            const string filename = "HelloWorld.pdf";
            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
