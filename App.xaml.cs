using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using Visyn.Collection;
using System.Linq;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IReadOnlyDictionary<string, string> CommandLineArguments { get; private set; }
        public static bool Headless { get; internal set; }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            // Add the event handler for handling UI thread exceptions to the event.
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            CommandLineArguments = ProcessStartupArguments(args);
            var text = ProcessArgs();

            if (Headless)
            {
                foreach (var line in text) Console.WriteLine(line);
                Shutdown(1);
                return;
            }
            else
            {
                StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
        }

        public static IList<string> ProcessArgs()
        {
            if (CommandLineArguments?.Count > 0)
            {
                bool verbose = CommandLineArguments.Keys.Contains("-verbose");
                string logPath = null;
                ProjectFileBase project1 = null;
                ProjectFileBase project2 = null;

                List<string> results = new List<string>();
                try
                {
                    if (verbose)
                    {
                        CommandLineArguments.ForEach((arg) => results.Add($"{arg.Key} {arg.Value}"));
                    }
                    List<string> omit = CommandLineArguments.ContainsKey("-omit") ? CommandLineArguments["-omit"].Split(',').Select((o) => (o.Trim(new[] { ' ' }))).ToList<string>() : null;
                    //if (CommandLineArguments.ContainsKey("-omit"))
                    //{
                    //    omit = CommandLineArguments["-omit"].Split(',').Select((o) => (o.Trim(new[] { ' ' }))).ToList<string>();
                    //}

                    foreach (var arg in CommandLineArguments)
                    {
                        string validPath = null;
                        if (verbose)
                        {
                            results.Add(null);
                            results.Add($"{arg.Key} : {arg.Value}");
                        }

                        switch (arg.Key)
                        {
                            case "-verbose": break; // Verbose already handled
                            case "-headless":
                                Headless = true;
                                break;
                            case "-log":
                                logPath = arg.Value;
                                break;
                            case "-open":
                                validPath = VerifyAndGetPath(arg.Value);
                                project1 = Commands.Open(validPath, null);
                                if(project1 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");
                                results.AddRange(project1.Results(verbose));
                                break;
                            case "-save":
                                project1.Serialize(arg.Value, null);
                                results.AddRange(project1.Results(verbose));
                                break;
                            case "-compare":
                                validPath = VerifyAndGetPath(arg.Value);
                                project2 = Commands.Open(validPath, null);
                                if (project2 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");
                                results.AddRange(project1.Compare(project2, verbose));
                                break;
                            case "-merge":
                                validPath = VerifyAndGetPath(arg.Value);
                                project2 = Commands.Open(validPath, null);
                                if (project2 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");

                                results.AddRange(project1.Merge(project2, omit, verbose));
                                break;
                        }
                    }
                }
                catch(Exception exc)
                {
                    results.Add($"{exc.GetType()} {exc.Message}");
                }
                if (logPath != null)
                {
                    File.WriteAllLines(logPath, results);
                    System.Diagnostics.Process.Start(logPath);
                }
                return results;
            }
            return null;
        }

        private static string VerifyAndGetPath(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"File not found: {path}");
            return path;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs arg)
        {
            if(arg != null)
                Console.WriteLine($"Unhandled {arg.Exception?.GetType().Name} Exception from {sender?.GetType().Name}: {arg.Exception?.Message}");
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="args">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs args)
        {
            base.OnExit(args);
        }

        public static IReadOnlyDictionary<string, string> ProcessStartupArguments(StartupEventArgs args)
        {
            if (args != null && args.Args.Length > 0)
            {
                IDictionary<string, string> dictionary = new Dictionary<string, string>();
                var arguments = args.Args;
                string key = null;
                string value = null;
                foreach (var argument in arguments)
                {
                    if (argument.StartsWith("-"))
                    {
                        AddArgument(dictionary, key, value);
                        key = argument;
                        value = null;
                    }
                    else
                    {
                        value = argument;
                    }
                }

                AddArgument(dictionary, key, value);

                return new ReadOnlyDictionary<string, string>(dictionary);
            }
            return null;
        }

        private static void AddArgument(IDictionary<string, string> dict, string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            dict.Add(key.ToLower(), value);
        }
    }
}
