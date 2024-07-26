using ProgressNotice.Data;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window, IDisposable
    {

        string[] filters = { "By creation time", "By alphabet", "By last change time" };

        internal List<ProjectLBI> list;
        internal ProjectLBI SelectedProject;

        public SearchWindow(ref List<ProjectLBI> projects)
        {
            InitializeComponent();
            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);
            list = projects;
            foreach (string filter in filters)
            {
                FiltersCB.Items.Add(filter);
            }
            foreach (ProjectLBI project in list)
            {
                ListOfProjects.Items.Add(project);
            }
            SearchBox.TextChanged += Searching;
            ListOfProjects.SelectionChanged += ProjectSelected;
            FiltersCB.SelectionChanged += FilterChanged;
            ReverseCB.Checked += Reversing;
            ReverseCB.Unchecked += Reversing;
            GoToBtn.Click += GoToSelected;
        }

        private void GoToSelected(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Reversing(object sender, RoutedEventArgs e)
        {
            if(list.Count > 0)
            {
                if ((bool)ReverseCB.IsChecked == true)
                {
                    list.Reverse();
                }
                else
                {
                    Filter();
                }
            }
            RefreshListBox();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
            RefreshListBox();
        }

        private void Filter()
        {
            if (list.Count > 0)
            {
                switch (FiltersCB.SelectedIndex)
                {
                    case 0: //By creation time
                        list = list.OrderBy(x => x.GetProject().CreationDate).ToList();
                        break;

                    case 1: //By alphabet
                        list = list.OrderBy(x => x.originalTitle).ToList();
                        break;

                    case 2: //By last change time
                        list = list.OrderBy(x => x.LastChange).ToList();
                        break;
                }
            }
        }

        private void ProjectSelected(object sender, RoutedEventArgs e)
        {
            SelectedProject = ListOfProjects.SelectedItem as ProjectLBI;
            GoToBtn.IsEnabled = SelectedProject == null ? false : true;
        }

        private void Searching(object sender, TextChangedEventArgs e)
        {
            ListOfProjects.Items.Clear();
            if (String.IsNullOrEmpty(SearchBox.Text))
            {
                RefreshListBox();
                return;
            }
            bool haveStar = SearchBox.Text.Trim().Contains(_starFilter);
            string[] keyWords = SearchBox.Text.Replace(_starFilter, "").ToLower().Trim().Replace(" ", "").Split("||");
            foreach (ProjectLBI project in list)
            {
                if(project.isStarred && haveStar)
                {
                    ListOfProjects.Items.Add(project);
                    continue;
                }
                for(int i = 0; i < keyWords.Length; i++) {
                    if (project.ProjectTitle.ToLower().Replace(" ", "").Contains(keyWords[i]))
                    {
                        if (ListOfProjects.Items.Contains(project))
                        {
                            continue;
                        }
                        ListOfProjects.Items.Add(project);
                        continue;
                    }
                }
            }
        }

        private void RefreshListBox()
        {
            ListOfProjects.Items.Clear();
            foreach (ProjectLBI project in list)
            {
                ListOfProjects.Items.Add(project);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
