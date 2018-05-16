using System;
using System.IO;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Win32;

using Visyn.Exceptions;
using Visyn.Collection;
using Visyn.Wpf.Console.ViewModel;
using Visyn.Build.VisualStudio;
using Visyn.Build.VisualStudio.CsProj;
using Visyn.Build.VisualStudio.MsBuild;
using Visyn.Build.VisualStudio.sln;
using Visyn.Build.VisualStudio.VCxProj;
using Visyn.Build.Wix;
using Visyn.Log;
using Visyn.Build.Gac;
using Visyn.Io;

namespace Visyn.Build.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ConsoleWithSeverityViewModel Terminal { get; }

        public MainViewModel MainViewModelProperty => this;

        #region UI Text
        public string OpenButtonText => "Open";
        public string CompareButtonText => "Compare";
        public string GacButtonText => "Gac";
        public string VerboseText => "Verbose";
        public string RecurseText => "Recurse";
        public string SummaryText => "Summary";
        #endregion UI Text

        #region UI Options
        public bool Verbose { get; set; }
        public bool Recurse { get; set; }

        public bool Summary { get; set; }
        
        #endregion  UI Options

        public ICommand OpenCommand { get; } = new RelayCommand<MainViewModel>(Open, (vm)=> vm != null);
        private static void Open(MainViewModel vm)
        {
            var dialog = new OpenFileDialog { Filter = $"{VisualStudioSolution.FileFilter}|{MsBuildProject.FileFilter}|{VisualStudioCsProject.FileFilter}|{WixProject.FileFilter}" };
            if (dialog.ShowDialog() == true)
            {
                vm.MissingProjects.Clear();
                vm.MissingFiles.Clear();
                vm.OpenProject(dialog.FileName, vm.Recurse);
                vm.ShowSummary();
            }
        }

        public ICommand GacCommand { get; } = new RelayCommand<IOutputDeviceMultiline>(gacPrint, ((o) => (o != null)));

        private static void gacPrint(IOutputDeviceMultiline terminal)
        {
            var result = new List<string>();
            foreach (var file in GacUtil.Instance.GacFiles.Values)
            {
                result.Add($"{file.Name}\t{file.DirectoryName}");
            }
            terminal.Write(result);
        }

        public ICommand LoadedCommand { get; } = new RelayCommand<IOutputDeviceMultiline>(CommandLine.ExecuteCommandLine);

        public ICommand CompareCommand { get; }
        public ICommand ClearCommand { get; } = new ClearCommand();

        #region Loaded projects

        private List<VisualStudioProject> _visualStudioProjects = new List<VisualStudioProject>();
        private VisualStudioSolution _visualStudioSolution;

        List<VisualStudioProject> VisualStudioProjects
        {
            get { return _visualStudioProjects; }
            set { Set(ref _visualStudioProjects, value); }
        }

        public VisualStudioSolution VisualStudioSolution
        {
            get { return _visualStudioSolution; }
            set { if(Set(ref _visualStudioSolution,value)) RaisePropertyChanged(nameof(SolutionLoaded)); }
        }

        public List<ProjectFile> MissingFiles { get; set; } = new List<ProjectFile>();
        List<NestedProject> MissingProjects { get; } = new List<NestedProject>();

        public bool SolutionLoaded => VisualStudioSolution != null;
        #endregion Loaded projects
        public MainViewModel(ConsoleWithSeverityViewModel console, IExceptionHandler handler) : base(handler)
        {
            Terminal = console;
            CompareCommand = new RelayCommand<ProjectFileBase>(Compare, ((p) => p != null));
            Verbose = CommandLine.Verbose;
        }


        private void Compare(ProjectFileBase baseProject)
        {
            if (baseProject != null)
            {
                var dialog = new OpenFileDialog
                {
                    Filter = $"{MsBuildProject.FileFilter}|{WixProject.FileFilter}"
                };
                if (dialog.ShowDialog() == true)
                {
                    var filename = dialog.FileName;
                    var project = OpenProject(filename, false);
                    if (project != null)
                    {
                        var result = baseProject.Compare(project, Verbose);
                        Terminal.Write(result);
                    }
                }
            }
        }


        private ProjectFileBase OpenProject(string filename, bool recurse)
        {
            ProjectFileBase project = null;
            if (filename.EndsWith(".wixproj")) project = OpenWixProject(filename);
            else if (filename.EndsWith(".csproj")) project = OpenVisualStudioCsProject(filename);
            else if (filename.EndsWith(".vcxproj")) project = OpenVisualStudioVCxProject(filename);
            else if (filename.EndsWith(".proj")) project = OpenMsBuildProject(filename);
            else if (filename.EndsWith(".sln")) project = VisualStudioSolution = OpenVisualStudioSolution(filename);

            else base.HandleException(this, new Exception($"Un-registered file extension {System.IO.Path.GetExtension(filename)}"));

            if (project != null)
            {
                Terminal.WriteLine($"{project.FileType} opened: {project.ProjectPath}");
                Terminal.Write(project.Results(Verbose));
                if (project.ImportFailed) FileOpenFailed(project.FileType, filename);

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
            else HandleException(this, new Exception($"File open failed! [{filename}]"));
            return project;
        }



        private void ShowSummary()
        {
            if (!Summary) return;
            foreach (var project in VisualStudioProjects)
            {
                if (project == null) continue;
                var file = Path.GetFileName(project.ProjectFilename);
                Terminal.WriteLine($"\t{file}\t.net {project.TargetFrameworkVersion}\tTools {project.ToolsVersion}");
            }
            var severity = MissingProjects.Count > 0 ? SeverityLevel.Warning : SeverityLevel.Informational;
            Terminal.WriteLine($"Missing Projects: [{MissingProjects.Count}]", severity);
            foreach (var missing in MissingProjects)
            {
                Terminal.WriteLine("\t" + missing.Path, severity);
            }
            severity = MissingFiles.Count > 0 ? SeverityLevel.Warning : SeverityLevel.Informational;
            Terminal.WriteLine($"Missing Files [{MissingFiles.Count}]", severity);
            foreach (var file in MissingFiles)
            {
                Terminal.WriteLine("\t" + file.Path, severity);
            }
        }


        private VisualStudioSolution OpenVisualStudioSolution(string filename)
        {
            var solution = VisualStudioSolution.Deserialize(filename, Terminal.HandleException);
            if (solution == null) FileOpenFailed("Visual Studio Solution", filename);

            return solution;
        }

        private MsBuildProject OpenMsBuildProject(string filename)
        {
            var msBuildProject = MsBuildProject.Deserialize(filename, Terminal.HandleException);
            if (msBuildProject == null) FileOpenFailed("MsBuild Project", filename);
            return msBuildProject;
        }

        private WixProject OpenWixProject(string filename)
        {
            var wixProject = WixProject.Deserialize(filename, Terminal.HandleException);
            if (wixProject == null) FileOpenFailed("Wix Project", filename);
            return wixProject;
        }

        private VisualStudioCsProject OpenVisualStudioCsProject(string filename)
        {
            var cSharpProject = VisualStudioCsProject.Deserialize(filename, Terminal.HandleException);
            if (cSharpProject != null)
            {
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
            var cProject = VisualStudioVCxProject.Deserialize(filename, Terminal.HandleException);
            if (cProject != null)
            {
                VisualStudioProjects.Add(cProject);
            }
            else
            {
                FileOpenFailed("Visual Studio C/C++ Project", filename);
            }
            return cProject;
        }


        private void FileOpenFailed(string filetype, string filename)
        {
            Terminal.WriteLine($"Opening {filetype} failed! File: {filename}",Log.SeverityLevel.Error);
        }
    }
}
