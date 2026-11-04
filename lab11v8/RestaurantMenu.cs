namespace Lab11
{
    // Меню ресторану, яке містить вкладену колекцію категорій
    public class RestaurantMenu
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DishCategory> Categories { get; set; }

        // 2. Конструктор
        public RestaurantMenu(int id, string name, List<DishCategory> categories)
        {
            Id = id;
            Name = name;
            Categories = categories;
        }
    }
}
