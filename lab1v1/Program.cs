using System;

namespace lab1v8
{
    public class Animal
    {
        private string _species;
        private string _nickname;
        private int _age;

        public string Species
        {
            get { return _species; }
            set { _species = value; }
        }

        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        public int Age
        {
            get { return _age; }
            set 
            { 
                if (value >= 0)
                    _age = value; 
            }
        }

        public Animal(string species, string nickname, int age)
        {
            _species = species;
            _nickname = nickname;
            _age = age >= 0 ? age : 0;
        }

        public void Speak()
        {
            Console.WriteLine($"[{_species}] на кличку {_nickname} видає звук!");
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Вид: {_species} | Кличка: {_nickname} | Вік: {_age} р.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота №1 (Варіант 8: Animal) ===\n");

            Animal animal1 = new Animal("Собака", "Рекс", 3);
            Animal animal2 = new Animal("Кіт", "Мурчик", 2);
            Animal animal3 = new Animal("Папуга", "Кеша", 5);

            animal1.PrintInfo();
            animal1.Speak();
            Console.WriteLine();

            animal2.PrintInfo();
            animal2.Speak();
            Console.WriteLine();

            animal3.PrintInfo();
            animal3.Speak();
            Console.WriteLine();
        }
    }
}