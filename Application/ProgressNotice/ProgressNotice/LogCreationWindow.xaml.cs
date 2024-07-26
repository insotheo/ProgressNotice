using System;
using System.Windows;
using ProgressNotice.Data;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for LogCreationWindow.xaml
    /// </summary>
    public partial class LogCreationWindow : Window, IDisposable
    {

        internal Log NewLog;
        internal bool IsCreated = false;

        public LogCreationWindow()
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
                NewLog = new Log(TitleTB.Text, DescriptionBox.GetContent());
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
