using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ImportGroup
    {
        /// <remarks/>
        [XmlElement("Import")]
        public ProjectImportGroupImport[] Import { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Label { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}