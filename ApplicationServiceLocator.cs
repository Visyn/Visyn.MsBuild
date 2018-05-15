using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Threading;
using CommonServiceLocator;
using Visyn.Exceptions;
using Visyn.Io;
using Visyn.Wpf.Console.ViewModel;

namespace Visyn.Build.ViewModel
{
    public class ApplicationServiceLocator : Visyn.Wpf.Console.ServiceLocator.ServiceLocator
    {
        #region Public Properties
        public MainViewModel MainViewModel => GetInstance<MainViewModel>();

        #endregion Public Properties

        public override object GetInstance(Type serviceType)
        {
            if (serviceType == typeof(IExceptionHandler)) serviceType = typeof(ConsoleWithSeverityViewModel);
            if (serviceType == typeof(IOutputDevice)) serviceType = typeof(ConsoleWithSeverityViewModel);

            object instance;
            if (SingletonDictionary.TryGetValue(serviceType, out instance)) return instance;

            instance = CreateInstance(serviceType);

            SingletonDictionary.TryAdd(serviceType, instance);
            return instance;
        }

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="serviceType">Type of the specified service.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException">CreateInstance</exception>
        protected override object CreateInstance(Type serviceType)
        { 
            if(serviceType == typeof(MainViewModel)) return new MainViewModel(GetInstance<ConsoleWithSeverityViewModel>(), GetInstance<IExceptionHandler>());
            var result = base.CreateInstance(serviceType);
            if (result != null) return result;
            throw new NotImplementedException($"{nameof(GetInstance)} not implemented for type {serviceType.GetType()}!");
        }
    }
}
