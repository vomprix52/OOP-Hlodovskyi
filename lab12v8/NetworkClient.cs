namespace Lab12
{
    // Імітує мережеві запити: перші 2 виклики завершуються HttpRequestException, потім успіх
    public class NetworkClient
    {
        // 1. Скільки разів метод має "зламатися" перед успіхом
        private const int FailuresBeforeSuccess = 2;

        // 2. Лічильник викликів методу
        private int _callCount;

        // 3. Відправка даних користувача на сервер
        public bool PostUserData(string url, string userData)
        {
            _callCount++;
            Console.WriteLine($"  NetworkClient: відправка даних на {url}");

            // Імітація тимчасової мережевої помилки
            if (_callCount <= FailuresBeforeSuccess)
            {
                throw new HttpRequestException($"Сервер {url} тимчасово недоступний (виклик {_callCount}).");
            }

            Console.WriteLine($"  NetworkClient: сервер прийняв дані \"{userData}\"");
            return true;
        }
    }
}
