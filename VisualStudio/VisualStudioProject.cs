using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Visyn.Public.Exceptions;
using Visyn.Util.Events;

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
