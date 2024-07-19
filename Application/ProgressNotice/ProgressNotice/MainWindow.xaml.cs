using System.Collections.Generic;
using System.Windows;
using ProgressNotice.Data;
using Newtonsoft.Json;
using System.IO;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TopMenuTM.Setup();

            Project prj = new Project("Hello, World!", "bla-bla-bla");
            ProjectsLB.Items.Add(prj.GetListBoxItem());
            prj.IsStarred = true;
            ProjectsLB.Items.Add(prj.GetListBoxItem());
        }
    }
}