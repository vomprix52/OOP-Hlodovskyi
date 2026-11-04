namespace Lab11
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Тестові дані: меню містять категорії, а категорії містять страви
            List<RestaurantMenu> menus = CreateMenus();

            // 1. SelectMany: усі страви з усіх категорій усіх меню
            Console.WriteLine("1. Усі страви з усіх категорій усіх меню (SelectMany)");

            // Синтаксис методів
            List<Dish> allDishes = menus
                .SelectMany(m => m.Categories)
                .SelectMany(c => c.Dishes)
                .ToList();

            for (int i = 0; i < allDishes.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {allDishes[i]}");
            }
            Console.WriteLine($"Усього страв: {allDishes.Count}");

            // Синтаксис запитів: кілька from підряд працюють як SelectMany
            List<Dish> allDishesQuery =
                (from m in menus
                 from c in m.Categories
                 from d in c.Dishes
                 select d).ToList();

            Console.WriteLine($"Синтаксис запитів дав страв: {allDishesQuery.Count}");
            Console.WriteLine();

            // 2. Різниця між Select і SelectMany
            Console.WriteLine("2. Різниця між Select і SelectMany");

            // Select повертає колекцію списків: для кожного меню окремий список категорій
            List<List<DishCategory>> categoriesBySelect = menus
                .Select(m => m.Categories)
                .ToList();

            // SelectMany "розгортає" ці списки в один плоский список категорій
            List<DishCategory> categoriesBySelectMany = menus
                .SelectMany(m => m.Categories)
                .ToList();

            Console.WriteLine($"Select: {categoriesBySelect.Count} елементи, кожен з яких є списком категорій");
            for (int i = 0; i < categoriesBySelect.Count; i++)
            {
                Console.WriteLine($"  {menus[i].Name}: {categoriesBySelect[i].Count} категорії");
            }
            Console.WriteLine($"SelectMany: {categoriesBySelectMany.Count} елементів, кожен з яких є категорією");
            Console.WriteLine();

            // 3. Пагінація: усі категорії по 2 на сторінку (Skip + Take)
            Console.WriteLine("3. Категорії страв по 2 на сторінку (Skip + Take)");

            int pageSize = 2;
            int totalPages = (int)Math.Ceiling(categoriesBySelectMany.Count / (double)pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                // Пропускаємо категорії попередніх сторінок і беремо наступні pageSize
                List<DishCategory> pageItems = categoriesBySelectMany
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                Console.WriteLine($"Сторінка {page} з {totalPages}:");
                foreach (DishCategory category in pageItems)
                {
                    Console.WriteLine($"  {category}");
                }
            }
            Console.WriteLine();

            // 4. Метод розширення AverageDishPrice()
            Console.WriteLine("4. Середня ціна страв (метод розширення AverageDishPrice)");

            // Для всіх категорій усіх меню
            Console.WriteLine($"Усі категорії: {categoriesBySelectMany.AverageDishPrice():F2} грн");

            // Для категорій одного меню
            foreach (RestaurantMenu menu in menus)
            {
                Console.WriteLine($"{menu.Name}: {menu.Categories.AverageDishPrice():F2} грн");
            }

            // Для кожної категорії окремо (передаємо список з однієї категорії)
            foreach (DishCategory category in categoriesBySelectMany)
            {
                List<DishCategory> oneCategory = new List<DishCategory> { category };
                Console.WriteLine($"  {category.Name}: {oneCategory.AverageDishPrice():F2} грн");
            }
        }

        // 3 меню, 7 категорій, 23 страви. Категорія "Авторські коктейлі" навмисно порожня.
        private static List<RestaurantMenu> CreateMenus()
        {
            return new List<RestaurantMenu>
            {
                new RestaurantMenu(1, "Основне меню", new List<DishCategory>
                {
                    new DishCategory(1, "Супи", new List<Dish>
                    {
                        new Dish(1, "Борщ український", 95m),
                        new Dish(2, "Солянка", 110m),
                        new Dish(3, "Курячий бульйон", 75m),
                        new Dish(4, "Грибна юшка", 85m)
                    }),
                    new DishCategory(2, "Гарячі страви", new List<Dish>
                    {
                        new Dish(5, "Вареники з картоплею", 120m),
                        new Dish(6, "Котлета по-київськи", 185m),
                        new Dish(7, "Деруни зі сметаною", 115m),
                        new Dish(8, "Голубці", 140m)
                    }),
                    new DishCategory(3, "Салати", new List<Dish>
                    {
                        new Dish(9, "Олів'є", 90m),
                        new Dish(10, "Грецький салат", 125m),
                        new Dish(11, "Вінегрет", 70m)
                    })
                }),
                new RestaurantMenu(2, "Дитяче меню", new List<DishCategory>
                {
                    new DishCategory(4, "Дитячі страви", new List<Dish>
                    {
                        new Dish(12, "Сирники", 80m),
                        new Dish(13, "Курячі нагетси", 95m),
                        new Dish(14, "Макарони з сиром", 70m)
                    }),
                    new DishCategory(5, "Десерти", new List<Dish>
                    {
                        new Dish(15, "Медовик", 85m),
                        new Dish(16, "Чізкейк", 110m),
                        new Dish(17, "Морозиво", 60m),
                        new Dish(18, "Млинці з варенням", 75m)
                    })
                }),
                new RestaurantMenu(3, "Барна карта", new List<DishCategory>
                {
                    new DishCategory(6, "Напої", new List<Dish>
                    {
                        new Dish(19, "Узвар", 45m),
                        new Dish(20, "Лимонад", 65m),
                        new Dish(21, "Чай трав'яний", 40m),
                        new Dish(22, "Кава американо", 55m),
                        new Dish(23, "Свіжовичавлений сік", 90m)
                    }),
                    new DishCategory(7, "Авторські коктейлі", new List<Dish>())
                })
            };
        }
    }
}
