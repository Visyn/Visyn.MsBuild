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

using Visyn.Build.VisualStudio.CsProj;

namespace Visyn.Build
{
    public class ProjectFile
    {
        public ResourceType ResourceType { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }

        public bool FileExists => System.IO.File.Exists(this.Path);

        public string Dependancy { get; private set; }
        public ProjectFile(string fileName, ResourceType resourceType, ProjectFileBase project)
        {
            FileName = fileName;
            Path = project.GetFullPath(FileName);
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, FileName)); 
            ResourceType = resourceType;
        }

        public ProjectFile(string fileName, ResourceType resourceType, string path)
        {
            FileName = fileName;
            Path = path;
            //Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, FileName)); 
            ResourceType = resourceType;
        }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Dependancy) ?
                $"{ResourceType}\t{FileName}\tDependant: {Dependancy}" :
                $"{ResourceType}\t{FileName}";
        }

        public  Compile ToCompile(string projectPath)
        {
            // public string AutoGen { get; set; }
            // public string DesignTime { get; set; }
            // public string DependentUpon { get; set; }
            // public string Include { get; set; }
            var path = this.Path;
            if(this.Path.Contains(projectPath))
            {
                path = this.Path.Replace(projectPath, "");
            }
            return new Compile() { Include = path };
        }

        public static ProjectFile CreateIfValid(ProjectItemGroup item, ResourceType resourceType, ProjectFileBase project)
        {
            return !string.IsNullOrWhiteSpace(item?.Include) ? 
                new ProjectFile(item.Include, resourceType, project) { Dependancy = (item as Compile)?.DependentUpon } :
                null;
        }
    }
}