using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.Data
{
    class ProjectLBI
    {
        private bool isStarred;
        private string token;

        public string? ProjectTitle {  get; set; }
        public string? LastChange { get; set; }

        public ProjectLBI(Project parent)
        {
            ProjectTitle = parent.IsStarred ? _starredChar + parent.Title : parent.Title;
            LastChange = parent.LastChangeDate?.ToString("g");
            token = parent.Token;
            isStarred = parent.IsStarred;
        }

        public Project GetProject()
        {
            string path = Path.Combine(_projectsPath, token);
            if (!Directory.Exists(path))
            {
                throw new Exception($"Token({token}) doesn't exist!");
            }
            return JsonConvert.DeserializeObject<Project>(File.ReadAllText(Path.Combine(path, _projectInfoFileName)));
        }

        public static List<ProjectLBI> GetListOfProjects()
        {
            return JsonConvert.DeserializeObject<List<ProjectLBI>>(File.ReadAllText(_projectsListPath));
        }

        public static void SaveListOfProjects(ref List<ProjectLBI> list)
        {
            File.WriteAllText(_projectsListPath, JsonConvert.SerializeObject(list));
        }

    }
}
