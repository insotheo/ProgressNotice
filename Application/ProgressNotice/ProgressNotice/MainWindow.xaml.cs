using System.Collections.Generic;
using System.Windows;
using ProgressNotice.Data;
using System.IO;
using Newtonsoft.Json;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private List<ProjectLBI> previews;

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

            RefreshListBox();
        }

        private void RefreshListBox()
        {
            ProjectsLB.Items.Clear();
            foreach(ProjectLBI prj in previews)
            {
                ProjectsLB.Items.Add(prj);
            }
        }

        private void SaveList()
        {
            File.WriteAllText(_projectsListPath, JsonConvert.SerializeObject(previews));
        }

        private void RemoveProject(object sender, RoutedEventArgs e)
        {
            if (ProjectsLB.SelectedItem != null)
            {
                ProjectLBI selected = ProjectsLB.SelectedItem as ProjectLBI;
                if (MessageBox.Show($"Are you sure you want to remove \"{selected.ProjectTitle}\"?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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