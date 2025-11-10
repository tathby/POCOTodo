using System;
using System.Windows;
using POCOTodoCross.Views;
using POCOTodoCross.Models;

namespace POCOTodoCross
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Create storage and services if the view model needs them (MainWindow also constructs its own ones,
            // but it's safe to let MainWindow handle DI internally). We'll just start the WPF application and
            // show MainWindow.
            var app = new Application();
            var main = new MainWindow();
            app.Run(main);
        }
    }
}
