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

using System.Collections.Generic;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

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
