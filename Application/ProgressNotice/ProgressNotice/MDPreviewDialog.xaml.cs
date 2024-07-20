using System.IO;
using System.Windows;
using System.Windows.Documents;
using Markdig;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MDPreviewDialog.xaml
    /// </summary>
    public partial class MDPreviewDialog : Window
    {
        public MDPreviewDialog(string mdSource)
        {
            InitializeComponent();
            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);
            MarkdownPipeline pipeline = new MarkdownPipelineBuilder().Build();
            string html = Markdown.ToHtml(mdSource, pipeline);

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(html));

            FlowDocument flowDoc = new FlowDocument();
            flowDoc.Blocks.Add(paragraph);

            PreviewBox.Document = flowDoc;
        }
    }
}
