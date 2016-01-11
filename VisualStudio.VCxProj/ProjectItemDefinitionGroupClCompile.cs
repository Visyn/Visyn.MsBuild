using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemDefinitionGroupClCompile
    {
        /// <remarks/>
        [XmlElement("AdditionalIncludeDirectories", typeof(string))]
        [XmlElement("AssemblerListingLocation", typeof(string))]
        [XmlElement("BasicRuntimeChecks", typeof(string))]
        [XmlElement("DebugInformationFormat", typeof(string))]
        [XmlElement("DisableLanguageExtensions", typeof(bool))]
        [XmlElement("FunctionLevelLinking", typeof(bool))]
        [XmlElement("InlineFunctionExpansion", typeof(string))]
        [XmlElement("MinimalRebuild", typeof(bool))]
        [XmlElement("ObjectFileName", typeof(string))]
        [XmlElement("Optimization", typeof(string))]
        [XmlElement("PrecompiledHeaderOutputFile", typeof(string))]
        [XmlElement("PreprocessorDefinitions", typeof(string))]
        [XmlElement("ProgramDataBaseFileName", typeof(string))]
        [XmlElement("RuntimeLibrary", typeof(string))]
        [XmlElement("StringPooling", typeof(bool))]
        [XmlElement("SuppressStartupBanner", typeof(bool))]
        [XmlElement("WarningLevel", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        /// <remarks/>
        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType1[] ItemsElementName { get; set; }
    }
}