using System.Windows.Controls;
using System.IO;

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
            string src = $"<style>\n{File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "CustomControls", "ViewerStyle.css"))}\n</style>\n<body>\n" +
                            $"{htmlSource}\n</body>";

            LoadHtml(src);
        }

        private async void LoadHtml(string src)
        {
            await mdView.EnsureCoreWebView2Async();
            mdView.NavigateToString(src);
        }

    }
}
