using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio.VCxProj
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class PropertyGroup
    {
        /// <remarks/>
        [XmlElement("CharacterSet", typeof(string))]
        [XmlElement("ConfigurationType", typeof(string))]
        [XmlElement("IntDir", typeof(ConditionValue))]
        [XmlElement("LinkIncremental", typeof(ConditionValue))]
        [XmlElement("OutDir", typeof(ConditionValue))]
        [XmlElement("PlatformToolset", typeof(string))]
        [XmlElement("ProjectGuid", typeof(string))]
        [XmlElement("ProjectName", typeof(string))]
        [XmlElement("UseOfMfc", typeof(bool))]
        [XmlElement("_ProjectFileVersion", typeof(string))]
        [XmlElement("FreeRTOSDir", typeof(string))]
        [XmlElement("LightspeedIncludes", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        /// <remarks/>
        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Label { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}