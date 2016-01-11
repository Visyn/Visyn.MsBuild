using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Visyn.Build.VisualStudio
{
    public abstract class VisualStudioProject : ProjectFileBase
    {
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
                var _dependancies = new List<string>();
                foreach (var item in SourceFiles)
                {
                    if (string.IsNullOrWhiteSpace(item.Dependancy)) continue;
                    var dep = item.Dependancy.Trim();
                    if (!_dependancies.Contains(dep)) _dependancies.Add(dep);
                }
                return _dependancies;
            }
        }

        protected override void Analyze(string fileName, Action<object, Exception> exceptionHandler)
        {
            MissingFiles.AddRange(
                SourceFiles.Where(
                    source => (!source.FileExists && (source.ResourceType != ResourceType.Reference))));
        }
    }
}
