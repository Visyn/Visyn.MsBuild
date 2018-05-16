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

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Exceptions;

namespace Visyn.Build.VisualStudio
{
    public abstract class VisualStudioProject : ProjectFileBase
    {
        [XmlIgnore]
        public string TargetFrameworkVersion { get; set; }
        [XmlAttribute()]
        public string DefaultTargets { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ToolsVersion { get; set; }


        [XmlIgnore]
        public List<string> Dependencies
        {
            get
            {
                var dependancies = new List<string>();
                foreach (var item in SourceFiles)
                {
                    if (string.IsNullOrWhiteSpace(item.Dependancy)) continue;
                    var dep = item.Dependancy.Trim();
                    if (!dependancies.Contains(dep)) dependancies.Add(dep);
                }
                return dependancies;
            }
        }

        protected override void Analyze(string fileName, ExceptionHandler exceptionHandler)
        {
            MissingFiles.AddRange(
                SourceFiles.Where(
                    source => (!source.FileExists && (source.ResourceType != ResourceType.Reference))));
        }
    }
}
