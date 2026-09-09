using System;

namespace Lab2
{
    public class Animal
    {
        private string _species;
        private string _nickname;
        private int _age;

        public string Species
        {
            get => _species;
            set => _species = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        public string Nickname
        {
            get => _nickname;
            set => _nickname = string.IsNullOrWhiteSpace(value) ? "Unnamed" : value;
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                {
                    Console.WriteLine($"[Валідація] Помилка: вік не може бути від'ємним ({value}). Встановлено 0.");
                    _age = 0;
                }
                else
                {
                    _age = value;
                }
            }
        }

        public Animal(string species, string nickname, int age)
        {
            Console.WriteLine($"Виклик параметризованого конструктора для '{nickname}'");
            Species = species;
            Nickname = nickname;
            Age = age;
        }

        public Animal() : this("Unknown", "Unnamed", 0)
        {
            Console.WriteLine("--> Виклик конструктора за замовчуванням завершено");
        }

        public Animal(string species, string nickname) : this(species, nickname, 0)
        {
            Console.WriteLine($"--> Виклик перевантаженого конструктора (без віку) для '{nickname}'");
        }

        public void Speak()
        {
            string sound = _species.ToLower() switch
            {
                "кіт" or "cat" => "Мяу!",
                "собака" or "dog" => "Гав-гав!",
                "корова" or "cow" => "Му-у-у!",
                "папуга" or "parrot" => "Привіт!",
                _ => "Звуки тварини..."
            };

            Console.WriteLine($"[{_species}] {_nickname} (вік: {_age}) видає звук: \"{sound}\"");
        }

        ~Animal()
        {
            Console.WriteLine($"[Деструктор] Об'єкт тварини '{_nickname}' ({_species}) вилучено з пам'яті.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Початок виконання програми ===");

            CreateAndUseObjects();

            Console.WriteLine("\n=== Завершення Main, підготовка до GC ===");
            Console.WriteLine("Примусовий виклик збирача сміття (GC)...");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("=== Програму завершено ===");
        }

        static void CreateAndUseObjects()
        {
            Console.WriteLine("\n--- 1. Створення об'єкта через конструктор за замовчуванням ---");
            Animal animal1 = new Animal();
            animal1.Speak();

            Console.WriteLine("\n--- 2. Створення об'єкта через параметризований конструктор ---");
            Animal animal2 = new Animal("Собака", "Рекс", 4);
            animal2.Speak();

            Console.WriteLine("\n--- 3. Створення об'єкта з некоректним віком (перевірка валідації) ---");
            Animal animal3 = new Animal("Кіт", "Мурчик", -3);
            animal3.Speak();
        }
    }
}