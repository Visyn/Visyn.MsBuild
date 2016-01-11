using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectImportGroupImport
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Project { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Label { get; set; }
    }
}