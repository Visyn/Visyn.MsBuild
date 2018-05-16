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
using System.IO;
using Visyn.Build.Gac;

namespace Visyn.Build.VisualStudio.CsProj
{
    public class VsAssemblyInfo
    {
        public string Name { get; }
        public string Version { get; }

        public bool Exists { get; }
        public string Path { get; }
        public VsAssemblyInfo(PropertyGroup group)
        {
            Name = group.AssemblyName;
            Version = group.AssemblyVersion;
        }

        public VsAssemblyInfo(string name,string hintPath, VisualStudioProject project, string version)
        {
            if(!string.IsNullOrWhiteSpace(hintPath))
            {
                Path = project.GetFullPath(hintPath);
//                Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectPath, hintPath));
            }
            if(!name.ToLower().EndsWith(".dll"))
            {
                Name = name;
            }
            else
            {
                Name = name.Substring(0,name.Length-4);
            }

            Version = version;
            if(!string.IsNullOrWhiteSpace(Path))
            {
                if(File.Exists(Path))
                {
                    Exists = true;
                }
                else
                {
                    Exists = false;
                }
            }
            else
            {
                var gac = GacUtil.Instance;
                string gacPath = gac.AssemblyPath(Name);
                if(!string.IsNullOrWhiteSpace(gacPath))
                {
                    Exists = true;
                    Path = gacPath;
                }
                else
                {
                    Exists = false;
                }
            }
        }
        

        public override string ToString()
        {
            return $"{Name} {Version}";
        }

        public static VsAssemblyInfo CreateIfValid(Reference reference, VisualStudioProject project)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(reference.Include))
                {
                    var split = reference.Include.Split(',');
                    if(!string.IsNullOrWhiteSpace(split[0]))
                    {
                        return new VsAssemblyInfo(split[0].Trim(), reference.HintPath, project, split.Length > 1 ? split[1].Trim() : "");
                    }

                }
            }
            catch (Exception) { /* Ignore */}
            return null;
        }
    }
}