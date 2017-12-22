using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, 
        ElementName = "Project")]
    public class VisualStudioCsProject : VisualStudioProject
    {
        public static string FileFilter = "Visual Studio c# Project (*.csproj)|*.csproj";

        public override string FileType => "Visual Studio C# Project";

        /// <remarks/>
        [XmlElement("ProjectImport", typeof(Import))]
        [XmlElement("ItemGroup", typeof(ItemGroup))]
        [XmlElement("PropertyGroup", typeof(PropertyGroup))]
        public object[] Items { get; set; }

  
        [XmlIgnore]
        public List<VsAssemblyInfo> Assemblies { get; private set; } = new List<VsAssemblyInfo>();

        [XmlIgnore]
        public List<VsAssemblyInfo> References { get; private set; } = new List<VsAssemblyInfo>();


        public override IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string>() {$"Opened {FileType} File: {ProjectPath}"};
            if (verbose)
            {
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
            else
            {
                result.Add($"Files: {SourceFiles.Count} Missing: {MissingFiles.Count} References: {References.Count}");
            }
            return result;
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