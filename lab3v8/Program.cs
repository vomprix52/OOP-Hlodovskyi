using System;

namespace Lab3
{
    public class TemporaryFile : IDisposable
    {
        // 1. Поля
        private bool _disposed = false;
        private bool _fileExists;         
        private readonly string _tempFilePath;

        // 2. Властивості
        public string TempFilePath => _tempFilePath;
        public bool FileExists => _fileExists;

        // 3. Конструктор (виділення ресурсу)
        public TemporaryFile(string filePath)
        {
            _tempFilePath = filePath;
            _fileExists = true;
            Console.WriteLine($"[Конструктор] Створено тимчасовий файл: '{_tempFilePath}'");
        }

        // 4. Метод класу
        public void Write(string content)
        {
            if (_disposed || !_fileExists)
            {
                throw new ObjectDisposedException(nameof(TemporaryFile), "Помилка: тимчасовий файл уже видалено!");
            }

            Console.WriteLine($"[Запис] У файл '{_tempFilePath}' записано: \"{content}\"");
        }

        // 5. Захищений віртуальний метод Dispose(bool disposing) — Патерн Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[Dispose(true)] Звільнення керованих ресурсів для '{_tempFilePath}'");
                }

                if (_fileExists)
                {
                    Console.WriteLine($"[Dispose] Видалення тимчасового файла (некерований ресурс): '{_tempFilePath}'");
                    _fileExists = false;
                }

                _disposed = true;
            }
        }

        // 6. Публічний метод Dispose()
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // 7. Деструктор (Фіналізатор)
        ~TemporaryFile()
        {
            Console.WriteLine($"[~TemporaryFile] Викликано деструктор для '{_tempFilePath}'");
            Dispose(false);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №3 (Варіант 8: TemporaryFile) ===");

            // Сценарій 1: Використання оператора using
            Console.WriteLine("\n--- Сценарій 1: Автоматичне звільнення через 'using' ---");
            using (TemporaryFile file1 = new TemporaryFile("session_data_1.tmp"))
            {
                file1.Write("Кешовані дані сесії №1");
            } 

            // Сценарій 2: Явний виклик Dispose()
            Console.WriteLine("\n--- Сценарій 2: Явний виклик Dispose() ---");
            TemporaryFile file2 = new TemporaryFile("session_data_2.tmp");
            file2.Write("Кешовані дані сесії №2");
            file2.Dispose();

            // Сценарій 3: Без Dispose() — робота збирача сміття (GC) та деструктора
            Console.WriteLine("\n--- Сценарій 3: Об'єкт без Dispose() (робота GC та деструктора) ---");
            CreateUnreleasedObject();

            Console.WriteLine("\nПримусовий запуск Garbage Collector (GC.Collect)...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\n=== Програму завершено ===");
        }

        // Окремий метод, щоб посилання вийшло з області видимості
        static void CreateUnreleasedObject()
        {
            TemporaryFile file3 = new TemporaryFile("forgotten_data_3.tmp");
            file3.Write("Дані без явного закриття");
        }
    }
}