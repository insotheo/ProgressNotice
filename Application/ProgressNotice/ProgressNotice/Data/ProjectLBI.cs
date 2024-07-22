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
        public string originalTitle { get; set; }
        public string ProjectTitle { get { return isStarred ? _starredChar + originalTitle : originalTitle; } }
        public string LastChange { get; set; }

        public ProjectLBI(Project parent)
        {
            originalTitle = parent.Title;
            LastChange = parent.LastChangeDate.ToString("g");
            token = parent.Token;
            isStarred = parent.IsStarred;
        }

        [JsonConstructor]
        public ProjectLBI(bool isStarred, string token, string originalTitle, string lastChange)
        {
            this.isStarred = isStarred;
            this.token = token;
            this.originalTitle = originalTitle;
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
