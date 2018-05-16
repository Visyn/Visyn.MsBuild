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

namespace Visyn.Build
{
    public abstract class ProjectFileBase
    {
        [XmlIgnore]
        public string ProjectFilename { get; protected set; }
        [XmlIgnore]
        public string ProjectPath => Path.GetDirectoryName(ProjectFilename);
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

        protected virtual void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            foreach (var project in Projects)
            {
                if (!project.FileExists) MissingProjects.Add(project);
            }
        }

        protected void FileFormatException(ExceptionHandler exceptionHandler, string message)
        {
            ImportFailed = true;
            var exception = new FileFormatException(new Uri(ProjectFilename), message);
            if (exceptionHandler != null) exceptionHandler(this, exception);
            else throw exception;
        }

        public virtual IEnumerable<string> Results(bool verbose)
        {
            var result = new List<string>();
            if (verbose)
            {
                result.Add($"Opened {FileType} File: {ProjectFilename}");
                result.Add($"Projects: {Projects.Count}");
                result.AddRange(Projects.Select(project => $"\t{project.Path}"));
                result.Add($"Missing Projects: {MissingProjects.Count}");
                result.AddRange(MissingProjects.Select(project => $"\t{project.Name}\t{project.Path}"));
            }
            else if(MissingProjects.Count > 0)
            {
                result.Add($"{Path.GetFileName(ProjectFilename)} Projects: {Projects.Count} Missing: {MissingProjects.Count}");
            }
            if (ImportFailed) result.Add($"Opening {FileType} failed! File: {ProjectPath}");

            return result;
        }

        public virtual IEnumerable<string> Compare(ProjectFileBase project, bool verbose)
        {
            var result = new List<string>
            {
                $"Compare Projects",
                $"{this.FileType}:\t{ProjectFilename}",
                $"{project.FileType}:\t{project.ProjectFilename}"
            };

            var missing = this.Projects.Where(nested => project.FindProject(nested.Path) == null).ToList();
            result.Add($"Missing {missing.Count} projects");
            result.AddRange(missing.Select(nested => $"{nested.Path}"));

            return result;
        }
        /// <summary>
        /// Merges the specified project into the current project.
        /// </summary>
        /// <param name="project">The project whose contents are to be merged into the current project.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IEnumerable<string> Merge(ProjectFileBase project, IEnumerable<string> omit, bool verbose)
        {
            throw new NotImplementedException($"{GetType().Name}.{nameof(Merge)} not implemented!");
        }

        public virtual void Serialize(string fileName, ExceptionHandler exceptionHandler)
        {
            XmlIO.Serialize(GetType(),this, fileName, null,exceptionHandler);
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