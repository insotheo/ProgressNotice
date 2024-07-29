using Newtonsoft.Json;
using System;

namespace ProgressNotice.Data
{
    public class Log
    {

        public DateTime LogDateTime;
        public string LogName;
        public string LogDescriptionMD;

        [JsonConstructor]
        internal Log(DateTime logDateTime, string logName, string logDescription)
        {
            LogDateTime = logDateTime;
            LogName = logName;
            LogDescriptionMD = logDescription;
        }

        internal Log(string name, string description)
        {
            LogDateTime = DateTime.Now;
            LogName = name;
            LogDescriptionMD = description;
        }

    }
}
