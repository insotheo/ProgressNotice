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

        internal void RemoveLog(Log log)
        {
            if(LogsList.Contains(log))
            {
                LogsList.Remove(log);
            }
        }

        internal void RemoveLog(int index)
        {
            if(LogsList.Count - 1 > index && index >= 0)
            {
                LogsList.RemoveAt(index);
            }
        }

        internal void Save()
        {
            File.WriteAllText(Path.Combine(_projectsPath, ParentToken, _projectLogsFileName), JsonConvert.SerializeObject(this, Formatting.Indented));
        }

    }
}
