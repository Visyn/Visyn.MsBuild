
using Visyn.Build.Wix;

namespace Visyn.Build
{
    public class VisualStudioProject
    {
        public string Path { get; }

        public string Name { get; set; }
        public string Include { get; }
        public string Project { get; }

        public bool FileExists => System.IO.File.Exists(this.Path);
        public string Guid { get; set; }

        public VisualStudioProject(ProjectItemGroupProjectReference reference, string projectPath)
        {
            Name = reference.Name;
            Include = reference.Include;
            Project = reference.Project;
            Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, Include));
            //var abs = System.IO.Path.GetFullPath(Path);
        }

        public VisualStudioProject(string item, string projectPath)
        {
            Name = item;
            Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath,item));
        }

        public static bool IsValidProjectReference(ProjectItemGroupProjectReference reference)
        {
            return !string.IsNullOrWhiteSpace(reference?.Name);
        }
    }
}