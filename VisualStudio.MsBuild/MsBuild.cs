using System;
using System.Collections.Generic;
using Visyn.Exceptions;
using Visyn.Util.Events;
using Visyn.Util.Xml;

namespace Visyn.Build.VisualStudio.MsBuild
{
    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, ElementName = "Project")]
    public class MsBuildProject : ProjectFileBase
    {
        public static string FileFilter = "MsBuild Project (*.proj) |*.proj";
        public override string FileType => "MsBuild Project";
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


        protected override void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            ProjectFilename = fileName;
            //var path = ProjectPath;
            foreach(var item in ItemGroup)
            {
                var projectPath = item.Include.Replace("$(MSBuildProjectDirectory)", ".");
                Projects.Add(new NestedProject(this, projectPath));
            }
            base.Analyze(fileName,exceptionHandler);
        }

        public static MsBuildProject Deserialize(string fileName, ExceptionHandler exceptionHandler)
        {
            var project = XmlIO.Deserialize<MsBuildProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        public override IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string> {$"Opened {FileType} File: {ProjectFilename}"};
            result.AddRange(base.Results(verbose));
            return result;
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
