using System.IO;
using System;
using Newtonsoft.Json;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.Data
{
    class ProjectLBI
    {
        public bool isStarred {  get; set; }
        public string token {  get; set; }

        public string ProjectTitle {  get; set; }
        public string LastChange { get; set; }

        public ProjectLBI(Project parent)
        {
            ProjectTitle = parent.IsStarred ? _starredChar + parent.Title : parent.Title;
            LastChange = parent.LastChangeDate.ToString("g");
            token = parent.Token;
            isStarred = parent.IsStarred;
        }

        [JsonConstructor]
        public ProjectLBI(bool isStarred, string token, string projectTitle, string lastChange)
        {
            this.isStarred = isStarred;
            this.token = token;
            ProjectTitle = projectTitle;
            LastChange = lastChange;
        }

        public Project GetProject()
        {
            string path = Path.Combine(_projectsPath, token);
            if (!Directory.Exists(path))
            {
                throw new Exception($"Token({token}) doesn't exist!");
            }
            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(Path.Combine(path, _projectInfoFileName)));
            return project;
        }

    }
}
