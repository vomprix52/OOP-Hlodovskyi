namespace Lab7
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Створюємо репозиторій для задач
            IRepository<TaskItem> repository = new InMemoryRepository<TaskItem>();

            // 2. Додаємо задачі (Add)
            Console.WriteLine("1. Додавання задач (Add)");
            repository.Add(new TaskItem(1, "Здати лабу з ООП", "Лабораторна №7: Generics та IRepository"));
            repository.Add(new TaskItem(2, "Підготуватися до контрольної", "Повторити where-обмеження"));
            repository.Add(new TaskItem(3, "Купити продукти", "Хліб, молоко, яблука"));
            Console.WriteLine("Додано 3 задачі.");
            Console.WriteLine();

            // 3. Виводимо всі задачі (GetAll)
            Console.WriteLine("2. Усі задачі (GetAll)");
            PrintAll(repository);
            Console.WriteLine();

            // 4. Отримуємо одну задачу за Id (GetById)
            Console.WriteLine("3. Задача з Id = 1 (GetById)");
            TaskItem task = repository.GetById(1);
            Console.WriteLine(task);
            Console.WriteLine();

            // 5. Позначаємо задачу виконаною (Update)
            Console.WriteLine("4. Позначаємо задачу #1 виконаною (Update)");
            repository.Update(new TaskItem(1, task.Title, task.Description, isCompleted: true));
            PrintAll(repository);
            Console.WriteLine();

            // 6. Видаляємо задачу (Delete)
            Console.WriteLine("5. Видаляємо задачу #3 (Delete)");
            repository.Delete(3);
            PrintAll(repository);
            Console.WriteLine();

            // 7. Спроби роботи з неіснуючим Id
            Console.WriteLine("6. Робота з неіснуючим Id = 99");
            try
            {
                repository.GetById(99);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"GetById: {ex.Message}");
            }

            try
            {
                repository.Delete(99);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Delete: {ex.Message}");
            }
        }

        // Допоміжний метод: виводить усі задачі з репозиторію
        private static void PrintAll(IRepository<TaskItem> repository)
        {
            foreach (TaskItem item in repository.GetAll())
            {
                Console.WriteLine(item);
            }
        }
    }
}
