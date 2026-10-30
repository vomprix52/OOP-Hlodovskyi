namespace Lab10
{
    // Гравець, який належить до команди через TeamId
    public class Player
    {
        // 1. Властивості
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }      // зв'язок з Team.Id
        public int Age { get; set; }

        // 2. Конструктор
        public Player(int id, string name, int teamId, int age)
        {
            Id = id;
            Name = name;
            TeamId = teamId;
            Age = age;
        }

        // 3. Вивід гравця
        public override string ToString()
        {
            return $"{Name}, {Age} р.";
        }
    }
}
