namespace Lab7.Tests
{
    // Юніт-тести для InMemoryRepository<TaskItem>.
    // Кожен тест створює власний репозиторій, тому тести не впливають один на одного.
    public class InMemoryRepositoryTests
    {
        // 1. Add

        [Fact]
        public void Add_NewItem_ItemIsStored()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            var task = new TaskItem(1, "Здати лабу", "ООП №7");

            // Act
            repository.Add(task);

            // Assert
            TaskItem stored = Assert.Single(repository.GetAll());
            Assert.Equal(1, stored.Id);
            Assert.Equal("Здати лабу", stored.Title);
        }

        [Fact]
        public void Add_DuplicateId_ThrowsInvalidOperationException()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис"));
            var duplicate = new TaskItem(1, "Друга", "Той самий Id");

            // Act
            void act() => repository.Add(duplicate);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Single(repository.GetAll());
        }

        [Fact]
        public void Add_Null_ThrowsArgumentNullException()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();

            // Act
            void act() => repository.Add(null!);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        // 2. GetById

        [Fact]
        public void GetById_ExistingId_ReturnsItem()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис 1"));
            repository.Add(new TaskItem(2, "Друга", "Опис 2"));

            // Act
            TaskItem result = repository.GetById(2);

            // Assert
            Assert.Equal(2, result.Id);
            Assert.Equal("Друга", result.Title);
        }

        [Fact]
        public void GetById_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис"));

            // Act
            void act() => repository.GetById(99);

            // Assert
            Assert.Throws<KeyNotFoundException>(act);
        }

        // 3. GetAll

        [Fact]
        public void GetAll_EmptyRepository_ReturnsEmpty()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();

            // Act
            IEnumerable<TaskItem> result = repository.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_AfterAddingItems_ReturnsAllItems()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис 1"));
            repository.Add(new TaskItem(2, "Друга", "Опис 2"));
            repository.Add(new TaskItem(3, "Третя", "Опис 3"));

            // Act
            List<TaskItem> result = repository.GetAll().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, t => t.Id == 1);
            Assert.Contains(result, t => t.Id == 2);
            Assert.Contains(result, t => t.Id == 3);
        }

        // 4. Update

        [Fact]
        public void Update_ExistingItem_ChangesValues()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Стара назва", "Старий опис", isCompleted: false));
            var updated = new TaskItem(1, "Нова назва", "Новий опис", isCompleted: true);

            // Act
            repository.Update(updated);

            // Assert
            TaskItem result = repository.GetById(1);
            Assert.Equal("Нова назва", result.Title);
            Assert.Equal("Новий опис", result.Description);
            Assert.True(result.IsCompleted);
            Assert.Single(repository.GetAll());
        }

        [Fact]
        public void Update_NonExistingItem_ThrowsKeyNotFoundException()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис"));
            var missing = new TaskItem(99, "Неіснуюча", "Такого Id немає");

            // Act
            void act() => repository.Update(missing);

            // Assert
            Assert.Throws<KeyNotFoundException>(act);
        }

        // 5. Delete

        [Fact]
        public void Delete_ExistingId_RemovesItem()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис 1"));
            repository.Add(new TaskItem(2, "Друга", "Опис 2"));

            // Act
            repository.Delete(1);

            // Assert
            TaskItem remaining = Assert.Single(repository.GetAll());
            Assert.Equal(2, remaining.Id);
            Assert.DoesNotContain(repository.GetAll(), t => t.Id == 1);
        }

        [Fact]
        public void Delete_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var repository = new InMemoryRepository<TaskItem>();
            repository.Add(new TaskItem(1, "Перша", "Опис"));

            // Act
            void act() => repository.Delete(99);

            // Assert
            Assert.Throws<KeyNotFoundException>(act);
            Assert.Single(repository.GetAll());
        }
    }
}
