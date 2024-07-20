using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ProgressNotice.CustomControls
{
    /// <summary>
    /// Interaction logic for MDBox.xaml
    /// </summary>
    public partial class MDBox : UserControl
    {
        public MDBox()
        {
            InitializeComponent();
            CodeBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            CodeBox.TextChanged += CodeBoxTextChanged;
            Scroller.PreviewMouseWheel += ScrollerPreviewMouseWheel;
            CodeBox.ScrollToVerticalOffset(0);
            PreviewBtn.Click += Preview;
        }

        public string GetContent()
        {
            TextRange range = new TextRange(CodeBox.Document.ContentStart, CodeBox.Document.ContentEnd);
            return range.Text;
        }

        public double GetFontSize()
        {
            return CodeBox.FontSize;
        }

        private void Preview(object sender, RoutedEventArgs e)
        {
            MDPreviewDialog previewDialog = new MDPreviewDialog(GetContent());
            previewDialog.ShowDialog();
        }

        private void ScrollerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Scroller != null)
            {
                Scroller.ScrollToVerticalOffset(Scroller.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void UpdateNumberLines()
        {
            NumList.Items.Clear();
            var doc = CodeBox.Document;
            doc.PageWidth = 5000;

            foreach(Block block in doc.Blocks)
            {
                if(block is Paragraph paragraph)
                {
                    paragraph.LineHeight = 0.01f;
                }
            }

            int linesCounter = 1;
            TextPointer pointer = doc.ContentStart;
            while(pointer != null && pointer.CompareTo(doc.ContentEnd) < 0)
            {
                if(pointer.GetTextInRun(LogicalDirection.Forward).Length > 0)
                {
                    NumList.Items.Add(linesCounter++);
                }
                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private void CodeBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateNumberLines();
        }
    }
}
