using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Visyn.Exceptions;
using Visyn.Threads;

namespace Visyn.Build.ViewModel
{
    /// <summary>
    /// Class ViewModelBase.
    /// </summary>
    /// <seealso cref="Visyn.Exceptions.IExceptionHandler" />
    public abstract class ViewModelBase : INotifyPropertyChanged, IExceptionHandler
    {
        /// <summary>
        /// Gets the dispatcher for this view model (typically UI Dispatcher).
        /// </summary>
        /// <value>The dispatcher.</value>
        public Dispatcher Dispatcher { get; }

        /// <summary>Occurs after a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the event handler for this view model.
        /// </summary>
        /// <value>The handler.</value>
        public IExceptionHandler Handler { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        protected ViewModelBase(IExceptionHandler handler)
        {
            Handler = handler;
            Dispatcher = Dispatcher.CurrentDispatcher;
        }


        /// <summary>
        /// Assigns a new value to the property. Invokes IPropertyChanged notification if property value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="backingField">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">Name of property for INotifyPropertyChanged</param>
        /// <returns>True if the value changed, false otherwise.</returns>
        protected bool Set<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue)) return false;
            if(!string.IsNullOrWhiteSpace(propertyName)) RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Begins invoke of IPropertyChanged notification if property value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="backingField">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="delayMs">Delay in milliseconds prior to raising property changed notification</param>
        /// <param name="propertyName">Name of property for INotifyPropertyChanged</param>
        /// <returns>True if the value changed, false otherwise.</returns>
        protected bool SetDelayedNotify<T>(ref T backingField, T newValue, double delayMs = 0, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue)) return false;
            if (!string.IsNullOrWhiteSpace(propertyName)) Dispatcher.DelayedBeginInvoke(TimeSpan.FromMilliseconds(delayMs), () => { RaisePropertyChanged(propertyName); });
            return true;
        }

        /// <summary>
        /// Assigns a new value to the property. Does not send notification messages.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change
        /// occurred.</param>
        /// <returns>True if the value changed, false otherwise.</returns>
        protected bool SetWithoutNotify<T>(ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) return false;
            field = newValue;
            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <exception cref="ArgumentException">RaisePropertyChanged - propertyName</exception>
        /// <remarks>If the propertyName parameter does not correspond to an existing property on the current class, an exception is thrown.</remarks>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException($"{nameof(RaisePropertyChanged)} can not be called with null or empty {nameof(propertyName)}", nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Implementation of IExceptionHandler

        /// <summary>
        /// Handles the exception
        /// If false is returned, exception  was not handled.  Sender should throw the exception.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <returns><c>true</c> if exception was handled, <c>false</c> otherwise.</returns>
        public virtual bool HandleException(object sender, Exception exception) => Handler?.HandleException(sender, exception) == true;

        #endregion
    }
}