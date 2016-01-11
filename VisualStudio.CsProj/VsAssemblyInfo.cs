using System;

namespace Visyn.Build.VisualStudio.CsProj
{
    public class VsAssemblyInfo
    {
        public string Name { get; }
        public string Version { get; }
        public VsAssemblyInfo(PropertyGroup group)
        {
            Name = group.AssemblyName;
            Version = group.AssemblyVersion;
        }

        public VsAssemblyInfo(string name,string version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }

        public static VsAssemblyInfo CreateIfValid(ProjectItemGroupReference reference)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(reference.Include))
                {
                    var split = reference.Include.Split(',');
                    if(!string.IsNullOrWhiteSpace(split[0]))
                    {
                        return new VsAssemblyInfo(split[0].Trim(), split.Length > 1 ? split[1].Trim() : "");
                    }

                }
            }
            catch (Exception) { /* Ignore */}
            return null;
        }
    }
}