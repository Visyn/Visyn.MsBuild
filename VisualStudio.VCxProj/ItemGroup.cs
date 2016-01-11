using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemGroup
    {
        /// <remarks/>
        [XmlElement("ClInclude")]
        public ClInclude[] ClInclude { get; set; }

        /// <remarks/>
        [XmlElement("ClCompile")]
        public ClCompile[] ClCompile { get; set; }

        /// <remarks/>
        [XmlElement("ProjectConfiguration")]
        public ProjectConfiguration[] ProjectConfiguration { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Label { get; set; }
    }
}