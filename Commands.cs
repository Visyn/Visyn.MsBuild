using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visyn.Build.VisualStudio.CsProj;
using Visyn.Exceptions;
using Visyn.Windows.Io.Xml;

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
