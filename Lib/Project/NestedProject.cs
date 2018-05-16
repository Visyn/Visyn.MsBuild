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


using Visyn.Build.Wix;

namespace Visyn.Build
{
    public class NestedProject
    {
        public string Path { get; }

        public string Name { get; set; }
        public string Include { get; }
        public string Project { get; }

        public bool FileExists => System.IO.File.Exists(this.Path);
        public string Guid { get; set; }

        public NestedProject(ProjectFileBase project, ProjectItemGroupProjectReference reference)
        {
            Name = reference.Name;
            Include = reference.Include;
            Project = reference.Project;
            Path = project.GetFullPath(Include);
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, Include));
        }

        public NestedProject(ProjectFileBase project, string item)
        {
            Name = item;
            Path = project.GetFullPath(item);
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath,item));
        }

        public static bool IsValidProjectReference(ProjectItemGroupProjectReference reference)
        {
            return !string.IsNullOrWhiteSpace(reference?.Name);
        }
    }
}