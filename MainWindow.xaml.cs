using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Views
{
    public partial class MainWindow : Window
    {
        private readonly DataService _dataService = new DataService();
        private List<ToDoItem> _tasks;

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        // Установка источника данных для DataGrid (т.е. загрузки и сохранение задач через сервис данных dataservice)
        private void LoadTasks()
        {
            _tasks = _dataService.LoadData();
            TasksGrid.ItemsSource = _tasks;
            UpdateStatus();
        }
                private void SaveTasks()
        {
            _dataService.SaveData(_tasks);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            int total = _tasks.Count;
            int completed = _tasks.Count(t => t.IsCompleted); // То етсь для каждого элемента из task возвращаем iscompleted // int completed = 0;   foreach (var task in _tasks) { if (task.IsCompleted) { completed++; } }
            StatusText.Text = $"Tasks: {total} | Done: {completed} | Left: {total - completed}";
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new ToDoItem();
            var editWindow = new TaskDetailWindow(newTask) { Owner = this };

            if (editWindow.ShowDialog() == true) // Показ блокирующего кона и проверка на нажатие "ок", дальше по коду будет также (вроде)
            {
                _tasks.Add(newTask);
                RefreshView();
                SaveTasks();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is ToDoItem task)
            {
                if (MessageBox.Show($"Delete '{task.Title}'?", "Confirm",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _tasks.Remove(task);
                    RefreshView();
                    SaveTasks();
                }
            }
            else
            {
                MessageBox.Show("Select a task first");
            }
        }

        // Логика редактирования
        private void OnCellEdit(object sender, DataGridCellEditEndingEventArgs e) // Обработчик окончания редактирования ячейки (вызывается когда редактировали ячейку в DataGrid (например изменили колонку, строку и т.д.)) Он вызывает событие CellEditEnding => срабатывает OnCellEdit и все изменения сохраняются
        {
            SaveTasks();
        }
        private void OnTaskDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TasksGrid.SelectedItem is ToDoItem task) // Проверка, что выбран элемент и он является задачей
            {
                var editWindow = new TaskDetailWindow(task) { Owner = this };
                if (editWindow.ShowDialog() == true)
                {
                    RefreshView();
                    SaveTasks();
                }
            }
        }


        private void RefreshView()
        {
            TasksGrid.ItemsSource = null; // Принудительно сбрасоваем источник данных
            TasksGrid.ItemsSource = _tasks; // А теперь устанавливаем обновлённый источник данных
        }

        // Логика закрытия окна с сохранением данных
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) //  Переопределение метода закрытия окна
        {
            SaveTasks();
            base.OnClosing(e);
        }
    }
}