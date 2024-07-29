using ProgressNotice.Data;
using System;
using System.Windows;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for CreationWindow.xaml
    /// </summary>
    public partial class CreationDialog : Window, IDisposable
    {

        internal ProjectCreationData CreationData;
        internal bool IsCreated = false;

        public CreationDialog()
        {
            InitializeComponent();

            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);
            CreateBtn.Click += Create;
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TitleTB.Text.Trim() == String.Empty)
                {
                    throw new Exception("Project title can't be empty");
                }
                CreationData = new ProjectCreationData()
                {
                    Title = TitleTB.Text,
                    Description = DescriptionBox.GetContent()
                };
                IsCreated = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
