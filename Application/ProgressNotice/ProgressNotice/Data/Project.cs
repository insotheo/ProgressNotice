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
            Token = createToken();
        }

        internal Project(ProjectCreationData creationData)
        {
            Title = creationData.Title;
            DescriptionMD = creationData.Description;
            CreationDate = LastChangeDate = DateTime.Now;
            Token = createToken();
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
                    writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                }
            }
        }

        internal void Remove()
        {
            string path = Path.Combine(_projectsPath, Token);
            Directory.Delete(path, true);
        }

        internal void Save()
        {
            string path = Path.Combine(_projectsPath, Token, _projectInfoFileName);
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        private string createToken()
        {
            string tmp = Title.Trim();

            foreach(char banned in Path.GetInvalidFileNameChars())
            {
                tmp = tmp.Replace(banned, _bannedTokenChar[0]);
            }

            return $"${{{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{tmp.Replace(" ", _bannedTokenChar).ToUpper()}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}}}";
        }

    }
}
