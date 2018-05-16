using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClCompile
    {
        /// <remarks/>
        public ProjectItemGroupClCompileAdditionalIncludeDirectories AdditionalIncludeDirectories { get; set; }

        /// <remarks/>
        [XmlElement("PreprocessorDefinitions")]
        public ProjectItemGroupClCompilePreprocessorDefinitions[] PreprocessorDefinitions { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }
}