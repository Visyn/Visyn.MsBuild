﻿#region Copyright (c) 2015-2018 Visyn
// The MIT License(MIT)
// 
// Copyright (c) 2015-2018 Visyn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Xml.Serialization;

namespace Visyn.Build.Wix
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectImport
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Project { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroup
    {
        /// <remarks/>
        [XmlElement("ProjectReference")]
        public ProjectItemGroupProjectReference[] ProjectReference { get; set; }

        /// <remarks/>
        [XmlElement("WixExtension")]
        public ProjectItemGroupWixExtension[] WixExtension { get; set; }

        /// <remarks/>
        [XmlElement("Content")]
        public ProjectItemGroup[] Content { get; set; }

        /// <remarks/>
        [XmlElement("Folder")]
        public ProjectItemGroupFolder Folder { get; set; }

        /// <remarks/>
        [XmlElement("Compile")]
        public ProjectItemGroupCompile[] Compile { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupProjectReference
    {
        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        public string Project { get; set; }

        /// <remarks/>
        public string Private { get; set; }

        /// <remarks/>
        public string DoNotHarvest { get; set; }

        /// <remarks/>
        public string RefProjectOutputGroups { get; set; }

        /// <remarks/>
        public string RefTargetDir { get; set; }

        /// <remarks/>
        public bool ReferenceOutputAssembly { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ReferenceOutputAssemblySpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupWixExtension
    {
        /// <remarks/>
        public string HintPath { get; set; }

        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupFolder
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupCompile
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroup
    {
        /// <remarks/>
        public string PreBuildEvent { get; set; }

        /// <remarks/>
        public string OutputPath { get; set; }

        /// <remarks/>
        public string IntermediateOutputPath { get; set; }

        /// <remarks/>
        public string DefineConstants { get; set; }

        /// <remarks/>
        public ProjectPropertyGroupConfiguration Configuration { get; set; }

        /// <remarks/>
        public ProjectPropertyGroupPlatform Platform { get; set; }

        /// <remarks/>
        public decimal ProductVersion { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ProductVersionSpecified { get; set; }

        /// <remarks/>
        public string ProjectGuid { get; set; }

        /// <remarks/>
        public decimal SchemaVersion { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool SchemaVersionSpecified { get; set; }

        /// <remarks/>
        public string OutputName { get; set; }

        /// <remarks/>
        public string OutputType { get; set; }

        /// <remarks/>
        [XmlElement("WixTargetsPath")]
        public ProjectPropertyGroupWixTargetsPath[] WixTargetsPath { get; set; }

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
        [XmlText()]
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
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupWixTargetsPath
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }


}
