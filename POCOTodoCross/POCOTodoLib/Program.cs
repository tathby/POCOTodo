using System;
using System.Windows;
using POCOTodoCross.Views;

namespace POCOTodoCross
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting WPF application...");
                var app = new Application();
                app.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Console.WriteLine("Creating main window...");
                var main = new MainWindow();
                Console.WriteLine("Main window created, showing...");
                main.Show();
                Console.WriteLine("Main window shown. Running application...");
                app.Run(main);
                Console.WriteLine("Application exited normally.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting application: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
            }
        }
    }
}
