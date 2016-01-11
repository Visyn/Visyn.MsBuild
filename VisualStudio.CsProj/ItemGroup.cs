using System.Collections.Generic;
using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemGroup
    {
        /// <remarks/>
        [XmlElement("None")]
        public ProjectItemGroup[] None { get; set; }

        /// <remarks/>
        [XmlElement("EmbeddedResource")]
        public EmbeddedResource[] EmbeddedResource { get; set; }

        /// <remarks/>
        [XmlElement("Resource")]
        public ProjectItemGroup[] Resource { get; set; }

        /// <remarks/>
        [XmlElement("Content")]
        public ProjectItemGroup Content { get; set; }

        /// <remarks/>
        public ProjectItemGroupApplicationDefinition ApplicationDefinition { get; set; }

        /// <remarks/>
        [XmlElement("Page")]
        public ProjectItemGroupPage[] Page { get; set; }

        /// <remarks/>
        [XmlElement("ProjectReference")]
        public ProjectItemGroupProjectReference[] ProjectReference { get; set; }

        /// <remarks/>
        [XmlElement("Compile")]
        public ProjectItemGroupCompile[] Compile { get; set; }

        /// <remarks/>
        [XmlElement("Reference")]
        public ProjectItemGroupReference[] Reference { get; set; }


        public List<ProjectFile> SourceFiles(string projectPath)
        {
            var files = ExtractSourceFiles(Compile, ResourceType.SourceFile,projectPath);
            files.AddRange(ExtractSourceFiles(EmbeddedResource, ResourceType.EmbeddedResource, projectPath));
            files.AddRange(ExtractSourceFiles(Resource, ResourceType.Resource, projectPath));


            files.AddRange(ExtractSourceFiles(ProjectReference, ResourceType.ProjectReference, projectPath));

            files.AddRange(ExtractSourceFiles(Reference, ResourceType.Reference, projectPath));
            if(Content != null) files.AddRange(ExtractSourceFiles(new [] { Content } , ResourceType.Content, projectPath));
            if (ApplicationDefinition != null) files.AddRange(ExtractSourceFiles(new[] { ApplicationDefinition }, ResourceType.ApplicationDefinition, projectPath));
            
            files.AddRange(ExtractSourceFiles(Page,ResourceType.Page, projectPath));
            files.AddRange(ExtractSourceFiles(None, ResourceType.Unknown, projectPath));
            return files;
        }



        private static List<ProjectFile> ExtractSourceFiles(IEnumerable<ProjectItemGroup> items, ResourceType resourceType, string projectPath)
        {
            var files = new List<ProjectFile>();
            if (items == null) return files;
            foreach (var item in items)
            {
                var resource = ProjectFile.CreateIfValid(item, resourceType, projectPath);
                if (resource != null)
                {
                    files.Add(resource);
                }
                else
                {
                    
                }
            }
            return files;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroup
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class EmbeddedResource : ProjectItemGroup
    {
        /// <remarks/>
        public string Generator { get; set; }

        /// <remarks/>
        public string LastGenOutput { get; set; }

        /// <remarks/>
        public string SubType { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupApplicationDefinition : ProjectItemGroup
    {
        /// <remarks/>
        public string Generator { get; set; }

        /// <remarks/>
        public string SubType { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupPage : ProjectItemGroup
    {
        /// <remarks/>
        public string SubType { get; set; }

        /// <remarks/>
        public string Generator { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupProjectReference : ProjectItemGroup
    {
        /// <remarks/>
        public string Project { get; set; }

        /// <remarks/>
        public string Name { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupCompile : ProjectItemGroup
    {
        /// <remarks/>
        public string AutoGen { get; set; }

        /// <remarks/>
        public string DesignTime { get; set; }

        /// <remarks/>
        public string DependentUpon { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupReference : ProjectItemGroup
    {
        /// <remarks/>
        public string HintPath { get; set; }

        /// <remarks/>
        public string Private { get; set; }
    }

}