using Visyn.Build.VisualStudio.CsProj;

namespace Visyn.Build
{
    public class ProjectFile
    {
        public ResourceType ResourceType { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }

        public bool FileExists => System.IO.File.Exists(this.Path);

        public string Dependancy { get; private set; }
        public ProjectFile(string fileName, ResourceType resourceType, string projectPath)
        {
            FileName = fileName;
            Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, FileName)); 
            ResourceType = resourceType;
        }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Dependancy) ?
                $"{ResourceType}\t{FileName}\tDependant: {Dependancy}" :
                $"{ResourceType}\t{FileName}";
        }

        public static ProjectFile CreateIfValid(ProjectItemGroup item, ResourceType resourceType, string projectPath)
        {
            return !string.IsNullOrWhiteSpace(item?.Include) ? 
                new ProjectFile(item.Include, resourceType, projectPath) { Dependancy = (item as Compile)?.DependentUpon } :
                null;
        }
    }
}