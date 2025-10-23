using System;
using System.ComponentModel;

namespace ToDoList.Models
{
    public class ToDoItem : INotifyPropertyChanged // Уведомление UI об изменениях свойств
    {
        private string _title = "New Task";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; }
        public string Description { get; set; } = string.Empty; // Инициализируем пустой строкой

        public string Title // Гарант, что у задачи будет валидное название
        {
            get => _title; // "=>" - лямбда выражение для сокращённой записи "{ return _title; }"
            set
            {
                _title = string.IsNullOrWhiteSpace(value) ? "New Task" : value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; // Обработчик событий (нужен для событий изменения свойств)

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Уведолмение подписчиков "PropertyChanged" (WPF Data Binding System), что свойство Title объекта this изменилось. Потом WPF сам обновит отображение в UI
        }
    }
}