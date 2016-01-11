using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Visyn.Build.VisualStudio.CsProj;
using Visyn.Build.VisualStudio.MsBuild;
using Visyn.Build.VisualStudio.sln;
using Visyn.Build.Wix;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Verbose { get; set; } = false;
        public bool Recurse { get; set; } = true;

        public bool Summary { get; set; } = true;

        MsBuildProject MsBuildProject { get; set; }
        WixProject WixProject { get; set; }
        List<VisualStudioCsProject> VisualStudioProjects { get; set; } = new List<VisualStudioCsProject>();
        public VisualStudioSolution VisualStudioSolution { get; set; }
        public List<ProjectFile> MissingFiles { get; set; } = new List<ProjectFile>();
        List<VisualStudioProject> MissingProjects { get; } = new List<VisualStudioProject>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void ExceptionDelegate(object sender, Exception exc);

        private void ExceptionHandler(object sender, Exception exc)
        {
            terminal.AppendLine(exc.Message);
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
                OpenProject(filename);
                ShowSummary();
            }
        }

        private void ShowSummary()
        {
            if(Summary)
            {
                terminal.AppendLine("Missing Projects");
                foreach(var missing in MissingProjects)
                {
                    terminal.AppendLine(missing.Path);
                }
            }
        }

        private void OpenProject(string filename)
        {
            ProjectFileBase project=null;
            if (filename.EndsWith(".wixproj")) project = OpenWixProject(filename);
            else if (filename.EndsWith(".csproj")) project = OpenVisualStudioCsProject(filename);
            else if (filename.EndsWith(".proj")) project = OpenMsBuildProject(filename);
            else if (filename.EndsWith(".sln")) project = OpenVisualStudioSolution(filename);
            if (project != null)
            {
                MissingProjects.AddRange(project.MissingProjects);
                MissingFiles.AddRange(project.MissingFiles);
                if (Recurse)
                {
                    foreach (var nested in project.VisualStudioProjects)
                    {
                        OpenProject(nested.Path);
                    }
                }
            }
        }


        //private void RecurseProjects(List<VisualStudioProject> projects)
        //{
        //    if (!Recurse) return;
        //    foreach (var project in projects)
        //    {
        //        OpenProject(project.Path);
        //    }
        //}

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

        private void DisplayResults(ProjectFileBase project)
        {
            var results = project.Results(Verbose);
            foreach (var line in results)
            {
                terminal.AppendLine(line);
            }
        }

        private void FileOpenFailed(string filetype, string filename)
        {
            terminal.AppendLine($"Opening {filetype} failed! File: {filename}");
        }
    }
}
