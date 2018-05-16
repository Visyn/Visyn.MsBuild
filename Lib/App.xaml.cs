using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Visyn.Build.ServiceLocator;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IReadOnlyDictionary<string, string> CommandLineArguments => CommandLine.CommandLineArguments;
        public static bool Headless => CommandLine.Headless;

        protected override void OnStartup(StartupEventArgs args)
        {
            ApplicationServiceLocator.Register<Dispatcher>(Dispatcher.CurrentDispatcher,true);
            base.OnStartup(args);
            // Add the event handler for handling UI thread exceptions to the event.
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            CommandLine.ProcessStartupArguments(args);

            if (Headless)
            {
                var text = CommandLine.ExecuteCommandLine();
                foreach (var line in text) Console.WriteLine(line);
                Shutdown(1);
                return;
            }
            else
            {
                StartupUri = new Uri("View/MainWindow.xaml", UriKind.Relative);
            }
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
    }
}
