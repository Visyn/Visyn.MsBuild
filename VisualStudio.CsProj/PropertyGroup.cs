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

namespace Visyn.Build.VisualStudio.CsProj
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

        public bool AllowUnsafeBlocks { get; set; }
        /// <remarks/>
        public ConditionalValue Configuration { get; set; }

        /// <remarks/>
        public ConditionalValue Platform { get; set; }

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
        public string ApplicationVersion { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Condition { get; set; }
    }
}