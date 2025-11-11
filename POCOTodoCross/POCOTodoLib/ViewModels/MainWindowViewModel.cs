using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using POCOTodoCross.Models;
using TaskModel = POCOTodoCross.Models.Task;

namespace POCOTodoCross.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly TaskService _taskService;
        private ObservableCollection<ITask> _tasks = new();
        private ITask? _selectedTask;
        private string _newTaskTitle = string.Empty;
        private string _newTaskDescription = string.Empty;
        private DateTime? _newTaskDueDate;
        private bool _isRecurring;
        private string _newTaskRecurrencePattern = string.Empty;

        public MainWindowViewModel(TaskService taskService)
        {
            _taskService = taskService;
            
            AddTaskCommand = new RelayCommand(_ => AddTask(), _ => CanAddTask());
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => CanDeleteTask());
            CompleteTaskCommand = new RelayCommand(_ => CompleteTask(), _ => CanCompleteTask());
            
            LoadTasks();
        }

        public ObservableCollection<ITask> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public ITask? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                _newTaskTitle = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set
            {
                _newTaskDescription = value;
                OnPropertyChanged();
            }
        }

        public DateTime? NewTaskDueDate
        {
            get => _newTaskDueDate;
            set
            {
                _newTaskDueDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsRecurring
        {
            get => _isRecurring;
            set
            {
                _isRecurring = value;
                OnPropertyChanged();
            }
        }

        public string NewTaskRecurrencePattern
        {
            get => _newTaskRecurrencePattern;
            set
            {
                _newTaskRecurrencePattern = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand CompleteTaskCommand { get; }

        private void LoadTasks()
        {
            var tasks = _taskService.GetTasks();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(NewTaskTitle);

        private void AddTask()
        {
            ITask newTask;
            if (IsRecurring)
            {
                newTask = new RecurringTask
                {
                    id = Guid.NewGuid().ToString(),
                    title = NewTaskTitle,
                    description = NewTaskDescription,
                    dueDate = NewTaskDueDate,
                    RecurrencePattern = NewTaskRecurrencePattern
                };
            }
            else
            {
                    newTask = new TaskModel
                {
                    id = Guid.NewGuid().ToString(),
                    title = NewTaskTitle,
                    description = NewTaskDescription,
                    dueDate = NewTaskDueDate
                };
            }

            _taskService.AddTask(newTask);
            Tasks.Add(newTask);

            // Clear the form
            NewTaskTitle = string.Empty;
            NewTaskDescription = string.Empty;
            NewTaskDueDate = null;
            IsRecurring = false;
            NewTaskRecurrencePattern = string.Empty;
        }

        private bool CanDeleteTask() => SelectedTask != null;

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                _taskService.RemoveTask(SelectedTask.id);
                Tasks.Remove(SelectedTask);
                SelectedTask = null;
            }
        }

        private bool CanCompleteTask() => SelectedTask != null && !SelectedTask.isCompleted;

        private void CompleteTask()
        {
            if (SelectedTask != null)
            {
                _taskService.ToggleCompleteTask(SelectedTask.id);
                // Force UI update
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}