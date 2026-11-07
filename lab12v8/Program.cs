namespace Lab12
{
    public class Program
    {
        private const string FilePath = "users.json";
        private const string Url = "https://api.example.com/users";
        private const string UserData = "Іван Коваль, ivan@example.com";

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            TimeSpan delay = TimeSpan.FromMilliseconds(200);

            // 1. NetworkClient: 2 невдачі, потім успіх
            Console.WriteLine("1. Відправка даних на сервер (NetworkClient.PostUserData)");
            NetworkClient client = new NetworkClient();
            bool posted = RetryHelper.ExecuteWithRetry(
                () => client.PostUserData(Url, UserData),
                retryCount: 3,
                initialDelay: delay,
                shouldRetry: IsTransientError);
            Console.WriteLine($"Результат: {posted}");
            Console.WriteLine();

            // 2. FileProcessor: 4 невдачі, потім успіх
            // SaveUserData нічого не повертає, тому обгортаємо його в лямбду, що повертає true
            Console.WriteLine("2. Збереження даних у файл (FileProcessor.SaveUserData)");
            FileProcessor processor = new FileProcessor();
            bool saved = RetryHelper.ExecuteWithRetry(
                () =>
                {
                    processor.SaveUserData(FilePath, UserData);
                    return true;
                },
                retryCount: 4,
                initialDelay: delay,
                shouldRetry: IsTransientError);
            Console.WriteLine($"Результат: {saved}");
            Console.WriteLine();

            // 3. Вибіркова обробка винятків через shouldRetry
            Console.WriteLine("3. Вибіркова обробка винятків (shouldRetry)");

            // 3.1. Порожні дані: ArgumentException не є тимчасовою помилкою, тому повтору немає
            Console.WriteLine("3.1. Порожні дані користувача");
            FileProcessor emptyDataProcessor = new FileProcessor();
            try
            {
                RetryHelper.ExecuteWithRetry(
                    () =>
                    {
                        emptyDataProcessor.SaveUserData(FilePath, "");
                        return true;
                    },
                    retryCount: 4,
                    initialDelay: delay,
                    shouldRetry: IsTransientError);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Main перехопив ArgumentException: {ex.Message}");
            }
            Console.WriteLine();

            // 3.2. shouldRetry повторює лише HttpRequestException, тому IOException одразу передається далі
            Console.WriteLine("3.2. shouldRetry повторює тільки HttpRequestException");
            FileProcessor httpOnlyProcessor = new FileProcessor();
            try
            {
                RetryHelper.ExecuteWithRetry(
                    () =>
                    {
                        httpOnlyProcessor.SaveUserData(FilePath, UserData);
                        return true;
                    },
                    retryCount: 4,
                    initialDelay: delay,
                    shouldRetry: ex => ex is HttpRequestException);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Main перехопив IOException: {ex.Message}");
            }
            Console.WriteLine();

            // 4. Спроби вичерпано: retryCount за замовчуванням (3) замало для 4 невдач
            Console.WriteLine("4. Вичерпання спроб (retryCount за замовчуванням = 3)");
            FileProcessor notEnoughRetriesProcessor = new FileProcessor();
            try
            {
                RetryHelper.ExecuteWithRetry(
                    () =>
                    {
                        notEnoughRetriesProcessor.SaveUserData(FilePath, UserData);
                        return true;
                    },
                    initialDelay: delay,
                    shouldRetry: IsTransientError);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Main перехопив IOException: {ex.Message}");
            }
        }

        // Тимчасові помилки, які варто повторювати: проблеми з файлом або мережею
        private static bool IsTransientError(Exception ex)
        {
            return ex is IOException || ex is HttpRequestException;
        }
    }
}
