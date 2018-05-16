#region Copyright (c) 2015-2018 Visyn
// The MIT License(MIT)
// 
// Copyright (c) 2015-2018 Visyn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using Visyn.Exceptions;
using Visyn.Io;
using Visyn.Wpf.Console.ViewModel;
using Visyn.Build.ViewModel;

namespace Visyn.Build.ServiceLocator
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
