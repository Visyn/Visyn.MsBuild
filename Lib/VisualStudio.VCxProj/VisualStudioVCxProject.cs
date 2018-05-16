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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false,
        ElementName = "Project")]
    public class VisualStudioVCxProject : VisualStudioProject
    {
        public static string FileFilter = "Visual C++ Project (*.vcxproj)|*.vcxproj";
        public override string FileType => "Visual Studio C\\C++ Project";
        /// <remarks/>
        [XmlElement("Import", typeof(Import))]
        [XmlElement("ImportGroup", typeof(ImportGroup))]
        [XmlElement("ItemDefinitionGroup", typeof(ItemDefinitionGroup))]
        [XmlElement("ItemGroup", typeof(ItemGroup))]
        [XmlElement("PropertyGroup", typeof(PropertyGroup))]
        public object[] Items { get; set; }

        public static VisualStudioVCxProject Deserialize(string fileName, ExceptionHandler exceptionHandler)
        {
            var project = XmlIO.Deserialize<VisualStudioVCxProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        public override IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string>() { $"Opened {FileType} File: {ProjectFilename}" };
            if (verbose)
            {
                result.Add($"Source Files {SourceFiles.Count}");
                result.AddRange(SourceFiles.Select(resource => '\t' + resource.ToString()));
                result.Add($"Dependancies {Dependencies.Count}");
                result.AddRange(Dependencies.Select(dependancy => '\t' + dependancy));
                result.Add($"Missing Files: {MissingFiles.Count}");
                result.AddRange(MissingFiles.Select(missing => '\t' + missing.Path));
            }
            else
            {
                result.Add($"Files: {SourceFiles.Count} Missing: {MissingFiles.Count}");
            }
            return result;
        }
        protected override void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            ProjectFilename = fileName;
            var projectPath = ProjectPath;
            foreach (var item in Items)
            {
                try
                {
                    var propertyGroup = item as PropertyGroup;
                    var itemGroup = item as ItemGroup;
                    var import = item as Import;
                    var importGroup = item as ImportGroup;
                    if (propertyGroup != null)
                    {
                        if(propertyGroup.Label == "Globals") ExtractGlobals(propertyGroup);
                    }
                    if (itemGroup != null)
                    {
                        ExtractSourceFiles(itemGroup.ClCompile);
                        ExtractSourceFiles(itemGroup.ClInclude);
                    }
                }
                catch (Exception exc)
                {
                    if (exceptionHandler != null) exceptionHandler(null, exc);
                    else throw;
                }
            }
            base.Analyze(fileName, exceptionHandler);
        }

        private void ExtractGlobals(PropertyGroup propertyGroup)
        {
            if (propertyGroup.Items != null && propertyGroup.ItemsElementName != null)
            {
                if (propertyGroup.Items.Length == propertyGroup.ItemsElementName.Length)
                {
                    for (var i = 0; i < propertyGroup.Items.Length; i++)
                    {
                        Globals.Add(propertyGroup.ItemsElementName[i].ToString(), propertyGroup.Items[i].ToString());
                    }
                }
            }
        }

        private void ExtractSourceFiles(IReadOnlyCollection<ClInclude> itemGroup)
        {
            if (itemGroup != null && itemGroup.Count > 0)
            {
                foreach (var compile in itemGroup)
                {
                    if (string.IsNullOrWhiteSpace(compile.Include)) continue;
                    var path = ProcessPath(compile.Include);
                    SourceFiles.Add(new ProjectFile(path, ResourceType.SourceFile, this));
                }
            }
        }
    }
}
