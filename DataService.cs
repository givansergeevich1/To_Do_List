using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class DataService
    {
        private readonly string _filePath;

        public DataService()
        {
            // Указание пути на "AppData", там хранится json файл с задачами
            string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            _filePath = Path.Combine(appData, "ToDoList", "tasks.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public List<ToDoItem> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<ToDoItem>(); // Проверка что файл не существует => возврат пустого списка (если его нет)

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>(); // Преобразование JSON в список объектов (т.е. если todoitem будет null, то вернёт новый пустой список)
            }
            catch
            {
                return new List<ToDoItem>();
            }
        }

        public void SaveData(List<ToDoItem> items)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(items, options);
            File.WriteAllText(_filePath, json);
        }
    }
}