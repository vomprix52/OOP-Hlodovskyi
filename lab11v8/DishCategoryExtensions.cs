namespace Lab11
{
    // Методи розширення для категорій страв.
    // Клас і метод статичні, а перший параметр позначено словом this.
    public static class DishCategoryExtensions
    {
        // Повертає середню ціну всіх страв у переданих категоріях.
        // Якщо страв немає (наприклад, категорія порожня), повертає 0.
        public static decimal AverageDishPrice(this IEnumerable<DishCategory> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories), "Колекція категорій не може бути null.");
            }

            // Збираємо страви з усіх категорій в один список
            List<Dish> dishes = categories.SelectMany(c => c.Dishes).ToList();

            if (dishes.Count == 0)
            {
                return 0;
            }

            return dishes.Average(d => d.Price);
        }
    }
}
