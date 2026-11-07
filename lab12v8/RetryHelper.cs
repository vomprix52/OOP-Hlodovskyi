namespace Lab12
{
    // Допоміжний клас для повторного виконання операцій, які можуть тимчасово не вдатися
    public static class RetryHelper
    {
        // operation - операція, яку виконуємо
        // retryCount - кількість повторів після першої спроби (усього спроб = 1 + retryCount)
        // initialDelay - затримка перед першим повтором, далі вона щоразу подвоюється
        // shouldRetry - вирішує, чи варто повторювати після конкретного винятку (null = повторювати завжди)
        public static T ExecuteWithRetry<T>(Func<T> operation, int retryCount = 3, TimeSpan initialDelay = default, Func<Exception, bool>? shouldRetry = null)
        {
            // 1. Перевірка параметрів
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Операція не може бути null.");
            }
            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), "Кількість повторів не може бути від'ємною.");
            }

            // Якщо затримку не передали, беремо 100 мс
            if (initialDelay == default)
            {
                initialDelay = TimeSpan.FromMilliseconds(100);
            }

            int totalAttempts = retryCount + 1;
            int attempt = 0;

            // 2. Цикл спроб
            while (true)
            {
                attempt++;
                try
                {
                    T result = operation();
                    Console.WriteLine($"[Спроба {attempt} з {totalAttempts}] Успіх");
                    return result;
                }
                catch (Exception ex)
                {
                    // 3. Логування невдачі
                    Console.WriteLine($"[Спроба {attempt} з {totalAttempts}] Помилка {ex.GetType().Name}: {ex.Message}");

                    // 4. Чи варто повторювати саме цей виняток
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        Console.WriteLine("Повтор не виконується: shouldRetry повернув false для цього винятку.");
                        throw;
                    }

                    // 5. Чи залишилися спроби
                    if (attempt == totalAttempts)
                    {
                        Console.WriteLine("Усі спроби вичерпано.");
                        throw;
                    }

                    // 6. Експоненційна затримка: initialDelay * 2^(номер повтору - 1)
                    TimeSpan delay = initialDelay * Math.Pow(2, attempt - 1);
                    Console.WriteLine($"Очікування {delay.TotalMilliseconds} мс перед наступною спробою");
                    Thread.Sleep(delay);
                }
            }
        }
    }
}
