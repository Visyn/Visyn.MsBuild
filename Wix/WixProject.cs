﻿
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