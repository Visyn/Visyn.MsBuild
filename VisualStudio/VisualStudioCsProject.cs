using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml.Serialization;
using ProtoLib.Util.Xml;

namespace Visyn.Build.VisualStudio
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, 
        ElementName = "Project")]
    public class VisualStudioCsProject
    {
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

        [XmlIgnore]
        public List<string> SourceFiles { get; private set; } = new List<string>();
        [XmlIgnore]
        public List<string> Dependencies { get; private set; } = new List<string>();
        [XmlIgnore]
        public List<string> Resources { get; private set; } = new List<string>();

        public static VisualStudioCsProject Deserialize(string fileName, Action<object, Exception> exceptionHandler)
        {
            var project = XmlIO.Deserialize<VisualStudioCsProject>(fileName, exceptionHandler);
            if (project == null) return null;
            foreach (var item in project.Items)
            {
                try
                {
                    var propertyGroup = item as PropertyGroup;
                    if (propertyGroup != null)
                    {
                        var assembly = propertyGroup.AssemblyName;
                        if (assembly != null)
                        {
                            project.Assemblies.Add(new VsAssemblyInfo(propertyGroup));
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
                                    project.References.Add(r);
                                }
                            }
                            continue;
                        }
                        if (itemGroup.EmbeddedResource != null)
                        {
                            foreach(ProjectItemGroupEmbeddedResource embedded in itemGroup.EmbeddedResource)
                            {
                                project.Resources.Add(embedded.Include);
                            }
                        }
                        if (itemGroup.Resource != null)
                        {

                        }
                        if (itemGroup.ProjectReference != null)
                        {

                        }
                        if(itemGroup.Compile != null)
                        {
                            foreach(var sourceFile in itemGroup.Compile)
                            {
                                if(!string.IsNullOrWhiteSpace(sourceFile.Include))
                                {
                                    project.SourceFiles.Add(sourceFile.Include);
                                }
                                if(!string.IsNullOrWhiteSpace(sourceFile.DependentUpon))
                                {
                                    project.Dependencies.Add(sourceFile.DependentUpon);
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    if (exceptionHandler != null) exceptionHandler(null, exc);
                    else throw;
                }

            }
            return project;
        }
    }

    public class VsAssemblyInfo
    {

        public string Name { get; }
        public string Version { get; }
        public VsAssemblyInfo(PropertyGroup group)
        {
            Name = group.AssemblyName;
            Version = group.AssemblyVersion;
        }

        public VsAssemblyInfo(string name,string version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }

        public static VsAssemblyInfo CreateIfValid(ProjectItemGroupReference reference)
        {
            try
            {
            if(!string.IsNullOrWhiteSpace(reference.Include))
            {
                var split = reference.Include.Split(',');
                if(!string.IsNullOrWhiteSpace(split[0]))
                {
                    return new VsAssemblyInfo(split[0].Trim(), split.Length > 1 ? split[1].Trim() : "");
                }

            }
            }
            catch (Exception) { /* Ignore */}
            return null;
        }
    }
}