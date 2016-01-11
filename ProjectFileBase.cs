using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Build.VisualStudio.CsProj;

namespace Visyn.Build
{
    public abstract class ProjectFileBase
    {
        [XmlIgnore]
        public string ProjectFilename { get; protected set; }
        [XmlIgnore]
        public string ProjectPath => System.IO.Path.GetDirectoryName(ProjectFilename);
        [XmlIgnore]
        public abstract string FileType { get; }
        [XmlIgnore]
        public bool ImportFailed { get; private set; }
        [XmlIgnore]
        public List<VisualStudioProject> VisualStudioProjects { get; private set; } = new List<VisualStudioProject>();
        [XmlIgnore]
        public List<VisualStudioProject> MissingProjects { get; private set; } = new List<VisualStudioProject>();
        [XmlIgnore]
        public List<ProjectFile> MissingFiles { get; private set; } = new List<ProjectFile>();
        protected virtual void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            foreach (var project in VisualStudioProjects)
            {
                if (!project.FileExists) MissingProjects.Add(project);
            }
        }

        protected void FileFormatException(Action<object, Exception> exceptionHandler, string message)
        {
            ImportFailed = true;
            var exception = new FileFormatException(new Uri(ProjectFilename), message);
            if (exceptionHandler != null) exceptionHandler(this, exception);
            else throw exception;
        }

        public virtual IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string> { $"Opened {FileType} File: {ProjectFilename}" };
            if (verbose)
            {
                result.Add($"Projects: {VisualStudioProjects.Count}");
                result.AddRange(VisualStudioProjects.Select(project => $"\t{project.Name}"));
                result.Add($"Missing Projects: {MissingProjects.Count}");
                result.AddRange(MissingProjects.Select(project => $"\t{project.Name}\t{project.Path}"));
            }
            else
            {
                result.Add($"Projects: {VisualStudioProjects.Count} Missing: {MissingProjects.Count}");
            }
            if (ImportFailed) result.Add($"Opening {FileType} failed! File: {ProjectPath}");

            return result;
        }
    }
}