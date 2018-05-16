#region Copyright (c) 2015-2018 Visyn
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