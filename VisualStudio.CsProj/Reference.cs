using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Reference : ProjectItemGroup
    {
        /// <remarks/>
        public string HintPath { get; set; }

        /// <remarks/>
        public string Private { get; set; }
    }
}