using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ConditionalImport
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Project { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}