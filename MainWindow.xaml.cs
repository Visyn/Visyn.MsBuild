using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ProtoLib.Util.Xml;
using Visyn.Build.Wix;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Project WixProject { get; set; }

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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Wix Project (*.wixproj)|*.wixproj|Visual Studio Project (*.vsproj)|*.vsproj";
            if(dialog.ShowDialog(this) == true)
            {
                openWixProject(dialog.FileName);
            }
        }

        private void openWixProject(string fileName)
        {
            WixProject = XmlIO.Deserialize<Project>(fileName, ExceptionHanddler);
            if (WixProject != null)
            {
                var projects = WixProject.Projects;
                var shortFile = Path.GetFileName(fileName);
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
