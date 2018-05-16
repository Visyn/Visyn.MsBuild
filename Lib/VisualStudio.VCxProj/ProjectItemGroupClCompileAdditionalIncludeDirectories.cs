using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupClCompileAdditionalIncludeDirectories
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }
}