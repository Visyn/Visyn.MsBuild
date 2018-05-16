using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ConditionalValue
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }
}