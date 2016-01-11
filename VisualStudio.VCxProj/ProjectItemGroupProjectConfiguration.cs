using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectConfiguration
    {
        /// <remarks/>
        public string Configuration { get; set; }

        /// <remarks/>
        public string Platform { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }
}