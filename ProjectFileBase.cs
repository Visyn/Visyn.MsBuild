using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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

        protected Dictionary<string, string> Globals { get; set; } = new Dictionary<string, string>();
        [XmlIgnore]
        public bool ImportFailed { get; private set; }
        [XmlIgnore]
        public List<NestedProject> Projects { get; private set; } = new List<NestedProject>();
        [XmlIgnore]
        public List<NestedProject> MissingProjects { get; private set; } = new List<NestedProject>();

        [XmlIgnore]
        public List<ProjectFile> SourceFiles { get; set; } = new List<ProjectFile>();
        [XmlIgnore]
        public List<ProjectFile> MissingFiles { get; private set; } = new List<ProjectFile>();

        public NestedProject FindProject(string path)
        {
            return Projects.FirstOrDefault(project => project.Path.Equals(path));
        }

        protected static void RemoveComments(List<string> body, string comment)
        {
            var comments = body.Where(line => line.Trim().StartsWith(comment)).ToList();
            foreach (var line in comments)
            {
                body.Remove(line);
            }
        }

        protected virtual void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            foreach (var project in Projects)
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
                result.Add($"Projects: {Projects.Count}");
                result.AddRange(Projects.Select(project => $"\t{project.Path}"));
                result.Add($"Missing Projects: {MissingProjects.Count}");
                result.AddRange(MissingProjects.Select(project => $"\t{project.Name}\t{project.Path}"));
            }
            else
            {
                result.Add($"Projects: {Projects.Count} Missing: {MissingProjects.Count}");
            }
            if (ImportFailed) result.Add($"Opening {FileType} failed! File: {ProjectPath}");

            return result;
        }

        public IEnumerable<string> Compare(ProjectFileBase project)
        {
            var result = new List<string>
            {
                $"{this.FileType}:\t{ProjectFilename}",
                $"{project.FileType}:\t{project.ProjectFilename}"
            };

            var missing = this.Projects.Where(nested => project.FindProject(nested.Path) == null).ToList();
            result.Add($"Missing {missing.Count} projects");
            result.AddRange(missing.Select(nested => $"{nested.Path}"));

            return result;
        }

        protected string ProcessPath(string path)
        {
            foreach (var global in Globals.Keys)
            {
                var match = "$(" + global + ")";
                if (path.Contains(match))
                {
                    path = path.Replace(match, Globals[global]);
                }
            }
            return path;
        }

        public string GetFullPath(string hintPath)
        {
            var path = ProcessPath(hintPath);
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(ProjectPath, hintPath));
        }
    }
}