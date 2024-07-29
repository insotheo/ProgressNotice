using Markdig;
using System.Windows;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MDPreviewDialog.xaml
    /// </summary>
    public partial class MDPreviewDialog : Window
    {

        private string html;

        public MDPreviewDialog(string mdSource)
        {
            InitializeComponent();
            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);

            MarkdownPipeline pipeline = new MarkdownPipelineBuilder().Build();
            html = Markdown.ToHtml(mdSource, pipeline);

            mdView.Load(html);
        }
    }
}
