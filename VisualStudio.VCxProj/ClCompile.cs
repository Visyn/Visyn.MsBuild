using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClCompile : ClInclude
    {
        /// <remarks/>
        public ConditionValue AdditionalIncludeDirectories { get; set; }

        /// <remarks/>
        [XmlElement("PreprocessorDefinitions")]
        public ConditionValue[] PreprocessorDefinitions { get; set; }

        /// <remarks/>
        //[XmlAttribute()]
        //public string Include { get; set; }
    }
}