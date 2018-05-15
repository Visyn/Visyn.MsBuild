﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Visyn.Collection;
using System.Linq;
using System.Diagnostics;

namespace Visyn.Build
{
    public class CommandLine
    {
        public static IReadOnlyDictionary<string, string> CommandLineArguments { get; private set; }
        public static bool Headless { get; private set; }
        public static bool Verbose { get; private set; }

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

                CommandLineArguments = new ReadOnlyDictionary<string, string>(dictionary);
                Verbose = CommandLineArguments.Keys.Contains("-verbose");
                Headless = CommandLineArguments.Keys.Contains("-headless");
            }
            return CommandLineArguments;
        }

        private static void AddArgument(IDictionary<string, string> dict, string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            dict.Add(key.ToLower(), value);
        }

        public static IList<string> ExecuteCommandLine()
        {
            if (CommandLineArguments?.Count > 0)
            {
                Debug.Assert(Verbose == CommandLineArguments.Keys.Contains("-verbose"));
                Debug.Assert(Headless == CommandLineArguments.Keys.Contains("-headless"));
                string logPath = null;
                ProjectFileBase project1 = null;
                ProjectFileBase project2 = null;

                List<string> results = new List<string>();
                try
                {
                    if (Verbose)
                    {
                        CommandLineArguments.ForEach((arg) => results.Add($"{arg.Key} {arg.Value}"));
                    }
                    List<string> omit = CommandLineArguments.ContainsKey("-omit") ?
                        CommandLineArguments["-omit"].Split(',').Select((o) => (o.Trim(' '))).ToList<string>()
                        : null;

                    foreach (var arg in CommandLineArguments)
                    {
                        string validPath = null;
                        if (Verbose)
                        {
                            results.Add(null);
                            results.Add($"{arg.Key} : {arg.Value}");
                        }

                        switch (arg.Key)
                        {
                            case "-verbose": break; // Verbose already handled
                            case "-headless": break; // Headless already handled
                            case "-log":
                                logPath = arg.Value;
                                break;
                            case "-open":
                                validPath = VerifyAndGetPath(arg.Value);
                                project1 = Commands.Open(validPath, null);
                                if (project1 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");
                                results.AddRange(project1.Results(Verbose));
                                break;
                            case "-save":
                                project1.Serialize(arg.Value, null);
                                results.AddRange(project1.Results(Verbose));
                                break;
                            case "-compare":
                                validPath = VerifyAndGetPath(arg.Value);
                                project2 = Commands.Open(validPath, null);
                                if (project2 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");
                                results.AddRange(project1.Compare(project2, Verbose));
                                break;
                            case "-merge":
                                validPath = VerifyAndGetPath(arg.Value);
                                project2 = Commands.Open(validPath, null);
                                if (project2 == null) throw new Exception($"Error processing argument {arg.Key} {arg.Value}");

                                results.AddRange(project1.Merge(project2, omit, Verbose));
                                break;
                        }
                    }
                }
                catch (Exception exc)
                {
                    results.Add($"{exc.GetType()} {exc.Message}");
                }
                if (!string.IsNullOrEmpty(logPath))
                {
                    File.WriteAllLines(logPath, results);
#if DEBUG
                    System.Diagnostics.Process.Start(logPath);
#endif
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
    }
}
