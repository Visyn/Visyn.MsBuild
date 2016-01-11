
using Visyn.Build.Wix;

namespace Visyn.Build
{
    public class NestedProject
    {
        public string Path { get; }

        public string Name { get; set; }
        public string Include { get; }
        public string Project { get; }

        public bool FileExists => System.IO.File.Exists(this.Path);
        public string Guid { get; set; }

        public NestedProject(ProjectItemGroupProjectReference reference, string projectPath)
        {
            Name = reference.Name;
            Include = reference.Include;
            Project = reference.Project;
            Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, Include));
        }

        public NestedProject(string item, string projectPath)
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