namespace Lab7
{
    // Узагальнений репозиторій для будь-якого типу T.
    // Обмеження where T : IEntity гарантує, що в T є властивість Id.
    public interface IRepository<T> where T : IEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}
