using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ConditionalText
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Text { get; set; }
    }
}