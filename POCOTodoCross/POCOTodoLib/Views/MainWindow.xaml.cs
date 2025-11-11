using System.Windows;
using POCOTodoCross.ViewModels;
using POCOTodoCross.Models;
using System;

namespace POCOTodoCross.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Console.WriteLine("MainWindow constructor: before InitializeComponent");
            InitializeComponent();
            Console.WriteLine("MainWindow constructor: after InitializeComponent");
            var taskStorage = new TaskStorage();
            var taskService = new TaskService(taskStorage);
            DataContext = new MainWindowViewModel(taskService);
        }
    }
}
