Звіт з лабораторної роботи №2
Тема: Клас із кількома конструкторами. Життєвий цикл об’єкта.

Варіант: №8 (Клас Animal)

Студент: Глодовський (група ІПЗ 3-2)

Репозиторій GitHub: OOP-Hlodovskyi/lab2v8

1. Код класу Animal
C#
using System;

namespace Lab2
{
    public class Animal
    {
        // Приватні поля
        private string _species;
        private string _nickname;
        private int _age;

        // Властивості з валідацією
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
                // Валідація: вік не може бути від'ємним
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

        // Параметризований конструктор (основний)
        public Animal(string species, string nickname, int age)
        {
            Species = species;
            Nickname = nickname;
            Age = age;
        }

        // Конструктор за замовчуванням (ланцюговий виклик : this(...))
        public Animal() : this("Unknown", "Unnamed", 0) { }

        // Перевантажений конструктор
        public Animal(string species, string nickname) : this(species, nickname, 0) { }

        // Метод класу
        public void Speak()
        {
            string sound = _species.ToLower() switch
            {
                "кіт" or "cat" => "Мяу!",
                "собака" or "dog" => "Гав-гав!",
                _ => "Звуки тварини..."
            };

            Console.WriteLine($"[{Species}] {Nickname} (вік: {Age}) видає звук: \"{sound}\"");
        }

        // Деструктор (Фіналізатор)
        ~Animal()
        {
            Console.WriteLine($"[Деструктор] Об'єкт тварини '{Nickname}' ({Species}) вилучено з пам'яті.");
        }
    }
}

2. Результат виконання програми (Консольний вивід)
Plaintext
=== Початок виконання програми ===

--- 1. Створення об'єкта через конструктор за замовчуванням ---
[Unknown] Unnamed (вік: 0) видає звук: "Звуки тварини..."

--- 2. Створення об'єкта через параметризований конструктор ---
[Собака] Рекс (вік: 4) видає звук: "Гав-гав!"

--- 3. Створення об'єкта з некоректним віком (перевірка валідації) ---
[Валідація] Помилка: вік не може бути від'ємним (-3). Встановлено 0.
[Кіт] Мурчик (вік: 0) видає звук: "Мяу!"

=== Завершення Main, підготовка до GC ===
Примусовий виклик збирача сміття (GC)...
[Деструктор] Об'єкт тварини 'Мурчик' (Кіт) вилучено з пам'яті.
[Деструктор] Об'єкт тварини 'Рекс' (Собака) вилучено з пам'яті.
[Деструктор] Об'єкт тварини 'Unnamed' (Unknown) вилучено з пам'яті.
=== Програму завершено ===

3. Висновок
Висновок: Життєвий цикл об’єкта в C# охоплює його виділення в купі (heap) та ініціалізацію конструктором (використання ланцюгового виклику : this() дозволяє оптимізувати код і уникнути дублювання), період активного використання за наявності посилань, а також автоматичне вилучення збирачем сміття (Garbage Collector) із виконанням деструктора ~Animal() після того, як об'єкт стає недосяжним.