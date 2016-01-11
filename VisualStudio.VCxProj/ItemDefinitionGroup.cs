using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemDefinitionGroup
    {
        /// <remarks/>
        public Midl Midl { get; set; }

        /// <remarks/>
        public ProjectItemDefinitionGroupClCompile ClCompile { get; set; }

        /// <remarks/>
        public ResourceCompile ResourceCompile { get; set; }

        /// <remarks/>
        public Link Link { get; set; }

        /// <remarks/>
        public Bscmake Bscmake { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}