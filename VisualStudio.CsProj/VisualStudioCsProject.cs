﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ProtoLib.Util.Xml;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, 
        ElementName = "Project")]
    public class VisualStudioCsProject : ProjectFileBase
    {
        public static string FileFilter = "Visual Studio c# Project (*.csproj)|*.csproj";

        public override string FileType => "Visual Studio C# Project";

        /// <remarks/>
        [XmlElement("ProjectImport", typeof(Import))]
        [XmlElement("ItemGroup", typeof(ItemGroup))]
        [XmlElement("PropertyGroup", typeof(PropertyGroup))]
        public object[] Items { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ToolsVersion { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string DefaultTargets { get; set; }
  
        [XmlIgnore]
        public List<VsAssemblyInfo> Assemblies { get; private set; } = new List<VsAssemblyInfo>();

        [XmlIgnore]
        public List<VsAssemblyInfo> References { get; private set; } = new List<VsAssemblyInfo>();

        private List<string> _dependancies;
        [XmlIgnore]
        public List<string> Dependencies
        {
            get
            {
                _dependancies = new List<string>();
                foreach (var item in Resources)
                {
                    if (string.IsNullOrWhiteSpace(item.Dependancy)) continue;
                    var dep = item.Dependancy.Trim();
                    if(!_dependancies.Contains(dep)) _dependancies.Add(dep);
                }
                return _dependancies;
            }
        }

        [XmlIgnore]
        public List<ProjectFile> Resources { get; private set; } = new List<ProjectFile>();

        public override IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string>() {$"Opened {FileType} File: {ProjectPath}"};
            if (verbose)
            {
                result.Add($"Output {Assemblies.Count} Assemblies");
                result.AddRange(Assemblies.Select(assembly => '\t' + assembly.ToString()));
                result.Add($"Referenced {References.Count} Assemblies");
                result.AddRange(References.Select(assembly => '\t' + assembly.ToString()));
                result.Add($"Source Files {Resources.Count}");
                result.AddRange(Resources.Select(resource => '\t' + resource.ToString()));
                result.Add($"Dependancies {Dependencies.Count}");
                result.AddRange(Dependencies.Select(dependancy => '\t' + dependancy));
                result.Add($"Missing Files: {MissingFiles.Count}");
                result.AddRange(MissingFiles.Select(missing => '\t' + missing.Path));
            }
            else
            {
                result.Add($"Files: {Resources.Count} Missing: {MissingFiles.Count} References: {References.Count}");
            }
            return result;
        }

        public static VisualStudioCsProject Deserialize(string fileName, Action<object, Exception> exceptionHandler)
        {
            var project = XmlIO.Deserialize<VisualStudioCsProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        private void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            ProjectFilename = fileName;
            var path = ProjectPath;
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
                        continue;
                    }
                    var itemGroup = item as ItemGroup;
                    if (itemGroup != null)
                    {
                        if (itemGroup?.Reference != null)
                        {
                            foreach (ProjectItemGroupReference reference in itemGroup.Reference)
                            {
                                var r = VsAssemblyInfo.CreateIfValid(reference);
                                if (r != null)
                                {
                                    References.Add(r);
                                }
                            }
                        }
                        Resources.AddRange(itemGroup.SourceFiles(path));
                    }
                }
                catch (Exception exc)
                {
                    if (exceptionHandler != null) exceptionHandler(null, exc);
                    else throw;
                }
            }


            MissingFiles.AddRange(Resources.Where(resource => (!resource.FileExists && (resource.ResourceType != ResourceType.Reference))));
        }
    }
}