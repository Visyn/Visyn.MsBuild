using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupPage : ProjectItemGroup
    {
        /// <remarks/>
        public string SubType { get; set; }

        /// <remarks/>
        public string Generator { get; set; }
    }
}