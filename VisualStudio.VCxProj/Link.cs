using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Link
    {
        /// <remarks/>
        [XmlElement("AdditionalDependencies", typeof(string))]
        [XmlElement("AdditionalLibraryDirectories", typeof(string))]
        [XmlElement("GenerateDebugInformation", typeof(bool))]
        [XmlElement("OutputFile", typeof(string))]
        [XmlElement("ProgramDatabaseFile", typeof(string))]
        [XmlElement("SubSystem", typeof(string))]
        [XmlElement("SuppressStartupBanner", typeof(bool))]
        [XmlElement("TargetMachine", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        /// <remarks/>
        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType2[] ItemsElementName { get; set; }
    }
}