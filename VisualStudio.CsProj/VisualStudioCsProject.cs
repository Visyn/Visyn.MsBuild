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
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false, ElementName = "Project")]
    public class VisualStudioCsProject : VisualStudioProject
    {
        public static string FileFilter = "Visual Studio c# Project (*.csproj)|*.csproj";
        public override string FileType => "Visual Studio C# Project";

        /// <remarks/>
        [XmlElement("ProjectImport", typeof(ConditionalImport))]
        [XmlElement("ItemGroup", typeof(ItemGroup))]
        [XmlElement("PropertyGroup", typeof(PropertyGroup))]
        [XmlElement("Import", typeof(Import))]
        public object[] Items { get; set; }

        [XmlElement("Target", typeof(Target))]
        public object[] Targets { get; set; }
 
        [XmlIgnore]
        public List<VsAssemblyInfo> Assemblies { get; private set; } = new List<VsAssemblyInfo>();

        [XmlIgnore]
        public List<VsAssemblyInfo> References { get; private set; } = new List<VsAssemblyInfo>();


        public override IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string>();
            if (verbose)
            {
                result.Add($"Opened {FileType} File: {ProjectPath}");
                result.Add($"TargetFrameworkVersion {TargetFrameworkVersion}");
                result.Add($"Output {Assemblies.Count} Assemblies");
                result.AddRange(Assemblies.Select(assembly => '\t' + assembly.ToString()));
                result.Add($"Referenced {References.Count} Assemblies");
                result.AddRange(References.Select(assembly => '\t' + assembly.ToString()));
                result.Add($"Source Files {SourceFiles.Count}");
                result.AddRange(SourceFiles.Select(resource => '\t' + resource.ToString()));
                result.Add($"Dependancies {Dependencies.Count}");
                result.AddRange(Dependencies.Select(dependancy => '\t' + dependancy));
                result.Add($"Missing Files: {MissingFiles.Count}");
                result.AddRange(MissingFiles.Select(missing => '\t' + missing.Path));
            }
            else if(MissingFiles.Count > 0)
            {
                result.Add($"Opened {Path.GetFileName(ProjectPath)} Missing: {MissingFiles.Count}/{SourceFiles.Count} References: {References.Count}");
            }
            return result;
        }

        public override IEnumerable<string> Compare(ProjectFileBase project, bool verbose)
        {
            var csproject = project as VisualStudioCsProject;
            if (csproject == null) return base.Compare(project, verbose);

            return CsProjComparison.Compare(this, csproject, verbose);
        }

        /// <summary>
        /// Merges the specified project into the current project.
        /// </summary>
        /// <param name="project">The project whose contents are to be merged into the current project.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public override IEnumerable<string> Merge(ProjectFileBase project, IEnumerable<string> omit, bool verbose)
        {
            var csSourceProject = project as VisualStudioCsProject;
            if (csSourceProject == null) return base.Merge(project,omit, verbose);

            var result = new List<string>();
            if(verbose)
                result.AddRange( new string[]
                    {
                        $"Merge {this.FileType}s",
                        $"\t{ProjectFilename}",
                        $"\t{project.ProjectFilename}"
                    });

            var sourceToAdd = new List<ProjectFile>();
            var sourceToSkip = new List<ProjectFile>();
            var filteredSource = SourceFiles.Where((f)=>f.ResourceType == ResourceType.SourceFile).ToList();
            var sourcesToOmit = filteredSource.Select((f) => Path.GetFileName(f.FileName)).ToList();

            if(omit != null) sourcesToOmit.AddRange(omit);

            var csFilteredSource = csSourceProject.SourceFiles.Where((s) => s.ResourceType == ResourceType.SourceFile).ToList();
            foreach (var csSource in csFilteredSource)
            {
                var csFilename = Path.GetFileName(csSource.FileName);
                if (!sourcesToOmit.Contains(csFilename))
                    sourceToAdd.Add(csSource);
                else
                    sourceToSkip.Add(csSource);
            }
            if(verbose || sourceToSkip.Count > 0)
                result.Add($"Merging {ResourceType.SourceFile}s {Path.GetFileName(project.ProjectFilename)}=>{Path.GetFileName(this.ProjectFilename)} {sourceToAdd.Count}/{csFilteredSource.Count} files.");

            foreach (var item in Items)
            {
                var itemGroup = item as ItemGroup;
                if (itemGroup != null && itemGroup.Compile != null)
                {
                    var compile = new List<Compile>(itemGroup.Compile);
                    foreach(var source in sourceToAdd)
                    {
                       compile.Add(source.ToCompile(this.ProjectPath));
                    }
                    itemGroup.Compile = compile.ToArray();
                }
            }

            if(verbose)
            {
                foreach(var source in sourceToAdd)
                {
                    result.Add($"\tAdd:\t{source}");
                }
            }
            foreach (var source in sourceToSkip)
            {
                result.Add($"\tSkip:\t{source}");
            }
            return result;
        }

        public override void Serialize(string fileName, ExceptionHandler exceptionHandler)
        {
            XmlIO.Serialize<VisualStudioCsProject>(this,fileName, exceptionHandler);
        }

        public static VisualStudioCsProject Deserialize(string fileName, ExceptionHandler exceptionHandler)
        {
            var project = XmlIO.Deserialize<VisualStudioCsProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        protected override void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            ProjectFilename = fileName;
            //var path = ProjectPath;
            foreach (var item in Items)
            {
                try
                {
                    var propertyGroup = item as PropertyGroup;
                    if (propertyGroup != null)
                    {
                        var assembly = propertyGroup.AssemblyName;
                        if (assembly != null)
                        {
                            Assemblies.Add(new VsAssemblyInfo(propertyGroup));
                        }
                        if (!string.IsNullOrWhiteSpace(propertyGroup.TargetFrameworkVersion))
                            TargetFrameworkVersion = propertyGroup.TargetFrameworkVersion;
                        continue;
                    }
                    var itemGroup = item as ItemGroup;
                    if (itemGroup != null)
                    {
                        if (itemGroup?.Reference != null)
                        {
                            foreach (var reference in itemGroup.Reference)
                            {
                                var r = VsAssemblyInfo.CreateIfValid(reference,this);
                                if (r != null)
                                {
                                    References.Add(r);
                                }
                            }
                        }
                        SourceFiles.AddRange(itemGroup.SourceFiles(this));
                    }
                }
                catch (Exception exc)
                {
                    if (exceptionHandler != null) exceptionHandler(null, exc);
                    else throw;
                }
            }
            base.Analyze(fileName,exceptionHandler);
            foreach(var r in References)
            {
                if (!r.Exists)
                {
                    if(!string.IsNullOrWhiteSpace(r.Path)) MissingFiles.Add(new ProjectFile(r.Name, ResourceType.Reference, r.Path));
                    else MissingFiles.Add(new ProjectFile(r.Name, ResourceType.Reference, this));
                }
            }
        }
    }
}