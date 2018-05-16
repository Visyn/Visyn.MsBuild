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
using System.Linq;
using System.Xml.Serialization;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

namespace Visyn.Build.Wix
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", TypeName = "WixProject")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false, ElementName = "Project")]
    public class WixProject : ProjectFileBase
    {
        public static string FileFilter = "Wix Project(*.wixproj)|*.wixproj";
        public override string FileType => "Wix Project";
        /// <remarks/>
        [XmlElement("Import", typeof(ProjectImport))]
        [XmlElement("ItemGroup", typeof(ProjectItemGroup))]
        [XmlElement("PropertyGroup", typeof(ProjectPropertyGroup))]
        public object[] Items { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ToolsVersion { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string DefaultTargets { get; set; }

        public static WixProject Deserialize(string fileName, ExceptionHandler exceptionHandler)
        {
            var project = XmlIO.Deserialize<WixProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        protected override void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            ProjectFilename = fileName;
            foreach (var item in Items)
            {
                var itemGroup = item as ProjectItemGroup;
                if (itemGroup?.ProjectReference == null) continue;
                foreach (var reference in itemGroup.ProjectReference.Where(NestedProject.IsValidProjectReference))
                {
                    Projects.Add(new NestedProject(this,reference));
                }
            }
            base.Analyze(fileName,exceptionHandler);
        }

        public override IEnumerable<string> Results(bool verbose)
        {
            return base.Results(verbose);
        }
    }
}