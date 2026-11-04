namespace Lab11
{
    // Категорія страв, яка містить вкладену колекцію страв
    public class DishCategory
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }

        // 2. Конструктор
        public DishCategory(int id, string name, List<Dish> dishes)
        {
            Id = id;
            Name = name;
            Dishes = dishes;
        }

        // 3. Вивід категорії
        public override string ToString()
        {
            return $"{Name} (страв: {Dishes.Count})";
        }
    }
}
