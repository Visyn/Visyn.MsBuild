using System;
using System.ComponentModel;
using Visyn.Exceptions;
using System.Windows.Input;
using Visyn.Wpf.Console.ViewModel;
using Visyn.Build.Gac;
using Visyn.Build.VisualStudio;
using Visyn.Build.VisualStudio.CsProj;
using Visyn.Build.VisualStudio.MsBuild;
using Visyn.Build.VisualStudio.sln;
using Visyn.Build.VisualStudio.VCxProj;
using Visyn.Build.Wix;
using System.Collections.Generic;
using Visyn.Log;
using System.IO;

namespace Visyn.Build.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string OpenButtonText => "Open";
        public string CompareButtonText => "Compare";
        public string GacButtonText => "Gac";
        public string VerboseText => "Verbose";
        public string RecurseText => "Recurse";
        public string SummaryText => "Summary";

        public bool Verbose { get; set; }
        public bool Recurse { get; set; }

        public bool Summary { get; set; }

        public ICommand OpenCommand { get; } //= new RelayCommand<string>(Open, new Func<string,bool>((s) => { return true; }));
        private void Open(string obj)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = $"{VisualStudioSolution.FileFilter}|{MsBuildProject.FileFilter}|{VisualStudioCsProject.FileFilter}|{WixProject.FileFilter}"
            };
            if (dialog.ShowDialog() == true)
            {
                var filename = dialog.FileName;
                MissingProjects.Clear();
                MissingFiles.Clear();
                OpenProject(filename, Recurse);
                ShowSummary();
            }
        }

        #region Loaded projects

        private List<VisualStudioProject> _visualStudioProjects = new List<VisualStudioProject>();
        private VisualStudioSolution _visualStudioSolution;

        List<VisualStudioProject> VisualStudioProjects
        {
            get { return _visualStudioProjects; }
            set
            {
                _visualStudioProjects = value;
                RaisePropertyChanged(nameof(VisualStudioProjects));
            }
        }

        public VisualStudioSolution VisualStudioSolution
        {
            get { return _visualStudioSolution; }
            set
            {
                _visualStudioSolution = value;
                RaisePropertyChanged(nameof(VisualStudioSolution));
                RaisePropertyChanged(nameof(SolutionLoaded));
            }
        }

        public List<ProjectFile> MissingFiles { get; set; } = new List<ProjectFile>();
        List<NestedProject> MissingProjects { get; } = new List<NestedProject>();

        public bool SolutionLoaded => VisualStudioSolution != null;
        #endregion Loaded projects
        public MainViewModel(ConsoleWithSeverityViewModel console, IExceptionHandler handler) : base(handler)
        {
            terminal = console; // new ConsoleWithSeverityViewModel(10000, Dispatcher);
            OpenCommand = new RelayCommand<string>(Open, new Func<string, bool>((s) => { return true; }));
            Verbose = CommandLine.Verbose;
        }

        ConsoleWithSeverityViewModel terminal;

        private void ShowComparison(VisualStudioSolution visualStudioSolution, ProjectFileBase project)
        {
            IEnumerable<string> result = visualStudioSolution.Compare(project, Verbose);
            foreach (var line in result)
            {
                terminal.WriteLine(line);
            }
        }


        private ProjectFileBase OpenProject(string filename, bool recurse)
        {
            ProjectFileBase project = null;
            if (filename.EndsWith(".wixproj")) project = OpenWixProject(filename);
            else if (filename.EndsWith(".csproj")) project = OpenVisualStudioCsProject(filename);
            else if (filename.EndsWith(".vcxproj")) project = OpenVisualStudioVCxProject(filename);
            else if (filename.EndsWith(".proj")) project = OpenMsBuildProject(filename);
            else if (filename.EndsWith(".sln")) project = OpenVisualStudioSolution(filename);
            else base.HandleException(this, new Exception($"Un-registered file extension {System.IO.Path.GetExtension(filename)}"));

            if (project != null)
            {
                MissingProjects.AddRange(project.MissingProjects);
                MissingFiles.AddRange(project.MissingFiles);
                if (recurse)
                {
                    foreach (var nested in project.Projects)
                    {
                        OpenProject(nested.Path, recurse);
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
                var file = Path.GetFileName(project.ProjectFilename);
                terminal.WriteLine($"\t{file}\t.net {project.TargetFrameworkVersion}\tTools {project.ToolsVersion}");
            }
            var severity = MissingProjects.Count > 0 ? SeverityLevel.Warning : SeverityLevel.Informational;
            terminal.WriteLine($"Missing Projects: [{MissingProjects.Count}]", severity);
            foreach (var missing in MissingProjects)
            {
                terminal.WriteLine("\t" + missing.Path, severity);
            }
            severity = MissingFiles.Count > 0 ? SeverityLevel.Warning : SeverityLevel.Informational;
            terminal.WriteLine($"Missing Files [{MissingFiles.Count}]", severity);
            foreach (var file in MissingFiles)
            {
                terminal.WriteLine("\t" + file.Path, severity);
            }
        }

        private ProjectFileBase OpenVisualStudioSolution(string filename)
        {
            VisualStudioSolution = VisualStudioSolution.Deserialize(filename, terminal.HandleException);
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
            var msBuildProject = MsBuildProject.Deserialize(filename, terminal.HandleException);
            if (msBuildProject != null)
            {
                DisplayResults(msBuildProject);
            }
            else
            {
                FileOpenFailed("MsBuild Project", filename);
            }
            return msBuildProject;
        }

        private ProjectFileBase OpenWixProject(string filename)
        {
            var wixProject = WixProject.Deserialize(filename, terminal.HandleException);
            if (wixProject != null)
            {
                DisplayResults(wixProject);
            }
            else
            {
                FileOpenFailed("Wix Project", filename);
            }
            return wixProject;
        }

        private VisualStudioCsProject OpenVisualStudioCsProject(string filename)
        {
            var cSharpProject = VisualStudioCsProject.Deserialize(filename, terminal.HandleException);
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

        private VisualStudioVCxProject OpenVisualStudioVCxProject(string filename)
        {
            var cProject = VisualStudioVCxProject.Deserialize(filename, terminal.HandleException);
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
            terminal.Write(results);
        }

        private void FileOpenFailed(string filetype, string filename)
        {
            terminal.WriteLine($"Opening {filetype} failed! File: {filename}",Log.SeverityLevel.Error);
        }
    }
}
