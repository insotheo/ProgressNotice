using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.Data
{
    public class Project
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
            using(FileStream logs = File.Create(Path.Combine(_projectsPath, Token, _projectLogsFileName)))
            {
                using(StreamWriter writer = new StreamWriter(logs))
                {
                    writer.Write(JsonConvert.SerializeObject(new Logs(new List<Log>(), Token), Formatting.Indented));
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

        internal Logs GetLogs()
        {
            Logs logs = JsonConvert.DeserializeObject<Logs>(File.ReadAllText(Path.Combine(_projectsPath, Token, _projectLogsFileName)));
            return logs == null ? new Logs(new List<Log>(), Token) : logs;
        }

        internal static Project GetProjectByToken(string token)
        {
            string path = Path.Combine(_projectsPath, token);
            if (!Directory.Exists(path))
            {
                throw new Exception($"Token({token}) doesn't exist!");
            }
            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(Path.Combine(path, _projectInfoFileName)));
            return project;
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
