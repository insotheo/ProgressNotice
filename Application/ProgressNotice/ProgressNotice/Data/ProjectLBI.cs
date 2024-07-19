namespace ProgressNotice.Data
{
    class ProjectLBI
    {
        public string? ProjectTitle {  get; set; }
        public string? LastChange { get; set; }

        public string? Token;

        public ProjectLBI(Project parent)
        {
            ProjectTitle = parent.IsStarred ? "✨ " + parent.Title : parent.Title;
            LastChange = parent.LastChangeDate?.ToString("g");
            Token = parent.Token;
        }
    }
}
