using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemDefinitionGroupMidl
    {
        /// <remarks/>
        public string TypeLibraryName { get; set; }

        /// <remarks/>
        public object HeaderFileName { get; set; }
    }
}