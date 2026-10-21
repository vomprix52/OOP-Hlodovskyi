namespace Lab8
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНА РОБОТА №8 (Варіант 8: CollectionHelper<T>.Any)");
            Console.WriteLine();

            // 1. List<int>: чи є від'ємні числа
            Console.WriteLine("1. Список чисел (List<int>)");

            List<int> numbersWithNegative = new List<int> { 5, 12, -3, 8, 0 };
            Console.WriteLine($"Колекція: {string.Join(", ", numbersWithNegative)}");
            bool hasNegative = CollectionHelper<int>.Any(numbersWithNegative, x => x < 0);
            PrintResult("x => x < 0", hasNegative);

            List<int> positiveNumbers = new List<int> { 1, 7, 15, 42 };
            Console.WriteLine($"Колекція: {string.Join(", ", positiveNumbers)}");
            hasNegative = CollectionHelper<int>.Any(positiveNumbers, x => x < 0);
            PrintResult("x => x < 0", hasNegative);
            Console.WriteLine();

            // 2. List<Product>: перевірки за категорією та ціною
            Console.WriteLine("2. Список продуктів (List<Product>)");

            List<Product> products = new List<Product>
            {
                new Product(1, "Ноутбук", "Electronics", 32000m, true),
                new Product(2, "Футболка", "Clothing", 450m, true),
                new Product(3, "Кавоварка", "Kitchen", 2800m, false),
                new Product(4, "Книга \"C# для початківців\"", "Books", 390m, false)
            };
            Console.WriteLine("Колекція:");
            foreach (Product product in products)
            {
                Console.WriteLine($"  {product}");
            }

            bool hasElectronics = CollectionHelper<Product>.Any(products, p => p.Category == "Electronics");
            PrintResult("p => p.Category == \"Electronics\"", hasElectronics);

            bool hasSports = CollectionHelper<Product>.Any(products, p => p.Category == "Sports");
            PrintResult("p => p.Category == \"Sports\"", hasSports);

            bool expensiveOutOfStock = CollectionHelper<Product>.Any(products, p => p.Price > 1000 && !p.InStock);
            PrintResult("p => p.Price > 1000 && !p.InStock", expensiveOutOfStock);
            Console.WriteLine();

            // 3. List<string>: чи є рядок, що починається на "A"
            Console.WriteLine("3. Список рядків (List<string>)");

            List<string> names = new List<string> { "Olena", "Andrii", "Ivan", "Maksym" };
            Console.WriteLine($"Колекція: {string.Join(", ", names)}");
            bool startsWithA = CollectionHelper<string>.Any(names, s => s.StartsWith("A"));
            PrintResult("s => s.StartsWith(\"A\")", startsWithA);
            Console.WriteLine();

            // 4. Граничні випадки
            Console.WriteLine("4. Граничні випадки");

            List<int> emptyList = new List<int>();
            Console.WriteLine("Колекція: (порожня)");
            bool emptyResult = CollectionHelper<int>.Any(emptyList, x => x < 0);
            PrintResult("x => x < 0", emptyResult);

            Console.WriteLine("Колекція: null");
            try
            {
                CollectionHelper<int>.Any(null!, x => x < 0);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Виняток ArgumentNullException: {ex.Message}");
            }

            Console.WriteLine($"Колекція: {string.Join(", ", positiveNumbers)}, умова: null");
            try
            {
                CollectionHelper<int>.Any(positiveNumbers, null!);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Виняток ArgumentNullException: {ex.Message}");
            }
            Console.WriteLine();

            // 5. Різні способи створення делегата Predicate<int>
            Console.WriteLine("5. Різні способи створення делегата");

            List<int> mixedNumbers = new List<int> { 3, 50, 101, 77 };
            Console.WriteLine($"Колекція: {string.Join(", ", mixedNumbers)}");

            // Лямбда-вираз
            Predicate<int> lambda = x => x > 100;
            PrintResult("лямбда x => x > 100", CollectionHelper<int>.Any(mixedNumbers, lambda));

            // Анонімний метод
            Predicate<int> anonymous = delegate (int x) { return x > 100; };
            PrintResult("анонімний метод delegate(int x) { return x > 100; }", CollectionHelper<int>.Any(mixedNumbers, anonymous));

            // Звичайний іменований метод
            Predicate<int> namedMethod = IsGreaterThan100;
            PrintResult("звичайний метод IsGreaterThan100", CollectionHelper<int>.Any(mixedNumbers, namedMethod));
        }

        // Звичайний метод, сигнатура якого підходить під Predicate<int>
        private static bool IsGreaterThan100(int x)
        {
            return x > 100;
        }

        // Допоміжний метод: виводить умову та результат перевірки
        private static void PrintResult(string condition, bool result)
        {
            Console.WriteLine($"Умова: {condition} -> Результат: {result}");
        }
    }
}
