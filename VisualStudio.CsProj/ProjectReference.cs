using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectReference : ProjectItemGroup
    {
        /// <remarks/>
        public string Project { get; set; }

        /// <remarks/>
        public string Name { get; set; }
    }
}