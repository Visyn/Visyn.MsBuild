using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ProtoLib.Util.Xml;

namespace Visyn.Build.Wix
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", TypeName = "WixProject")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", 
        IsNullable = false, 
        ElementName = "Project")]
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

        //[XmlIgnore]
        //public List<VisualStudioProject> Projects
        //{
        //    get
        //    {
        //        var list = new List<VisualStudioProject>();
        //        foreach (var item in Items)
        //        {
        //            var itemGroup = item as ProjectItemGroup;
        //            if (itemGroup?.ProjectReference != null)
        //            {
        //                foreach (var reference in itemGroup.ProjectReference)
        //                {
        //                    if (VisualStudioProject.IsValidProjectReference(reference))
        //                    {
        //                        list.Add(new VisualStudioProject(reference));
        //                    }
        //                }
        //            }
        //        }
        //        return list;
        //    }
        //}

        public static WixProject Deserialize(string fileName, Action<object, Exception> exceptionHandler)
        {
            var project = XmlIO.Deserialize<WixProject>(fileName, exceptionHandler);
            if (project == null) return null;
            project.Analyze(fileName, exceptionHandler);
            return project;
        }

        protected override void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            ProjectFilename = fileName;
            foreach (var item in Items)
            {
                var itemGroup = item as ProjectItemGroup;
                if (itemGroup?.ProjectReference == null) continue;
                foreach (var reference in itemGroup.ProjectReference.Where(NestedProject.IsValidProjectReference))
                {
                    Projects.Add(new NestedProject(reference, ProjectPath));
                }
            }
            base.Analyze(fileName,exceptionHandler);
        }

        public override IEnumerable<string> Results(bool verbose)
        {
            return base.Results(verbose);
        }
        //public override IEnumerable<string> VerboseResult()
        //{
        //    //var projects = WixProject.Projects;
        //    //var shortFile = Path.GetFileName(filename);
        //    //terminal.AppendLine($"Opened Wix Project File: {shortFile}");
        //    //terminal.AppendLine($"Found {projects.Count} projects");
        //    //foreach (var project in projects)
        //    //{
        //    //    terminal.AppendLine(project.Name);
        //    //    terminal.AppendLine('\t' + project.Include);
        //    //    terminal.AppendLine('\t' + project.Project);
        //    //}
        //    return base.VerboseResult();
        //}
    }
}