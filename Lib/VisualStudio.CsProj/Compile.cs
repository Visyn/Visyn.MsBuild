using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Compile : ProjectItemGroup
    {
        /// <remarks/>
        public string AutoGen { get; set; }

        /// <remarks/>
        public string DesignTime { get; set; }

        /// <remarks/>
        public string DependentUpon { get; set; }

        public string Link { get; set; }
    }
}