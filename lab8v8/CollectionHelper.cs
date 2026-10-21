namespace Lab8
{
    // Статичний узагальнений клас-утиліта для роботи з колекціями будь-якого типу T
    public static class CollectionHelper<T>
    {
        // Варіант 8: метод Any.
        // Повертає true, якщо хоча б один елемент колекції задовольняє умову predicate.
        public static bool Any(IEnumerable<T> collection, Predicate<T> predicate)
        {
            // 1. Перевірка вхідних параметрів
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Колекція не може бути null.");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Умова (predicate) не може бути null.");
            }

            // 2. Перебираємо елементи і викликаємо делегат для кожного
            foreach (T item in collection)
            {
                if (predicate(item))
                {
                    // Знайшли перший збіг: далі перевіряти немає сенсу
                    return true;
                }
            }

            // 3. Жоден елемент не підійшов (або колекція порожня)
            return false;
        }
    }
}
