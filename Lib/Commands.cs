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

using System;
using Visyn.Exceptions;

namespace Visyn.Build
{
    class Commands
    {
        public static ProjectFileBase Open(string filename, ExceptionHandler exceptionHandler)
        {
            ProjectFileBase project = null;
            if (filename.EndsWith(".wixproj")) project = Wix.WixProject.Deserialize(filename, exceptionHandler);
            else if (filename.EndsWith(".csproj")) project = VisualStudio.CsProj.VisualStudioCsProject.Deserialize(filename, exceptionHandler);
            else if (filename.EndsWith(".vcxproj")) project = VisualStudio.VCxProj.VisualStudioVCxProject.Deserialize(filename, exceptionHandler);
            else if (filename.EndsWith(".proj")) project = VisualStudio.MsBuild.MsBuildProject.Deserialize(filename, exceptionHandler);
            else if (filename.EndsWith(".sln")) project = VisualStudio.sln.VisualStudioSolution.Deserialize(filename, exceptionHandler);
            else exceptionHandler.Invoke(nameof(ProjectFile), new Exception($"Un-registered file extension {System.IO.Path.GetExtension(filename)}"));

            return project;
        }
    }
}
