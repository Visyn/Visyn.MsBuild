using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Visyn.Io;
using Visyn.Wpf.Console.ViewModel;

namespace Visyn.Build.Gac
{
    public class GacUtil
    {
        private static GacUtil _instance;

        public static GacUtil Instance => _instance ?? (_instance = new GacUtil());
        
        public static ICommand GacCommand { get; } = new RelayCommand<IOutputDeviceMultiline>(gacPrint, ((o) => (o != null)));

        private static void gacPrint(IOutputDeviceMultiline terminal)
        {
            var result = new List<string>();
            foreach (var file in GacUtil.Instance.GacFiles.Values)
            {
                result.Add($"{file.Name}\t{file.DirectoryName}");
            }
            terminal.Write(result);
        }

        private Dictionary<string, FileInfo> _gacFiles;
        public Dictionary<string, FileInfo> GacFiles
        {
            get { if(_gacFiles != null) return _gacFiles;
             _gacFiles = GetGlobalAssemblyCacheFiles(null);
#if false
                FileIO temp = new FileIO(FileIO.GetTempFileName("csv"), null) { OpenWithDefaultProgram = true };
                temp.SaveDelimitedFileToDisk(_gacFiles, ",");
#endif
                return _gacFiles;
            }
        }

        public string AssemblyPath(string assembly)
        {
            if(assembly.EndsWith(".dll"))
            {
                assembly = assembly.Substring(0, assembly.Length - 4);
            }
            if(GacFiles.ContainsKey(assembly))
            {
                return GacFiles[assembly].DirectoryName;
            }
            var ass = Path.GetFileNameWithoutExtension(assembly);
            var nativeAssembly = assembly + ".ni";
            if(GacFiles.ContainsKey(nativeAssembly))
            {
                return GacFiles[nativeAssembly].DirectoryName;
            }
            return "";
        }

        public static Dictionary<string, FileInfo> GetGlobalAssemblyCacheFiles(string path=null)
        {
            if(path == null) path = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\assembly";

            var di = new DirectoryInfo(path);

//            var files = di.GetFiles("*.dll").Select(fi => fi.FullName).ToList();

            var duplicates = new List<string>();
            var dictionary = new Dictionary<string, FileInfo>();
            foreach (var file in di.GetFiles())
            {
                if(file.Extension == ".dll") dictionary.Add(file.Name.Substring(0,file.Name.Length-4), file);
                else
                {
                    if(file.Name != "__AssemblyInfo__.ini")
                    {

                    }
                }
            }
            foreach (var file in di.GetDirectories().Select(diChild => GetGlobalAssemblyCacheFiles(diChild.FullName)).SelectMany(more => more))
            {
                if(!dictionary.ContainsKey(file.Key)) dictionary.Add(file.Key,file.Value);
                else
                {
                    var first = dictionary[file.Key];
                    duplicates.Add(file.Key);
                }
            }
            if(duplicates.Count > 0)
            {

            }

            return dictionary;
        }

        public static FileVersionInfo Info(string path)
        {
            if (File.Exists(path))
            {
                // Get the file version for the notepad.
                return FileVersionInfo.GetVersionInfo(path);
            }
            return null;
        }

        public static List<string> GacFolders()
        {
            var result = new List<string>();
            // List of all the different types of GAC folders for both 32bit and 64bit
            // environments.
            var gacFolders = new List<string>() {
                "GAC", "GAC_32", "GAC_64", "GAC_MSIL",
                "NativeImages_v2.0.50727_32",
                "NativeImages_v2.0.50727_64",
                "NativeImages_v4.0.30319_32",
                "NativeImages_v4.0.30319_64"
            };

            foreach (var folder in gacFolders)
            {
                var path = Path.Combine(
                   Environment.ExpandEnvironmentVariables(@"%systemroot%\assembly"),
                   folder);

                if (Directory.Exists(path))
                {
                    result.Add(path);
                    //Response.Write("<hr/>" + folder + "<hr/>");

                    string[] assemblyFolders = Directory.GetDirectories(path);
                    foreach (var assemblyFolder in assemblyFolders)
                    {
                        result.Add(assemblyFolder);
                    }
                }
            }
            return result;
        }
    }
}
