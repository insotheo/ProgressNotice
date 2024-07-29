using System.IO;
using System.Windows.Controls;
using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for MdViewer.xaml
    /// </summary>
    public partial class MdViewer : UserControl
    {
        public MdViewer()
        {
            InitializeComponent();

        }

        public void Load(string htmlSource)
        {
            string src = $"<style>\n{File.ReadAllText(_cssPath)}\n</style>\n" +
                $"<body>\n" +
                $"{htmlSource}\n" +
                $"<script>\n" +
                $"{File.ReadAllText(_jsPath)}\n" +
                $"</script>" +
                $"</body>\n";

            LoadHtml(src);
        }

        private async void LoadHtml(string src)
        {
            await mdView.EnsureCoreWebView2Async();
            mdView.NavigateToString(src);
        }

    }
}
