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
        private DateTime _currentDate = DateTime.Today;
        private string _newTaskTitle = string.Empty;
        private string _newTaskDescription = string.Empty;
        private DateTime? _newTaskDueDate;
        private bool _isRecurring;
        private string _newTaskRecurrencePattern = string.Empty;
        private bool _isEditingTask;
        private string _editTaskTitle = string.Empty;
        private string _editTaskDescription = string.Empty;
        private DateTime? _editTaskDueDate;

        public ICommand AdvanceDayCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }

        public MainWindowViewModel(TaskService taskService)
        {
            _taskService = taskService;
            
            AddTaskCommand = new RelayCommand(_ => AddTask(), _ => CanAddTask());
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => CanDeleteTask());
            CompleteTaskCommand = new RelayCommand(_ => CompleteTask(), _ => CanCompleteTask());
            AdvanceDayCommand = new RelayCommand(_ => AdvanceDay(), _ => true);
            EditTaskCommand = new RelayCommand(_ => StartEditTask(), _ => CanEditTask());
            SaveEditCommand = new RelayCommand(_ => SaveEdit(), _ => true);
            CancelEditCommand = new RelayCommand(_ => CancelEdit(), _ => true);
            
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

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
                UpdateOverdueFlags();
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

        public bool IsEditingTask
        {
            get => _isEditingTask;
            set
            {
                _isEditingTask = value;
                OnPropertyChanged();
            }
        }

        public string EditTaskTitle
        {
            get => _editTaskTitle;
            set
            {
                _editTaskTitle = value;
                OnPropertyChanged();
            }
        }

        public string EditTaskDescription
        {
            get => _editTaskDescription;
            set
            {
                _editTaskDescription = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EditTaskDueDate
        {
            get => _editTaskDueDate;
            set
            {
                _editTaskDueDate = value;
                OnPropertyChanged();
            }
        }

        private void LoadTasks()
        {
            var tasks = _taskService.GetTasks();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
            UpdateOverdueFlags();
        }

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(NewTaskTitle);

        private void AddTask()
        {
            ITask newTask;
            if (IsRecurring)
            {
                var recurringTask = new RecurringTask
                {
                    id = Guid.NewGuid().ToString(),
                    title = NewTaskTitle,
                    description = NewTaskDescription,
                    dueDate = NewTaskDueDate,
                    RecurrencePattern = NewTaskRecurrencePattern
                };
                // Initialize NextOccurrence based on recurrence pattern
                recurringTask.NextOccurrence = recurringTask.ComputeNextOccurrence(CurrentDate);
                newTask = recurringTask;
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
            UpdateOverdueFlags();

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
                UpdateOverdueFlags();
                OnPropertyChanged(nameof(Tasks));
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        private void AdvanceDay()
        {
            CurrentDate = CurrentDate.AddDays(1);
            CheckRecurringTasks();
        }

        private void CheckRecurringTasks()
        {
            for (int i = Tasks.Count - 1; i >= 0; i--)
            {
                var task = Tasks[i];
                if (task is RecurringTask recurringTask && task.isCompleted)
                {
                    // Check if the recurring task should reappear
                    if (recurringTask.NextOccurrence.Date <= CurrentDate.Date)
                    {
                        // Reset the task to not completed and update due date to next occurrence
                        task.isCompleted = false;
                        task.dueDate = recurringTask.NextOccurrence;
                        recurringTask.UpdateNextOccurrence();
                    }
                }
            }
            UpdateOverdueFlags();
        }

        private void UpdateOverdueFlags()
        {
            foreach (var task in Tasks)
            {
                task.isOverdue = !task.isCompleted && task.dueDate.HasValue && task.dueDate.Value.Date < CurrentDate.Date;
            }
            OnPropertyChanged(nameof(Tasks));
        }

        private bool CanEditTask() => SelectedTask != null;

        private void StartEditTask()
        {
            if (SelectedTask != null)
            {
                EditTaskTitle = SelectedTask.title;
                EditTaskDescription = SelectedTask.description;
                EditTaskDueDate = SelectedTask.dueDate;
                IsEditingTask = true;
            }
        }

        private void SaveEdit()
        {
            if (SelectedTask != null && IsEditingTask)
            {
                SelectedTask.title = EditTaskTitle;
                SelectedTask.description = EditTaskDescription;
                SelectedTask.dueDate = EditTaskDueDate;
                _taskService.UpdateTask(SelectedTask);
                IsEditingTask = false;
                UpdateOverdueFlags();
                OnPropertyChanged(nameof(Tasks));
            }
        }

        private void CancelEdit()
        {
            IsEditingTask = false;
            EditTaskTitle = string.Empty;
            EditTaskDescription = string.Empty;
            EditTaskDueDate = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}