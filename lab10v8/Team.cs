namespace Lab10
{
    // Команда спортивної ліги
    public class Team
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        // 2. Конструктор
        public Team(int id, string name, string city)
        {
            Id = id;
            Name = name;
            City = city;
        }

        // 3. Вивід команди
        public override string ToString()
        {
            return $"{Name} ({City})";
        }
    }
}
