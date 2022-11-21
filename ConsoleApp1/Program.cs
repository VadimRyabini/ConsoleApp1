using System;
using System.IO;
namespace FileSystem
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите адрес директории");
            string adres = Console.ReadLine();
            using var watcher = new FileSystemWatcher(@adres);                  
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Изменение элемента: {e.FullPath}");
        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Создан элемент: {e.FullPath}";
            Console.WriteLine(value);
        }
        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Удален элемент: {e.FullPath}");
        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Элемент переименован:");
            Console.WriteLine($"    Старое имя: {e.OldFullPath}");
            Console.WriteLine($"    Новое имя: {e.FullPath}");
        }
        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Сообщение об ошибке: {ex.Message}");
                Console.WriteLine("Трассировка стека:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}