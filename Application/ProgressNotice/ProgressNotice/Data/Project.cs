using Newtonsoft.Json;
using System;
using System.IO;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.Data
{
    internal class Project
    {
        public bool IsStarred;

        public string Title;
        public string Token;
        public string DescriptionMD;

        public DateTime LastChangeDate;
        public readonly DateTime CreationDate;

        [JsonConstructor]
        public Project(bool isStarred, string title, string token, string descriptionMD, DateTime lastChangeDate, DateTime creationDate)
        {
            IsStarred = isStarred;
            Title = title;
            Token = token;
            DescriptionMD = descriptionMD;
            LastChangeDate = lastChangeDate;
            CreationDate = creationDate;
        }

        internal Project(string title, string description)
        {
            Title = title;
            DescriptionMD = description;
            CreationDate = LastChangeDate = DateTime.Now;
            Token = $"${{{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{Title.Replace(" ", "#").ToUpper()}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}}}";
        }

        internal Project(ProjectCreationData creationData)
        {
            Title = creationData.Title;
            DescriptionMD = creationData.Description;
            CreationDate = LastChangeDate = DateTime.Now;
            Token = $"${{{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{Title.Replace(" ", "#").ToUpper()}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}}}";
        }

        internal ProjectLBI GetListBoxItem()
        {
            return new ProjectLBI(this);
        }

        internal void Create()
        {
            string path = Path.Combine(_projectsPath, Token);
            Directory.CreateDirectory(path);
            using(FileStream info = File.Create(Path.Combine(path, _projectInfoFileName)))
            {
                using(StreamWriter writer = new StreamWriter(info))
                {
                    writer.Write(JsonConvert.SerializeObject(this));
                }
            }
        }

    }
}
