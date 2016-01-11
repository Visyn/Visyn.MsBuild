using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Bscmake
    {
        /// <remarks/>
        public bool SuppressStartupBanner { get; set; }

        /// <remarks/>
        public string OutputFile { get; set; }
    }
}