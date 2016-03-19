using System;
using System.IO;
using Visyn.Util.Gac;

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