using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POCOTodoCross.Models
{
    public class Task : ITask, INotifyPropertyChanged
    {
        private string _id = string.Empty;
        private string _title = string.Empty;
        private bool _isCompleted;
        private bool _isOverdue;
        private string _description = string.Empty;
        private DateTime? _dueDate;

        public required string id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public required string title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public bool isCompleted { get => _isCompleted; set { _isCompleted = value; OnPropertyChanged(); } }
        public bool isOverdue { get => _isOverdue; set { _isOverdue = value; OnPropertyChanged(); } }
        public required string description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public DateTime? dueDate { get => _dueDate; set { _dueDate = value; OnPropertyChanged(); } }

        public void ToggleCompleted()
        {
            isCompleted = !isCompleted;
            OnPropertyChanged(nameof(isCompleted));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
