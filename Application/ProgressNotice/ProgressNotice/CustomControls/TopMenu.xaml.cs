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
        }

        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowStyle = WindowStyle.SingleBorderWindow;
            Environment.Exit(0);
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
            if(parent.WindowState != WindowState.Maximized)
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
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if(Window.GetWindow(this).WindowState == WindowState.Maximized)
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

        private void ParentWindowStateChanged(object? sender, EventArgs e)
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
            if(Window.GetWindow(this).WindowState == WindowState.Maximized)
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
