using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemDefinitionGroup
    {
        /// <remarks/>
        public ProjectItemDefinitionGroupMidl Midl { get; set; }

        /// <remarks/>
        public ProjectItemDefinitionGroupClCompile ClCompile { get; set; }

        /// <remarks/>
        public ProjectItemDefinitionGroupResourceCompile ResourceCompile { get; set; }

        /// <remarks/>
        public ProjectItemDefinitionGroupLink Link { get; set; }

        /// <remarks/>
        public ProjectItemDefinitionGroupBscmake Bscmake { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}