using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DevExpress.Xpf.Bars;
using Microsoft.Win32;
using Visyn.Util.Gac;
using Visyn.Build.Annotations;
using Visyn.Build.VisualStudio;
using Visyn.Build.VisualStudio.CsProj;
using Visyn.Build.VisualStudio.MsBuild;
using Visyn.Build.VisualStudio.sln;
using Visyn.Build.VisualStudio.VCxProj;
using Visyn.Build.Wix;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<VisualStudioProject> _visualStudioProjects = new List<VisualStudioProject>();
        private VisualStudioSolution _visualStudioSolution;
        public bool Verbose { get; set; } = false;
        public bool Recurse { get; set; } = true;

        public bool Summary { get; set; } = true;

        MsBuildProject MsBuildProject { get; set; }
        WixProject WixProject { get; set; }

        List<VisualStudioProject> VisualStudioProjects
        {
            get { return _visualStudioProjects; }
            set
            {
                _visualStudioProjects = value;
                RaisePropertyChangedEvent(nameof(VisualStudioProjects));
            }
        }

        public VisualStudioSolution VisualStudioSolution
        {
            get { return _visualStudioSolution; }
            set
            {
                _visualStudioSolution = value;
                RaisePropertyChangedEvent(nameof(VisualStudioSolution));
                RaisePropertyChangedEvent(nameof(SolutionLoaded));
            }
        }

        public List<ProjectFile> MissingFiles { get; set; } = new List<ProjectFile>();
        List<NestedProject> MissingProjects { get; } = new List<NestedProject>();

        public bool SolutionLoaded => VisualStudioSolution != null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void ExceptionDelegate(object sender, Exception exc);

        private bool ExceptionHandler(object sender, Exception exc)
        {
            terminal.AppendLine(exc.Message);
            return true;
        }


        private void openMenuItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"{VisualStudioSolution.FileFilter}|{MsBuildProject.FileFilter}|{VisualStudioCsProject.FileFilter}|{WixProject.FileFilter}"
            };
            if(dialog.ShowDialog(this) == true)
            {
                var filename = dialog.FileName;
                MissingProjects.Clear();
                MissingFiles.Clear();
                OpenProject(filename,Recurse);
                ShowSummary();
            }
        }

        private void compareMenuItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"{MsBuildProject.FileFilter}|{WixProject.FileFilter}"
            };
            if(dialog.ShowDialog(this)== true)
            {
                var filename = dialog.FileName;
                var project = OpenProject(filename,false);
                if(project != null)
                    ShowComparison(VisualStudioSolution, project);

            }
        }

        private void ShowComparison(VisualStudioSolution visualStudioSolution, ProjectFileBase project)
        {
            IEnumerable<string> result = visualStudioSolution.Compare(project);
            foreach(var line in result)
            {
                terminal.AppendLine(line);
            }
        }


        private ProjectFileBase OpenProject(string filename, bool recurse)
        {
            ProjectFileBase project=null;
            if (filename.EndsWith(".wixproj")) project = OpenWixProject(filename);
            else if (filename.EndsWith(".csproj")) project = OpenVisualStudioCsProject(filename);
            else if (filename.EndsWith(".vcxproj")) project = OpenVisualStudioVCxProject(filename);
            else if (filename.EndsWith(".proj")) project = OpenMsBuildProject(filename);
            else if (filename.EndsWith(".sln")) project = OpenVisualStudioSolution(filename);
            else ExceptionHandler(this,new Exception($"Un-registered file extension {System.IO.Path.GetExtension(filename)}"));
            if (project != null)
            {
                MissingProjects.AddRange(project.MissingProjects);
                MissingFiles.AddRange(project.MissingFiles);
                if (recurse)
                {
                    foreach (var nested in project.Projects)
                    {
                        OpenProject(nested.Path,recurse);
                    }
                }
            }
            return project;
        }



        private void ShowSummary()
        {
            if (!Summary) return;
            foreach (var project in VisualStudioProjects)
            {
                if (project == null) continue;
                var file = project.ProjectFilename.Split('\\').Last();
                terminal.AppendLine($"\t{file}\t.net {project.TargetFrameworkVersion}\tTools {project.ToolsVersion}");
            }
            terminal.AppendLine($"Missing Projects: [{MissingProjects.Count}]");
            foreach (var missing in MissingProjects)
            {
                terminal.AppendLine("\t" + missing.Path);
            }
            terminal.AppendLine($"Missing Files [{MissingFiles.Count}]");
            foreach (var file in MissingFiles)
            {
                terminal.AppendLine("\t" + file.Path);
            }
        }

        private ProjectFileBase OpenVisualStudioSolution(string filename)
        {
            VisualStudioSolution = VisualStudioSolution.Deserialize(filename, ExceptionHandler);
            if (VisualStudioSolution != null)
            {
                DisplayResults(VisualStudioSolution);
            }
            else
            {
                FileOpenFailed("Visual Studio Solution", filename);
            }
            return VisualStudioSolution;
        }

        private ProjectFileBase OpenMsBuildProject(string filename)
        {
            MsBuildProject = MsBuildProject.Deserialize(filename, ExceptionHandler);
            if (MsBuildProject != null)
            {
                DisplayResults( MsBuildProject);
            }
            else
            {
                FileOpenFailed("MsBuild Project",filename);
            }
            return MsBuildProject;
        }

        private ProjectFileBase OpenWixProject(string filename)
        {
            WixProject = WixProject.Deserialize(filename, ExceptionHandler);
            if (WixProject != null)
            {
                DisplayResults(WixProject);
            }
            else
            {
                FileOpenFailed("Wix Project",filename);
            }
            return WixProject;
        }
        private ProjectFileBase OpenVisualStudioCsProject(string filename)
        {
            var cSharpProject = VisualStudioCsProject.Deserialize(filename, ExceptionHandler);
            if (cSharpProject != null)
            {
                DisplayResults(cSharpProject);
                if (cSharpProject.ImportFailed) FileOpenFailed(cSharpProject.FileType, filename);
                VisualStudioProjects.Add(cSharpProject);
            }
            else
            {
                FileOpenFailed("Visual Studio C# Project", filename);
            }
            return cSharpProject;
        }

        private ProjectFileBase OpenVisualStudioVCxProject(string filename)
        {
            var cProject = VisualStudioVCxProject.Deserialize(filename, ExceptionHandler);
            if (cProject != null)
            {
                DisplayResults(cProject);
                if (cProject.ImportFailed) FileOpenFailed(cProject.FileType, filename);
                VisualStudioProjects.Add(cProject);
            }
            else
            {
                FileOpenFailed("Visual Studio C/C++ Project", filename);
            }
            return cProject;
        }

        private void DisplayResults(ProjectFileBase project)
        {
            var results = project.Results(Verbose);
            terminal.AppendLines(results);
        }

        private void FileOpenFailed(string filetype, string filename)
        {
            terminal.AppendLine($"Opening {filetype} failed! File: {filename}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChangedEvent([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void gacMenuItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var gac = GacUtil.Instance;
           // var files = ProtoLib.Util.Gac.GacUtil.GetGlobalAssemblyCacheFiles();
            foreach (var file in gac.GacFiles.Values)
            {
                if(file.Name.Contains("CData") || file.FullName.Contains("CData"))
                    terminal.AppendLine($"{file.Name}\t{file.DirectoryName}");
            }
            //terminal.AppendLines(files);
        }
    }
}
