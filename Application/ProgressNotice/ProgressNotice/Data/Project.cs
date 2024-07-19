using System;

namespace ProgressNotice.Data
{
    internal class Project
    {
        public bool IsStarred;

        public string? Title;
        public string? Token;
        public string? DescriptionMD;

        public DateTime? LastChangeDate;
        public readonly DateTime? CreationDate;
     
        
        internal Project(string title, string description)
        {
            Title = title;
            DescriptionMD = description;
            CreationDate = LastChangeDate = DateTime.Now;
            Token = $"${{{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{title.Replace(" ", "#").ToUpper()}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}}}";
        }

        internal ProjectLBI GetListBoxItem()
        {
            return new ProjectLBI(this);
        }

    }
}
