namespace Lab12
{
    // Імітує роботу з файлами: перші 4 виклики завершуються IOException, потім успіх
    public class FileProcessor
    {
        // 1. Скільки разів метод має "зламатися" перед успіхом
        private const int FailuresBeforeSuccess = 4;

        // 2. Лічильник викликів методу
        private int _callCount;

        // 3. Збереження даних користувача у файл
        public void SaveUserData(string path, string userData)
        {
            // Некоректні дані: таку помилку повторювати немає сенсу
            if (string.IsNullOrWhiteSpace(userData))
            {
                throw new ArgumentException("Дані користувача не можуть бути порожніми.", nameof(userData));
            }

            _callCount++;
            Console.WriteLine($"  FileProcessor: відкриття файлу {path}");

            try
            {
                // Імітація тимчасової помилки: файл зайнятий іншим процесом
                if (_callCount <= FailuresBeforeSuccess)
                {
                    throw new IOException($"Файл {path} зайнятий іншим процесом (виклик {_callCount}).");
                }

                Console.WriteLine($"  FileProcessor: дані \"{userData}\" збережено у {path}");
            }
            finally
            {
                // Виконується завжди: і після успіху, і після помилки
                Console.WriteLine($"  FileProcessor: файл {path} закрито");
            }
        }
    }
}
