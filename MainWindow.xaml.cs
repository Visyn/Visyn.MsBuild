using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ProtoLib.Util.Xml;
using Visyn.Build.VisualStudio;
using Visyn.Build.VisualStudio.MsBuild;
using Visyn.Build.Wix;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MsBuildProject MsBuildProject { get; set; }
        WixProject WixProject { get; set; }
        VisualStudioCsProject VisualStudioCsProject { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void ExceptionDelegate(object sender, Exception exc);

        private void ExceptionHanddler(object sender, Exception exc)
        {
            terminal.AppendLine(exc.Message);
        }


        private void openMenuItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = $"{MsBuildProject.FileFilter}|{VisualStudioCsProject.FileFilter}|{WixProject.FileFilter}"
            };
            if(dialog.ShowDialog(this) == true)
            {
                string filename = dialog.FileName;
                if(filename.EndsWith(".wixproj")) OpenWixProject(filename);
                else if (filename.EndsWith(".csproj")) OpenVisualStudioProject(filename);
                else if (filename.EndsWith(".proj")) OpenMsBuildProject(filename);
            }
        }

        private void OpenMsBuildProject(string filename)
        {
            MsBuildProject = MsBuildProject.Deserialize(filename, ExceptionHanddler);
            if (MsBuildProject != null)
            {
                terminal.AppendLine($"Opened Visual C# Project File: {Path.GetFileName(filename)}");
                terminal.AppendLine($"Projects: {MsBuildProject.VisualStudioProjects.Count}");
                foreach (var project in MsBuildProject.VisualStudioProjects)
                {
                    terminal.AppendLine($"\t{project.Name}");
                }
                terminal.AppendLine($"Missing Projects: {MsBuildProject.MissingProjects.Count}");
                foreach (var project in MsBuildProject.MissingProjects)
                {
                    terminal.AppendLine($"\t{project.Name}");
                }
            }
        }

        private void OpenVisualStudioProject(string filename)
        {

            VisualStudioCsProject = VisualStudioCsProject.Deserialize(filename, ExceptionHanddler);
            if(VisualStudioCsProject != null)
            {
                terminal.AppendLine($"Opened Visual C# Project File: {Path.GetFileName(filename)}");
                terminal.AppendLine($"Output {VisualStudioCsProject.Assemblies.Count} Assemblies");
                foreach(var assembly in VisualStudioCsProject.Assemblies)
                {
                    terminal.AppendLine('\t'+assembly.ToString());
                }
                terminal.AppendLine($"Referenced {VisualStudioCsProject.References.Count} Assemblies");
                foreach (var assembly in VisualStudioCsProject.References)
                {
                    terminal.AppendLine('\t' + assembly.ToString());
                }
                terminal.AppendLine($"Source Files {VisualStudioCsProject.Resources.Count}");
                foreach(var resource in VisualStudioCsProject.Resources)
                {
                    terminal.AppendLine('\t'+resource.ToString());
                }
                terminal.AppendLine($"Dependancies {VisualStudioCsProject.Dependencies.Count}");
                foreach (var dependancy in VisualStudioCsProject.Dependencies)
                {
                    terminal.AppendLine('\t' + dependancy);
                }

                terminal.AppendLine($"Missing Files: {VisualStudioCsProject.MissingFiles.Count}");
                foreach (var missing in VisualStudioCsProject.MissingFiles)
                {
                    terminal.AppendLine('\t' + missing.Path);
                }
            }
            return;
        }

        private void OpenWixProject(string filename)
        {
            WixProject = XmlIO.Deserialize<WixProject>(filename, ExceptionHanddler);
            if (WixProject != null)
            {
                var projects = WixProject.Projects;
                var shortFile = Path.GetFileName(filename);
                terminal.AppendLine($"Opened Wix Project File: {shortFile}");
                terminal.AppendLine($"Found {projects.Count} projects");
                foreach (var project in projects)
                {
                    terminal.AppendLine(project.Name);
                    terminal.AppendLine('\t' + project.Include);
                    terminal.AppendLine('\t' + project.Project);
                }
            }
        }
    }
}
