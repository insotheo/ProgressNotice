using System.Windows;

namespace ProgressNotice
{
    /// <summary>
    /// Interaction logic for CreationWindow.xaml
    /// </summary>
    public partial class CreationDialog : Window
    {
        public CreationDialog()
        {
            InitializeComponent();

            TopMenuTM.Setup();
            TopMenuTM.SetButtonsVisibility(false, false, true);
        }
    }
}
