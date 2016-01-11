using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {

        /// <remarks/>
        AdditionalDependencies,

        /// <remarks/>
        AdditionalLibraryDirectories,

        /// <remarks/>
        GenerateDebugInformation,

        /// <remarks/>
        OutputFile,

        /// <remarks/>
        ProgramDatabaseFile,

        /// <remarks/>
        SubSystem,

        /// <remarks/>
        SuppressStartupBanner,

        /// <remarks/>
        TargetMachine,
    }
}