using System.Windows;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for TitleEditorWindow.xaml
    /// </summary>
    public partial class TitleEditorWindow : Window
    {

        internal bool IsSaved = false;
        internal string NewTitle;

        public TitleEditorWindow(string title)
        {
            InitializeComponent();
            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);
            TitleTB.Text = title;
            SaveAndApplyBtn.Click += SaveAndApply;
        }

        private void SaveAndApply(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                IsSaved = true;
                NewTitle = TitleTB.Text.Trim();
                this.Close();
            }
        }
    }
}
