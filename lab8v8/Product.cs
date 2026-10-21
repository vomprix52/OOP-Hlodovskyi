namespace Lab8
{
    // Клас продукту для демонстрації роботи з власним типом
    public class Product
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        // 2. Конструктор
        public Product(int id, string name, string category, decimal price, bool inStock)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            InStock = inStock;
        }

        // 3. Гарний вивід продукту
        public override string ToString()
        {
            string stock = InStock ? "є в наявності" : "немає в наявності";
            return $"#{Id} {Name} [{Category}] {Price:F2} грн ({stock})";
        }
    }
}
