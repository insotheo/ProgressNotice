using System.Windows;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for MdEditorWindow.xaml
    /// </summary>
    public partial class MdEditorWindow : Window
    {
        internal bool IsSaved = false;
        internal string NewContent;

        public MdEditorWindow(string md)
        {
            InitializeComponent();
            TopMenuTM.Setup();
            mdBox.SetContent(md);
            SaveAndApplyBtn.Click += SaveAndApply;
        }

        private void SaveAndApply(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                IsSaved = true;
                NewContent = mdBox.GetContent();
                this.Close();
            }
        }
    }
}
