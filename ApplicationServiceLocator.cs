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
    public class ApplicationServiceLocator : IServiceLocator
    {
        public bool IsDataSource {get;set;} = true;
        public MainViewModel MainViewModel => GetInstance<MainViewModel>();

        private static ConcurrentDictionary<Type, object> _singletonDictionary { get; } = new ConcurrentDictionary<Type, object>();

        public static bool Register<TService>(TService instance) => _singletonDictionary.TryAdd(typeof(TService), instance);
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>() => (TService)GetInstance(typeof(TService));

        public object GetInstance(Type serviceType)
        {
            if (serviceType == typeof(IExceptionHandler)) serviceType = typeof(ConsoleWithSeverityViewModel);
            if (serviceType == typeof(IOutputDevice)) serviceType = typeof(ConsoleWithSeverityViewModel);

            object instance;
            if (_singletonDictionary.TryGetValue(serviceType, out instance)) return instance;

            instance = CreateInstance(serviceType);

            _singletonDictionary.TryAdd(serviceType, instance);
            return instance;
        }

        protected virtual object CreateInstance(Type serviceType)
        {
            if(serviceType == typeof(MainViewModel)) return new MainViewModel(GetInstance<ConsoleWithSeverityViewModel>(), GetInstance<IExceptionHandler>());
            if (serviceType == typeof(ConsoleWithSeverityViewModel)) return new ConsoleWithSeverityViewModel(10000, GetInstance<Dispatcher>());

            throw new NotImplementedException($"{nameof(GetInstance)} not implemented for type {serviceType.GetType()}!");
        }

        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }



        public TService GetInstance<TService>(string key)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
