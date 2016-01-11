using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {

        /// <remarks/>
        AdditionalIncludeDirectories,

        /// <remarks/>
        AssemblerListingLocation,

        /// <remarks/>
        BasicRuntimeChecks,

        /// <remarks/>
        DebugInformationFormat,

        /// <remarks/>
        DisableLanguageExtensions,

        /// <remarks/>
        FunctionLevelLinking,

        /// <remarks/>
        InlineFunctionExpansion,

        /// <remarks/>
        MinimalRebuild,

        /// <remarks/>
        ObjectFileName,

        /// <remarks/>
        Optimization,

        /// <remarks/>
        PrecompiledHeaderOutputFile,

        /// <remarks/>
        PreprocessorDefinitions,

        /// <remarks/>
        ProgramDataBaseFileName,

        /// <remarks/>
        RuntimeLibrary,

        /// <remarks/>
        StringPooling,

        /// <remarks/>
        SuppressStartupBanner,

        /// <remarks/>
        WarningLevel,
    }
}