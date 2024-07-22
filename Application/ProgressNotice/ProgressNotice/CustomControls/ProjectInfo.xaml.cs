﻿using ProgressNotice.Data;
using System.Windows.Controls;
using System.Windows;
using Markdig;
using System;
using System.Windows.Input;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for ProjectInfo.xaml
    /// </summary>
    public partial class ProjectInfo : UserControl
    {

        private Project prj;

        public ProjectInfo()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;

            Tabs.SelectionChanged += TabChanged;

            EditAboutBtn.Click += EditAbout;
            EditTitleBtn.Click += EditTitle;
            StarProjectBtn.Click += StarProject;

            TitleBorder.MouseEnter += (object sender, MouseEventArgs e) => { TitleButtonsBorder.Visibility = Visibility.Visible; };
            TitleBorder.MouseLeave += (object sender, MouseEventArgs e) => { TitleButtonsBorder.Visibility = Visibility.Collapsed; };
        }

        private void StarProject(object sender, RoutedEventArgs e)
        {
            prj.IsStarred = !prj.IsStarred;
            StarProjectBtn.Content = prj.IsStarred ? "Make unstarred" : "Make starred";
            if(Window.GetWindow(this) is MainWindow)
            {
                MainWindow parent = Window.GetWindow(this) as MainWindow;
                ProjectLBI prjLBI = prj.GetListBoxItem();
                parent.previews.RemoveAt(parent.GetSelectedItemIndex());
                parent.previews.Add(prjLBI);
                parent.SaveList();
                prj.Save();
                parent.RefreshListBox();
                parent.SetSelectedItemIndex(parent.previews.IndexOf(prjLBI));
            }
        }

        private void EditTitle(object sender, RoutedEventArgs e)
        {
            TitleEditorWindow titleEditor = new TitleEditorWindow(prj.Title);
            titleEditor.ShowDialog();
            if (titleEditor.IsSaved)
            {
                if(Window.GetWindow(this) is MainWindow)
                {
                    prj.Title = titleEditor.NewTitle;
                    MainWindow parent = Window.GetWindow(this) as MainWindow;
                    int i = parent.GetSelectedItemIndex();
                    parent.previews[i].ProjectTitle = prj.Title;
                    parent.SaveList();
                    prj.Save();
                    parent.RefreshListBox();
                    parent.SetSelectedItemIndex(i);
                }
            }
            LoadInfo(ref prj);
        }

        private void EditAbout(object sender, RoutedEventArgs e)
        {
            MdEditorWindow mdEditor = new MdEditorWindow(prj.DescriptionMD);
            mdEditor.ShowDialog();
            if (mdEditor.IsSaved)
            {
                prj.DescriptionMD = mdEditor.NewContent;
                prj.Save();
            }
            LoadInfo(ref prj);
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
            prj = project;
            TitleTB.Text = project.Title;
            SinceTB.Text = $"Since {project.CreationDate.ToString("g")}";
            LastChangeTB.Text = $"Last change on {project.LastChangeDate.ToString("g")}({Math.Round((DateTime.Now - project.LastChangeDate).TotalDays), 2} days ago)";
            mdViewer.Load(Markdown.ToHtml(project.DescriptionMD, new MarkdownPipelineBuilder().Build()));
            StarProjectBtn.Content = project.IsStarred ? "Make unstarred" : "Make starred";
        }

    }
}
