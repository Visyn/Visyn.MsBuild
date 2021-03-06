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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Visyn.Exceptions;
using Visyn.Text;

namespace Visyn.Build.VisualStudio.sln
{
    public class VisualStudioSolution : ProjectFileBase
    {
        public static string FileFilter = "Visual Studio Solution (*.sln)|*.sln";
        public override string FileType => "Visual Studio Solution";
        private string _visualStudioVersion;
        private string _minimumVisualStudioVersion;
        public double FileVersion { get; }

        public VisualStudioSolution(string filename, double fileVersion)
        {
            ProjectFilename = filename;
            FileVersion = fileVersion;
        }

        public override IEnumerable<string> Results(bool verbose)
        {
            return base.Results(verbose);
        }

        public static VisualStudioSolution Deserialize(string filename, ExceptionHandler exceptionHandler)
        {
            if (!File.Exists(filename)) return null;
            var lines = File.ReadAllLines(filename);
            List<string> body;
            double fileVersion;
            if( FindSolutionHeader(lines, out body, out fileVersion))
            {
                var solution = new VisualStudioSolution(filename,fileVersion);
                solution.Parse(body, exceptionHandler);
                solution.Analyze(filename,exceptionHandler);
                return solution;
            }
            return null;
        }

        private bool Parse(List<string> body, ExceptionHandler exceptionHandler)
        {
            RemoveComments(body, "#");
            if (FileVersion > 11)
            {
                if (!ScanUntilKeyValuePair(ref body, "VisualStudioVersion", out _visualStudioVersion, exceptionHandler))
                    return false;
                if (string.IsNullOrWhiteSpace(VisualStudioVersion))
                    FileFormatException(exceptionHandler, $"Field VisualStudioVersion missing!");
                if (!ScanUntilKeyValuePair(ref body, "MinimumVisualStudioVersion", out _minimumVisualStudioVersion,exceptionHandler))
                    return false;
                if (string.IsNullOrWhiteSpace(MinimumVisualStudioVersion))
                    FileFormatException(exceptionHandler, $"Field MinimumVisualStudioVersion missing!");
            }
            while (ParseSection(body, exceptionHandler))
            {

            }
            return true;
        }


        private bool ParseSection(List<string> body, ExceptionHandler exceptionHandler)
        {
            if (body == null || body.Count == 0) return false;

            var line = body[0].Trim();
            while(string.IsNullOrWhiteSpace(line))
            {
                body.RemoveAt(0);
                if (body.Count == 0) return false;
                line = body[0].Trim();
            }
            if(line.StartsWith("Project"))
            {
                body.RemoveAt(0);
                KeyValuePair<string, string> kvp;
                if(line.TrySplitKeyValuePair(new []{ '=' } ,out kvp))
                {
                    var project = CreateVisualStudioProject(kvp.Value);
                    if (project == null)
                    {
                        FileFormatException(exceptionHandler, $"Project entry mal-formed '{line}'. Entry should have 3 ',' delimited items");
                        return false;
                    }
                    Projects.Add(project);
                    var projectBody = ExtractLinesUntil(body, "EndProject", exceptionHandler);
                    return projectBody != null;
                }
                else
                {
                    FileFormatException(exceptionHandler, $"Project entry mal-formed '{line}'.  Split('=') failed");
                }
            }
            else if(line.StartsWith("Global"))
            {
                var globalSection = ExtractLinesUntil(body, "EndGlobal", exceptionHandler);
                return globalSection != null;
            }
            return false;
        }


        private List<string> ExtractLinesUntil(List<string> body, string endLabel, ExceptionHandler exceptionHandler)
        {
            var section = new List<string>();

            while(body.Count > 0)
            {
                var line = body[0];
                body.RemoveAt(0);
                if (line.StartsWith(endLabel)) return section;
            }
            exceptionHandler(this,new FileFormatException(new Uri(ProjectFilename),$"End label missing {endLabel}"));
            return null;
        }

        private NestedProject CreateVisualStudioProject(string value)
        {
            char[] trimChars = {'"','\\'};
            char[] guidTrimChars = { '{','}' };
            var split = value.Split(',');
            if (split.Length != 3) return null;
            
            var project = new NestedProject(this,split[1].Trim().Trim('"').Trim('\\'))
            {
                Name = split[0].Trim().Trim('"').Trim('\\'),
                Guid = split[2].Trim().Trim('"').Trim('\\').Trim(guidTrimChars)
            };
            return project;
        }

        private bool ScanUntilKeyValuePair(ref List<string> lines, string key, out string value, ExceptionHandler exceptionHandler)
        { 
            while (lines.Count > 0)
            {
                var line = lines.First().Trim();
                lines.RemoveAt(0);
                if(line.Trim().StartsWith("#")) continue;
                if (!line.StartsWith(key)) continue;
                KeyValuePair<string, string> kvp;
                if (!line.TrySplitKeyValuePair(new[] {'='}, out kvp)) continue;
                value = kvp.Value.Trim();
                return true;
            }
            value = null;
            FileFormatException(exceptionHandler, $"Missing field: {key}");
            return false;
        }

        public string VisualStudioVersion
        {
            get { return _visualStudioVersion; }
            set { _visualStudioVersion = value; }
        }

        public string MinimumVisualStudioVersion
        {
            get { return _minimumVisualStudioVersion; }
            set { _minimumVisualStudioVersion = value; }
        }



        private static bool FindSolutionHeader(IEnumerable<string> lines, out List<string> body, out double fileVersion)
        {
            body = new List<string>();
            var headerFound = false;
            fileVersion = 0;
            foreach(var line in lines)
            {
                if(headerFound == false)
                {
                    if (!line.StartsWith("Microsoft Visual Studio Solution File")) continue;
                    KeyValuePair<string,string> kvp;
                    if (!line.TrySplitKeyValuePair(new[] {','}, out kvp)) continue;
                    var split = kvp.Value.Trim().Split(' ');
                    double.TryParse(split.Last(),out fileVersion);
                    headerFound = true;
                }
                else
                {
                    body.Add(line);
                }
            }
            return headerFound;
        }
    }
}
