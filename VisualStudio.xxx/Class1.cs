using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Import
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Project { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemGroup
    {
        /// <remarks/>
        [XmlElement("None")]
        public ProjectItemGroupNone[] None { get; set; }

        /// <remarks/>
        [XmlElement("EmbeddedResource")]
        public ProjectItemGroupEmbeddedResource[] EmbeddedResource { get; set; }

        /// <remarks/>
        [XmlElement("Resource")]
        public ProjectItemGroupResource[] Resource { get; set; }

        /// <remarks/>
        public ProjectItemGroupContent Content { get; set; }

        /// <remarks/>
        public ProjectItemGroupApplicationDefinition ApplicationDefinition { get; set; }

        /// <remarks/>
        [XmlElement("Page")]
        public ProjectItemGroupPage[] Page { get; set; }

        /// <remarks/>
        [XmlElement("ProjectReference")]
        public ProjectItemGroupProjectReference[] ProjectReference { get; set; }

        /// <remarks/>
        [XmlElement("Compile")]
        public ProjectItemGroupCompile[] Compile { get; set; }

        /// <remarks/>
        [XmlElement("Reference")]
        public ProjectItemGroupReference[] Reference { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupNone
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupEmbeddedResource
    {
        /// <remarks/>
        public string Generator { get; set; }

        /// <remarks/>
        public string LastGenOutput { get; set; }

        /// <remarks/>
        public string SubType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupResource
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupContent
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupApplicationDefinition
    {
        /// <remarks/>
        public string Generator { get; set; }

        /// <remarks/>
        public string SubType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupPage
    {
        /// <remarks/>
        public string SubType { get; set; }

        /// <remarks/>
        public string Generator { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupProjectReference
    {
        /// <remarks/>
        public string Project { get; set; }

        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupCompile
    {
        /// <remarks/>
        public string AutoGen { get; set; }

        /// <remarks/>
        public string DesignTime { get; set; }

        /// <remarks/>
        public string DependentUpon { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupReference
    {
        /// <remarks/>
        public string HintPath { get; set; }

        /// <remarks/>
        public string Private { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class PropertyGroup
    {
        /// <remarks/>
        public string StartupObject { get; set; }

        /// <remarks/>
        public bool DebugSymbols { get; set; }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool DebugSymbolsSpecified { get; set; }

        /// <remarks/>
        public string DebugType { get; set; }

        /// <remarks/>
        public bool Optimize { get; set; }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool OptimizeSpecified { get; set; }

        /// <remarks/>
        public string OutputPath { get; set; }

        /// <remarks/>
        public string DefineConstants { get; set; }

        /// <remarks/>
        public string ErrorReport { get; set; }

        /// <remarks/>
        public byte WarningLevel { get; set; }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool WarningLevelSpecified { get; set; }

        /// <remarks/>
        public ProjectPropertyGroupConfiguration Configuration { get; set; }

        /// <remarks/>
        public ProjectPropertyGroupPlatform Platform { get; set; }

        /// <remarks/>
        public string ProjectGuid { get; set; }

        /// <remarks/>
        public string OutputType { get; set; }

        /// <remarks/>
        public string AppDesignerFolder { get; set; }

        /// <remarks/>
        public string RootNamespace { get; set; }

        /// <remarks/>
        public string AssemblyName { get; set; }

        /// <remarks/>
        public string TargetFrameworkVersion { get; set; }

        /// <remarks/>
        public ushort FileAlignment { get; set; }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool FileAlignmentSpecified { get; set; }

        /// <remarks/>
        public string AssemblyVersion { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupConfiguration
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupPlatform
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public string Value { get; set; }
    }


}
