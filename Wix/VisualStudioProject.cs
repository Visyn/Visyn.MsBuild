namespace Visyn.Build.Wix
{
    public class VisualStudioProject
    {
        public string Name { get; }
        public string Include { get; }
        public string Project { get; }

        public VisualStudioProject(ProjectItemGroupProjectReference reference)
        {
            Name = reference.Name;
            Include = reference.Include;
            Project = reference.Project;
        }

        public static bool IsValidProjectReference(ProjectItemGroupProjectReference reference)
        {
            return !string.IsNullOrWhiteSpace(reference?.Name);
        }
    }
}