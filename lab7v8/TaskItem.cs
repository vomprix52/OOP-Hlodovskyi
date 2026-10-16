namespace Lab7
{
    // Сутність за варіантом 8: задача
    public class TaskItem : IEntity
    {
        // 1. Властивості
        public int Id { get; set; }                       // з інтерфейсу IEntity
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        // 2. Конструктор без параметрів
        public TaskItem()
        {
        }

        // 3. Зручний конструктор з усіма полями
        public TaskItem(int id, string title, string description, bool isCompleted = false)
        {
            Id = id;
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
        }

        // 4. Гарний вивід задачі українською
        public override string ToString()
        {
            string status = IsCompleted ? "виконано" : "не виконано";
            return $"#{Id} \"{Title}\" ({Description}) [{status}]";
        }
    }
}
