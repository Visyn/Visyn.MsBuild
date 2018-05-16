#region Copyright (c) 2015-2018 Visyn
// The MIT License(MIT)
// 
// Copyright (c) 2015-2018 Visyn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

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