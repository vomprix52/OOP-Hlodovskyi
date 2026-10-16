namespace Lab7
{
    // Реалізація репозиторію, що зберігає дані в пам'яті (у списку)
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        // 1. Внутрішнє сховище
        private readonly List<T> _items = new();

        // 2. Отримати елемент за Id
        public T GetById(int id)
        {
            // Шукаємо індекс елемента з потрібним Id (item.Id доступний завдяки where T : IEntity)
            int index = _items.FindIndex(item => item.Id == id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
            }
            return _items[index];
        }

        // 3. Отримати всі елементи
        public IEnumerable<T> GetAll()
        {
            // Повертаємо копію, щоб ззовні не можна було змінити внутрішній список
            return _items.ToList();
        }

        // 4. Додати елемент
        public void Add(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            // Id має бути унікальним
            if (_items.Any(existing => existing.Id == item.Id))
            {
                throw new InvalidOperationException($"Елемент з Id = {item.Id} вже існує.");
            }
            _items.Add(item);
        }

        // 5. Оновити елемент (замінюємо старий об'єкт новим з тим самим Id)
        public void Update(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            int index = _items.FindIndex(existing => existing.Id == item.Id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Неможливо оновити: елемент з Id = {item.Id} не знайдено.");
            }
            _items[index] = item;
        }

        // 6. Видалити елемент за Id
        public void Delete(int id)
        {
            int index = _items.FindIndex(item => item.Id == id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Неможливо видалити: елемент з Id = {id} не знайдено.");
            }
            _items.RemoveAt(index);
        }
    }
}
