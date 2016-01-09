using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProtoLib.Util.Xml;

namespace Visyn.Build.VisualStudio.MsBuild
{
    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, ElementName = "Project")]
    public class MsBuildProject : ProjectFileBase
    {
        public static string FileFilter = "MsBuild Project (*.proj) |*.proj";
        /// <remarks/>
        public ProjectPropertyGroup PropertyGroup { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Projects", IsNullable = false)]
        public ProjectProjects[] ItemGroup { get; set; }

        /// <remarks/>
        public ProjectImport Import { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string DefaultTargets { get; set; }

        [XmlIgnore]
        public List<VisualStudioProject> VisualStudioProjects { get; private set; } = new List<VisualStudioProject>();
        [XmlIgnore]
        public List<VisualStudioProject> MissingProjects { get; private set; } = new List<VisualStudioProject>();
        private void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            FileName = fileName;
            var path = Path;
            foreach(var item in ItemGroup)
            {
                var projectPath = item.Include.Replace("$(MSBuildProjectDirectory)", ".");
                VisualStudioProjects.Add(new VisualStudioProject(projectPath, path));
            }
            foreach(var project in VisualStudioProjects)
            {
                if(!project.FileExists) MissingProjects.Add(project);
            }
        }

        public static MsBuildProject Deserialize(string fileName, Action<object, Exception> exceptionHandler)
        {
            var project = XmlIO.Deserialize<MsBuildProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroup
    {
        /// <remarks/>
        public ProjectPropertyGroupConfiguration Configuration { get; set; }

        /// <remarks/>
        public ProjectPropertyGroupOutputPath OutputPath { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupConfiguration
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupOutputPath
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectProjects
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectImport
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Project { get; set; }
    }


}
