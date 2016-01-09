using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class PropertyGroup
    {
        /// <remarks/>
        public string StartupObject { get; set; }

        /// <remarks/>
        public bool DebugSymbols { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool DebugSymbolsSpecified { get; set; }

        /// <remarks/>
        public string DebugType { get; set; }

        /// <remarks/>
        public bool Optimize { get; set; }

        /// <remarks/>
        [XmlIgnore()]
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
        [XmlIgnore()]
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
        [XmlIgnore()]
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