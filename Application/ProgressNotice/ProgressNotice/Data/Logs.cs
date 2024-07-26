using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static ProgressNotice.Data.GlobalProjectVars;

namespace ProgressNotice.Data
{
    internal class Logs
    {
        public List<Log> LogsList;
        public string ParentToken;

        [JsonConstructor]
        internal Logs(List<Log> logsList, string token)
        {
            LogsList = logsList;
            ParentToken = token;
        }

        internal void AddLog(Log log)
        {
            LogsList.Add(log);
        }

        internal void Save()
        {
            File.WriteAllText(Path.Combine(_projectsPath, ParentToken, _projectLogsFileName), JsonConvert.SerializeObject(this, Formatting.Indented));
        }

    }
}
