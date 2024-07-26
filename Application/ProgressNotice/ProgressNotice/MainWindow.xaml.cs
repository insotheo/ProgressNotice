using System.Collections.Generic;
using System.Windows;
using ProgressNotice.Data;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Linq;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        internal List<ProjectLBI> previews;

        public MainWindow()
        {
            if (!Directory.Exists(_projectsPath))
            {
                Directory.CreateDirectory(_projectsPath);
            }
            if (!File.Exists(_projectsListPath))
            {
                previews = new List<ProjectLBI>();
                using (FileStream fs = File.Create(_projectsListPath))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(JsonConvert.SerializeObject(previews));
                    }
                }
            }
            else
            {
                previews = JsonConvert.DeserializeObject<List<ProjectLBI>>(File.ReadAllText(_projectsListPath));
            }

            InitializeComponent();
            TopMenuTM.Setup();

            AddNewProjectBtn.Click += AddNewProject;
            RemoveProjectBtn.Click += RemoveProject;
            ProjectsLB.SelectionChanged += OnProjectSelected;
            SearchBtn.Click += Search;

            RefreshListBox();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            using(SearchWindow searchWindow = new SearchWindow(ref previews))
            {
                searchWindow.ShowDialog();
                previews = searchWindow.list;
                RefreshListBox();
                SaveList();
                if (searchWindow.SelectedProject != null)
                {
                    SetSelectedItemIndex(previews.IndexOf(searchWindow.SelectedProject));
                }
            }
        }

        internal void RefreshListBox()
        {
            ProjectsLB.Items.Clear();
            previews = previews.OrderByDescending(u => u.isStarred).ToList();
            foreach (ProjectLBI prj in previews)
            {
                ProjectsLB.Items.Add(prj);
            }
        }

        internal void SaveList()
        {
            File.WriteAllText(_projectsListPath, JsonConvert.SerializeObject(previews));
        }


        internal int GetSelectedItemIndex() => ProjectsLB.SelectedIndex;

        internal void SetSelectedItemIndex(int index) => ProjectsLB.SelectedIndex = index; 

        private void OnProjectSelected(object sender, SelectionChangedEventArgs e)
        {
            if(ProjectsLB.Items.Count <= 0 && ProjectsLB.SelectedItem == null)
            {
                PrjInfoMenu.SetVisibilty(false);
                return;
            }
            Project selected = (ProjectsLB.SelectedItem as ProjectLBI).GetProject();
            PrjInfoMenu.LoadInfo(ref selected);
            PrjInfoMenu.SetVisibilty(true);
        }

        private void RemoveProject(object sender, RoutedEventArgs e)
        {
            if (ProjectsLB.SelectedItem != null)
            {
                ProjectLBI selected = ProjectsLB.SelectedItem as ProjectLBI;
                if (MessageBox.Show($"Are you sure you want to remove \"{selected.ProjectTitle.Replace(_starredChar, "")}\"?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    previews.Remove(selected);
                    selected.GetProject().Remove();
                    SaveList();
                    RefreshListBox();
                }
            }
            else
            {
                MessageBox.Show("You have to select project from the list!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddNewProject(object sender, RoutedEventArgs e)
        {
            using (CreationDialog creationDialog = new CreationDialog())
            {
                creationDialog.ShowDialog();
                if (creationDialog.IsCreated)
                {
                    Project newProject = new Project(creationDialog.CreationData);
                    newProject.Create();
                    previews.Add(newProject.GetListBoxItem());
                    SaveList();
                }
            }

            RefreshListBox();
        }
    }
}