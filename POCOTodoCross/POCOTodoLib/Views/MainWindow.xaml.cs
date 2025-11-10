using System.Windows;
using POCOTodoCross.ViewModels;
using POCOTodoCross.Models;

namespace POCOTodoCross.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var taskStorage = new TaskStorage();
            var taskService = new TaskService(taskStorage);
            DataContext = new MainWindowViewModel(taskService);
        }
    }
}
