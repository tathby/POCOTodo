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
                Console.WriteLine("Creating main window...");
                var main = new MainWindow();
                Console.WriteLine("Running application...");
                main.Show();
                app.Run(main);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting application: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
