namespace Lab11
{
    // Страва з меню
    public class Dish
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // 2. Конструктор
        public Dish(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        // 3. Вивід страви
        public override string ToString()
        {
            return $"{Name} ({Price:F2} грн)";
        }
    }
}
