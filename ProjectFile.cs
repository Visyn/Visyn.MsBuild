using Visyn.Exceptions;
using Visyn.Build.VisualStudio.CsProj;
using System;

namespace Visyn.Build
{
    public class ProjectFile
    {
        public ResourceType ResourceType { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }

        public bool FileExists => System.IO.File.Exists(this.Path);

        public string Dependancy { get; private set; }
        public ProjectFile(string fileName, ResourceType resourceType, ProjectFileBase project)
        {
            FileName = fileName;
            Path = project.GetFullPath(FileName);
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, FileName)); 
            ResourceType = resourceType;
        }

        public ProjectFile(string fileName, ResourceType resourceType, string path)
        {
            FileName = fileName;
            Path = path;
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, FileName)); 
            ResourceType = resourceType;
        }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Dependancy) ?
                $"{ResourceType}\t{FileName}\tDependant: {Dependancy}" :
                $"{ResourceType}\t{FileName}";
        }

        public  Compile ToCompile(string projectPath)
        {
            // public string AutoGen { get; set; }
            // public string DesignTime { get; set; }
            // public string DependentUpon { get; set; }
            // public string Include { get; set; }
            var path = this.Path;
            if(this.Path.Contains(projectPath))
            {
                path = this.Path.Replace(projectPath, "");
            }
            return new Compile() { Include = path };
        }

        public static ProjectFile CreateIfValid(ProjectItemGroup item, ResourceType resourceType, ProjectFileBase project)
        {
            return !string.IsNullOrWhiteSpace(item?.Include) ? 
                new ProjectFile(item.Include, resourceType, project) { Dependancy = (item as Compile)?.DependentUpon } :
                null;
        }
    }
}