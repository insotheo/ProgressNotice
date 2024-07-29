using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for TopMenu.xaml
    /// </summary>
    public partial class TopMenu : UserControl
    {
        private bool windowStateChangedByControl = false;

        public TopMenu()
        {
            InitializeComponent();

            TopArea.MouseDown += TopAreaMouseDown;
            HideBtn.Click += HideBtnClick;
            ResizeBtn.Click += ResizeBtnClick;
            CloseBtn.Click += CloseBtnClick;
            WindowIcon.ToolTip = "Application version: " + Application.ResourceAssembly.GetName().Version.ToString();
        }

        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowStyle = WindowStyle.SingleBorderWindow;
            Window.GetWindow(this).Close();
        }

        private void HideBtnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowStyle = WindowStyle.SingleBorderWindow;
            Window.GetWindow(this).WindowState = WindowState.Minimized;
            Window.GetWindow(this).WindowStyle = WindowStyle.None;
        }

        private void ResizeBtnClick(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            if (parent == null)
            {
                return;
            }
            if (parent.WindowState != WindowState.Maximized)
            {
                windowStateChangedByControl = true;
                parent.WindowStyle = WindowStyle.SingleBorderWindow;
                parent.WindowState = WindowState.Maximized;
                parent.WindowStyle = WindowStyle.None;
                windowStateChangedByControl = false;
                SetResizeBtnIcon();
                return;
            }
            parent.WindowStyle = WindowStyle.SingleBorderWindow;
            parent.WindowState = WindowState.Normal;
            parent.WindowStyle = WindowStyle.None;
            SetResizeBtnIcon();
        }

        private void TopAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Window.GetWindow(this).WindowState == WindowState.Maximized)
                {
                    Window parentWindow = Window.GetWindow(this);

                    parentWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    parentWindow.WindowState = WindowState.Normal;
                    parentWindow.WindowStyle = WindowStyle.None;

                    Point cursorPos = Mouse.GetPosition(null);
                    double screenWidth = SystemParameters.PrimaryScreenWidth;
                    double screenHeight = SystemParameters.PrimaryScreenHeight;
                    double windowWidth = parentWindow.ActualWidth;
                    double windowHeight = parentWindow.ActualHeight;
                    parentWindow.Left = Math.Min(cursorPos.X, screenWidth - windowWidth);
                    parentWindow.Top = Math.Min(cursorPos.Y, screenHeight - windowHeight);

                }
                Window.GetWindow(this)?.DragMove();
                SetResizeBtnIcon();
            }
        }

        internal void Setup()
        {
            WindowTitleTB.Text = Window.GetWindow(this).Title;
            Window.GetWindow(this).StateChanged += ParentWindowStateChanged;
        }

        internal void SetButtonsVisibility(bool hide, bool resize, bool close)
        {
            HideBtn.Visibility = hide ? Visibility.Visible : Visibility.Collapsed;
            ResizeBtn.Visibility = resize ? Visibility.Visible : Visibility.Collapsed;
            CloseBtn.Visibility = close ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void ShowIcon()
        {
            if (Window.GetWindow(this).Icon != null)
            {
                IconBackground.Visibility = Visibility.Visible;
                WindowIcon.Visibility = Visibility.Visible;
                WindowIcon.Source = Window.GetWindow(this).Icon;
            }
        }

        private void ParentWindowStateChanged(object sender, EventArgs e)
        {
            if (!windowStateChangedByControl)
            {
                WindowState newWindowState = (sender as Window).WindowState;
                if (newWindowState == WindowState.Maximized)
                {
                    windowStateChangedByControl = true;
                    Window.GetWindow(this).WindowState = WindowState.Normal;
                    Window.GetWindow(this).WindowStyle = WindowStyle.SingleBorderWindow;
                    Window.GetWindow(this).WindowState = WindowState.Maximized;
                    Window.GetWindow(this).WindowStyle = WindowStyle.None;
                    SetResizeBtnIcon();
                    windowStateChangedByControl = false;
                    return;
                }
            }
        }

        private void SetResizeBtnIcon()
        {
            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                ResizeBtn.Content = "◱";
            }
            else
            {
                ResizeBtn.Content = "▭";
            }
        }

    }
}
