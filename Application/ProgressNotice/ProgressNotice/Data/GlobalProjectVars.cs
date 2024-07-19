using System.IO;

namespace ProgressNotice.Data
{
    public static class GlobalProjectVars
    {
        public static readonly string _projectsPath = Path.Combine(Directory.GetCurrentDirectory(), "Projects");
        public static readonly string _projectsListPath = Path.Combine(_projectsPath, "prjs.list");
        public static readonly string _projectInfoFileName = "info.json";
        public static readonly string _starredChar = "✨ ";
    }
}
