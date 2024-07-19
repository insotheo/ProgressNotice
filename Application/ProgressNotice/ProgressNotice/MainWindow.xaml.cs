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

        private List<ProjectLBI>? previews;

        public MainWindow()
        {
            if (!Directory.Exists(_projectsPath))
            {
                Directory.CreateDirectory(_projectsPath);
            }
            if (!File.Exists(_projectsListPath))
            {
                using (FileStream fs = File.Create(_projectsListPath))
                {
                    using(StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(JsonConvert.SerializeObject(new List<ProjectLBI>()));
                    }
                }
            }

            previews = ProjectLBI.GetListOfProjects();

            InitializeComponent();
            TopMenuTM.Setup();

            AddNewProjectBtn.Click += AddNewProject;

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

        private void AddNewProject(object sender, RoutedEventArgs e)
        {
            CreationDialog creationDialog = new CreationDialog();
            creationDialog.ShowDialog();

            RefreshListBox();
        }
    }
}