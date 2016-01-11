using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Import
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Project { get; set; }
    }
}