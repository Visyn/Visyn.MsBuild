using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemDefinitionGroupResourceCompile
    {
        /// <remarks/>
        public string PreprocessorDefinitions { get; set; }

        /// <remarks/>
        public string Culture { get; set; }
    }
}