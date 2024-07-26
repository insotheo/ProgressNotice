using Markdig;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice
{
    internal static class MDSaver
    {

        internal static void Save(string textContent)
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                InitialDirectory = Path.Combine("C:", "Users", Environment.UserName, "Documents"),
                Filter = "Markdown | *.md |Text | *.txt|Web page| *.html",
                AddExtension = true
            };
            saveFile.ShowDialog();
            string fileName = saveFile.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("File name can't be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Path.GetExtension(fileName).ToLower() == ".html")
            {
                string html = Markdown.ToHtml(textContent, new MarkdownPipelineBuilder().Build());
                textContent = "<!DOCTYPE hmtl>\n" +
                    "<html>\n" +
                    "<head>\n" +
                    "<style>\n" +
                    $"{File.ReadAllText(_cssPath)}\n" +
                    "</style>\n" +
                    "</head>\n" +
                    "<body>\n" +
                    $"{html}\n" +
                    "</body>\n" +
                    "</html>";
            }
            File.WriteAllText(fileName, textContent);
        }

    }
}
