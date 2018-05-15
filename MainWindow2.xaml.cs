using System;
using System.Windows;
using Visyn.Build.ViewModel;

namespace Visyn.Build
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
            if (DataContext == null)
                DataContext = new ApplicationServiceLocator().GetInstance<MainViewModel>();
        }
    }
}
