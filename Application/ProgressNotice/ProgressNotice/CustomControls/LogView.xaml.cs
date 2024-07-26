using System.Windows.Controls;
using ProgressNotice.Data;
using Markdig;
using System.Windows;
using System.Linq;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private Log log;
        private string token;
        private int index;

        public LogView(Log log, string token, int index)
        {
            InitializeComponent();
            this.log = log;
            this.token = token;
            this.index = index;
            UpdatePreviewAndWidth();
            EditTitleBtn.Click += EditTitle;
            EditDescriptionBtn.Click += EditDescription;
        }

        private void UpdatePreviewAndWidth()
        {
            string html = Markdown.ToHtml(log.LogDescriptionMD, new MarkdownPipelineBuilder().Build());
            if (!string.IsNullOrEmpty(html))
            {
                this.Width = html.Split("\n", System.StringSplitOptions.RemoveEmptyEntries).Max(x => x.Length) * 26;
            }
            mdViewer.Load(html);
        }

        private void EditDescription(object sender, RoutedEventArgs e)
        {
            MdEditorWindow mdEditor = new MdEditorWindow(log.LogDescriptionMD);
            mdEditor.ShowDialog();
            if (mdEditor.IsSaved)
            {
                log.LogDescriptionMD = mdEditor.NewContent;
                SaveLog();
                UpdatePreviewAndWidth();
            }
        }

        private void EditTitle(object sender, RoutedEventArgs e)
        {
            TitleEditorWindow titleEditor = new TitleEditorWindow(log.LogName);
            titleEditor.ShowDialog();
            if (titleEditor.IsSaved)
            {
                log.LogName = titleEditor.NewTitle;
                SaveLog();
                (Window.GetWindow(this) as MainWindow).PrjInfoMenu.RefreshTree();
            }
        }

        private void SaveLog()
        {
            Project tmpProject = Project.GetProjectByToken(token);
            if (tmpProject == null)
            {
                return;
            }
            Logs tmpLogs = tmpProject.GetLogs();
            tmpLogs.LogsList[index] = log;
            tmpLogs.Save();
        }

    }
}
