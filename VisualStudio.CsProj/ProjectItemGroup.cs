using System.IO;
using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.CsProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroup
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }

        [XmlIgnore]
        public string FileName {
            get
            {
                if (string.IsNullOrEmpty(Include) || !Include.Contains("\\")) return Include;
                return Path.GetFileName(Include);
            }
        }
    }
}