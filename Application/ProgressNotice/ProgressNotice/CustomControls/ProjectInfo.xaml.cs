using ProgressNotice.Data;
using System.Windows.Controls;
using System.Windows;
using Markdig;
using System;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for ProjectInfo.xaml
    /// </summary>
    public partial class ProjectInfo : UserControl
    {
        public ProjectInfo()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;

            Tabs.SelectionChanged += TabChanged;
        }

        private void TabChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 0:
                    AboutMenu.Visibility = Visibility.Visible;
                    ChangeLogMenu.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    AboutMenu.Visibility = Visibility.Collapsed;
                    ChangeLogMenu.Visibility = Visibility.Visible;
                    break;
            }
        }

        internal void SetVisibilty(bool isVisible)
        {
            if(isVisible)
            {
                this.Visibility = Visibility.Visible;
                return;
            }
            this.Visibility = Visibility.Collapsed;
        }

        internal void LoadInfo(ref Project project)
        {
            TitleTB.Text = project.Title;
            SinceTB.Text = $"Since {project.CreationDate.ToString("g")}";
            LastChangeTB.Text = $"Last change on {project.LastChangeDate.ToString("g")}({Math.Round((DateTime.Now - project.LastChangeDate).TotalDays), 2} days ago)";
            mdViewer.Load(Markdown.ToHtml(project.DescriptionMD, new MarkdownPipelineBuilder().Build()));
        }

    }
}
