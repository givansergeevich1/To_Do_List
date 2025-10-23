using System.Windows;
using ToDoList.Models;

namespace ToDoList.Views
{
    public partial class TaskDetailWindow : Window
    {
        public ToDoItem Task { get; }

        public TaskDetailWindow(ToDoItem task) // Окно редактирования
        {
            InitializeComponent();
            Task = task;
            DataContext = Task; // Обращение всех привязок "binding" (из XAML) к свойствам объекта Task
        }

        // Гарант сохранения валидного названия задачи
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Task.Title))
            {
                MessageBox.Show("Enter a title");
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}