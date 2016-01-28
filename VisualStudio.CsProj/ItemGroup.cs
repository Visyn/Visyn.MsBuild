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
        public ProjectReference[] ProjectReference { get; set; }

        /// <remarks/>
        [XmlElement("Compile")]
        public Compile[] Compile { get; set; }

        /// <remarks/>
        [XmlElement("Reference")]
        public Reference[] Reference { get; set; }


        public List<ProjectFile> SourceFiles(ProjectFileBase project)
        {
            var files = ExtractSourceFiles(Compile, ResourceType.SourceFile,project);
            files.AddRange(ExtractSourceFiles(EmbeddedResource, ResourceType.EmbeddedResource, project));
            files.AddRange(ExtractSourceFiles(Resource, ResourceType.Resource, project));

            files.AddRange(ExtractSourceFiles(ProjectReference, ResourceType.ProjectReference, project));

            files.AddRange(ExtractSourceFiles(Reference, ResourceType.Reference, project));
            if(Content != null) files.AddRange(ExtractSourceFiles(new [] { Content } , ResourceType.Content, project));
            if (ApplicationDefinition != null) files.AddRange(ExtractSourceFiles(new[] { ApplicationDefinition }, ResourceType.ApplicationDefinition, project));
            
            files.AddRange(ExtractSourceFiles(Page,ResourceType.Page, project));
            files.AddRange(ExtractSourceFiles(None, ResourceType.Unknown, project));
            return files;
        }



        private static List<ProjectFile> ExtractSourceFiles(IEnumerable<ProjectItemGroup> items, ResourceType resourceType, ProjectFileBase project)
        {
            var files = new List<ProjectFile>();
            if (items == null) return files;
            foreach (var item in items)
            {
                var resource = ProjectFile.CreateIfValid(item, resourceType, project);
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
}