using System.Collections.Generic;
using System.Xml.Serialization;

namespace Visyn.Build.Wix
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false)]
    public partial class Project
    {
        private object[] itemsField;

        private decimal toolsVersionField;

        private string defaultTargetsField;

        /// <remarks/>
        [XmlElement("Import", typeof(ProjectImport))]
        [XmlElement("ItemGroup", typeof(ProjectItemGroup))]
        [XmlElement("PropertyGroup", typeof(ProjectPropertyGroup))]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ToolsVersion
        {
            get { return toolsVersionField; }
            set { toolsVersionField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DefaultTargets
        {
            get { return defaultTargetsField; }
            set { defaultTargetsField = value; }
        }

        [XmlIgnore]
        public List<VisualStudioProject> Projects 
        {
            get
            {
                var list = new List<VisualStudioProject>();
                foreach (var item in Items)
                {
                    var itemGroup = item as ProjectItemGroup;
                    if (itemGroup?.ProjectReference != null)
                    {
                        foreach (var reference in itemGroup.ProjectReference)
                        {
                            if(VisualStudioProject.IsValidProjectReference(reference))
                            {
                                list.Add(new VisualStudioProject(reference));
                            }
                        }
                    }
                }
                return list;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectImport
    {
        private string projectField;

        /// <remarks/>
        [XmlAttribute()]
        public string Project
        {
            get
            {
                return projectField;
            }
            set
            {
                projectField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroup
    {

        private ProjectItemGroupProjectReference[] projectReferenceField;

        private ProjectItemGroupWixExtension[] wixExtensionField;

        private ProjectItemGroupContent[] contentField;

        private ProjectItemGroupFolder folderField;

        private ProjectItemGroupCompile[] compileField;

        /// <remarks/>
        [XmlElement("ProjectReference")]
        public ProjectItemGroupProjectReference[] ProjectReference
        {
            get
            {
                return projectReferenceField;
            }
            set
            {
                projectReferenceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("WixExtension")]
        public ProjectItemGroupWixExtension[] WixExtension
        {
            get
            {
                return wixExtensionField;
            }
            set
            {
                wixExtensionField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Content")]
        public ProjectItemGroupContent[] Content
        {
            get
            {
                return contentField;
            }
            set
            {
                contentField = value;
            }
        }

        /// <remarks/>
        public ProjectItemGroupFolder Folder
        {
            get
            {
                return folderField;
            }
            set
            {
                folderField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Compile")]
        public ProjectItemGroupCompile[] Compile
        {
            get
            {
                return compileField;
            }
            set
            {
                compileField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroupProjectReference
    {

        private string nameField;

        private string projectField;

        private string privateField;

        private string doNotHarvestField;

        private string refProjectOutputGroupsField;

        private string refTargetDirField;

        private bool referenceOutputAssemblyField;

        private bool referenceOutputAssemblyFieldSpecified;

        private string includeField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        public string Project
        {
            get
            {
                return projectField;
            }
            set
            {
                projectField = value;
            }
        }

        /// <remarks/>
        public string Private
        {
            get
            {
                return privateField;
            }
            set
            {
                privateField = value;
            }
        }

        /// <remarks/>
        public string DoNotHarvest
        {
            get
            {
                return doNotHarvestField;
            }
            set
            {
                doNotHarvestField = value;
            }
        }

        /// <remarks/>
        public string RefProjectOutputGroups
        {
            get
            {
                return refProjectOutputGroupsField;
            }
            set
            {
                refProjectOutputGroupsField = value;
            }
        }

        /// <remarks/>
        public string RefTargetDir
        {
            get
            {
                return refTargetDirField;
            }
            set
            {
                refTargetDirField = value;
            }
        }

        /// <remarks/>
        public bool ReferenceOutputAssembly
        {
            get
            {
                return referenceOutputAssemblyField;
            }
            set
            {
                referenceOutputAssemblyField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ReferenceOutputAssemblySpecified
        {
            get
            {
                return referenceOutputAssemblyFieldSpecified;
            }
            set
            {
                referenceOutputAssemblyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Include
        {
            get
            {
                return includeField;
            }
            set
            {
                includeField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroupWixExtension
    {

        private string hintPathField;

        private string nameField;

        private string includeField;

        /// <remarks/>
        public string HintPath
        {
            get
            {
                return hintPathField;
            }
            set
            {
                hintPathField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Include
        {
            get
            {
                return includeField;
            }
            set
            {
                includeField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroupContent
    {

        private string includeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Include
        {
            get
            {
                return includeField;
            }
            set
            {
                includeField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroupFolder
    {

        private string includeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Include
        {
            get
            {
                return includeField;
            }
            set
            {
                includeField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectItemGroupCompile
    {

        private string includeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Include
        {
            get
            {
                return includeField;
            }
            set
            {
                includeField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectPropertyGroup
    {

        private string preBuildEventField;

        private string outputPathField;

        private string intermediateOutputPathField;

        private string defineConstantsField;

        private ProjectPropertyGroupConfiguration configurationField;

        private ProjectPropertyGroupPlatform platformField;

        private decimal productVersionField;

        private bool productVersionFieldSpecified;

        private string projectGuidField;

        private decimal schemaVersionField;

        private bool schemaVersionFieldSpecified;

        private string outputNameField;

        private string outputTypeField;

        private ProjectPropertyGroupWixTargetsPath[] wixTargetsPathField;

        private string conditionField;

        /// <remarks/>
        public string PreBuildEvent
        {
            get
            {
                return preBuildEventField;
            }
            set
            {
                preBuildEventField = value;
            }
        }

        /// <remarks/>
        public string OutputPath
        {
            get
            {
                return outputPathField;
            }
            set
            {
                outputPathField = value;
            }
        }

        /// <remarks/>
        public string IntermediateOutputPath
        {
            get
            {
                return intermediateOutputPathField;
            }
            set
            {
                intermediateOutputPathField = value;
            }
        }

        /// <remarks/>
        public string DefineConstants
        {
            get
            {
                return defineConstantsField;
            }
            set
            {
                defineConstantsField = value;
            }
        }

        /// <remarks/>
        public ProjectPropertyGroupConfiguration Configuration
        {
            get
            {
                return configurationField;
            }
            set
            {
                configurationField = value;
            }
        }

        /// <remarks/>
        public ProjectPropertyGroupPlatform Platform
        {
            get
            {
                return platformField;
            }
            set
            {
                platformField = value;
            }
        }

        /// <remarks/>
        public decimal ProductVersion
        {
            get
            {
                return productVersionField;
            }
            set
            {
                productVersionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ProductVersionSpecified
        {
            get
            {
                return productVersionFieldSpecified;
            }
            set
            {
                productVersionFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ProjectGuid
        {
            get
            {
                return projectGuidField;
            }
            set
            {
                projectGuidField = value;
            }
        }

        /// <remarks/>
        public decimal SchemaVersion
        {
            get
            {
                return schemaVersionField;
            }
            set
            {
                schemaVersionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SchemaVersionSpecified
        {
            get
            {
                return schemaVersionFieldSpecified;
            }
            set
            {
                schemaVersionFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string OutputName
        {
            get
            {
                return outputNameField;
            }
            set
            {
                outputNameField = value;
            }
        }

        /// <remarks/>
        public string OutputType
        {
            get
            {
                return outputTypeField;
            }
            set
            {
                outputTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("WixTargetsPath")]
        public ProjectPropertyGroupWixTargetsPath[] WixTargetsPath
        {
            get
            {
                return wixTargetsPathField;
            }
            set
            {
                wixTargetsPathField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectPropertyGroupConfiguration
    {

        private string conditionField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectPropertyGroupPlatform
    {

        private string conditionField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public partial class ProjectPropertyGroupWixTargetsPath
    {

        private string conditionField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Condition
        {
            get
            {
                return conditionField;
            }
            set
            {
                conditionField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }


}
