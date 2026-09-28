using System;

namespace Lab4
{
    // 1. Базовий клас Device
    public class Device
    {
        private string _brand;
        private string _model;

        public string Brand
        {
            get => _brand;
            set => _brand = string.IsNullOrWhiteSpace(value) ? "Unknown Brand" : value;
        }

        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value) ? "Unknown Model" : value;
        }

        // Конструктор базового класу
        public Device(string brand, string model)
        {
            Brand = brand;
            Model = model;
        }

        // Віртуальний метод для перевизначення
        public virtual void PowerOn()
        {
            Console.WriteLine($"[Device] Пристрій {Brand} {Model} увімкнено.");
        }

        // Метод для демонстрації new (приховування)
        public string GetDeviceType()
        {
            return "Загальний електронний пристрій (Device)";
        }
    }

    // 2. Похідний клас Smartphone
    public class Smartphone : Device
    {
        public double ScreenSize { get; set; }

        // Виклик конструктора базового класу через base(...)
        public Smartphone(string brand, string model, double screenSize)
            : base(brand, model)
        {
            ScreenSize = screenSize;
        }

        // Перевизначення віртуального методу (override)
        public override void PowerOn()
        {
            Console.WriteLine($"[Smartphone] Смартфон {Brand} {Model} ({ScreenSize}\") завантажує оперативну систему...");
        }

        // Унікальний метод класу Smartphone
        public void MakeCall(string number)
        {
            Console.WriteLine($"[Smartphone] Здійснення дзвінка на номер {number} з {Brand} {Model}...");
        }

        // Приховування члена базового класу (new)
        public new string GetDeviceType()
        {
            return "Мобільний смартфон (Smartphone)";
        }
    }

    // 3. Похідний клас Laptop
    public class Laptop : Device
    {
        public string Processor { get; set; }

        // Виклик конструктора базового класу через base(...)
        public Laptop(string brand, string model, string processor)
            : base(brand, model)
        {
            Processor = processor;
        }

        // Перевизначення віртуального методу (override)
        public override void PowerOn()
        {
            Console.WriteLine($"[Laptop] Ноутбук {Brand} {Model} (Процесор: {Processor}) запускає робочий стіл...");
        }

        // Унікальний метод класу Laptop
        public void RunProgram(string programName)
        {
            Console.WriteLine($"[Laptop] Запуск програми '{programName}' на ноутбуці {Brand} {Model}...");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНА РОБОТА №4 (Варіант 8: Device - Smartphone - Laptop)\n");

            // 1. Створення об'єктів
            Console.WriteLine("1. Створення об'єктів базового та похідних класів");
            Device genericDevice = new Device("Generic", "Gadget");
            Smartphone phone = new Smartphone("Apple", "iPhone 17", 6.3);
            Laptop laptop = new Laptop("Lenovo", "LOQ 15", "Intel Core i5");

            genericDevice.PowerOn();
            phone.PowerOn();
            laptop.PowerOn();

            // 2. Виклик унікальних методів
            Console.WriteLine("\n2. Виклик унікальних методів похідних класів");
            phone.MakeCall("+380681234567");
            laptop.RunProgram("Visual Studio Code");

            // 3. Поліморфізм (виклики через масив посилань на базовий клас Device)
            Console.WriteLine("\n3. Демонстрація поліморфізму (virtual / override)");
            Device[] devices = new Device[] { genericDevice, phone, laptop };

            foreach (var dev in devices)
            {
                // Для кожного об'єкта виконується його власна override-версія
                dev.PowerOn();
            }

            // 4. Демонстрація різниці між override та new
            Console.WriteLine("\n4. Демонстрація приховування методів (new)");
            Smartphone directPhone = new Smartphone("Samsung", "Galaxy S24", 6.2);
            Device polyPhone = directPhone; // Посилання базового типу Device на об'єкт Smartphone

            Console.WriteLine($"Виклик через посилання Smartphone: {directPhone.GetDeviceType()}");
            Console.WriteLine($"Виклик через посилання Device:     {polyPhone.GetDeviceType()}");

            Console.WriteLine("\nПрограму завершено");
        }
    }
}