using Markdig;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System;
using ProgressNotice.Data;
using System.IO.Compression;

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
            if (saveFile.ShowDialog() == true)
            {
                saveOneFile(saveFile.FileName, textContent);
            }
        }

        internal static void SaveToArchive(Logs logs)
        {
            SaveFileDialog saveArchive = new SaveFileDialog()
            {
                InitialDirectory = Path.Combine("C:", "Users", Environment.UserName, "Documents"),
                Filter = "Archive | *.zip",
                AddExtension = true
            };
            if(saveArchive.ShowDialog() == true)
            {
                using (ZipArchive archive = ZipFile.Open(saveArchive.FileName, ZipArchiveMode.Create))
                {
                    foreach (Log log in logs.LogsList)
                    {
                        string name = log.LogName.Trim();
                        foreach(char invalid in Path.GetInvalidFileNameChars())
                        {
                            if (name.Contains(invalid))
                            {
                                name = name.Replace(invalid.ToString(), _bannedTokenChar);
                            }
                        }
                        name = $"{log.LogDateTime.Hour}{log.LogDateTime.Minute}{log.LogDateTime.Second}{log.LogDateTime.Millisecond}_" + name + ".md";
                        ZipArchiveEntry entry = archive.CreateEntry(name);
                        using(StreamWriter writer = new StreamWriter(entry.Open()))
                        {
                            writer.Write(log.LogDescriptionMD);
                        }
                    }
                }
            }
        }


        private static void saveOneFile(string fileName, string textContent)
        {
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
