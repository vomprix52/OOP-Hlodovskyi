using System;
using System.Collections.Generic;

namespace Lab6
{
    // 1. Базовий клас Device
    public class Device
    {
        // Приватні поля
        private string _brand;
        private string _model;

        // Публічні властивості з перевіркою значень
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
            _brand = "Unknown Brand";
            _model = "Unknown Model";
            Brand = brand;
            Model = model;
        }

        // Віртуальний метод, який перевизначають похідні класи
        public virtual void PowerOn()
        {
            Console.WriteLine($"[Device] Пристрій {Brand} {Model} увімкнено.");
        }

        // Звичайний (не віртуальний) метод — його приховує Smartphone через new
        public string GetDeviceType()
        {
            return "Електронний пристрій (Device)";
        }
    }

    // 2. Похідний клас Smartphone
    public class Smartphone : Device
    {
        private double _screenSize;

        public double ScreenSize
        {
            get => _screenSize;
            set => _screenSize = value > 0 ? value : 6.0;
        }

        // Виклик конструктора базового класу через base(...)
        public Smartphone(string brand, string model, double screenSize)
            : base(brand, model)
        {
            ScreenSize = screenSize;
        }

        // Перевизначення віртуального методу
        public override void PowerOn()
        {
            base.PowerOn(); // спочатку виконується логіка базового класу
            Console.WriteLine($"[Smartphone] Екран {ScreenSize}\" увімкнено, завантажується мобільна ОС...");
        }

        // Унікальний метод класу Smartphone
        public void MakeCall(string phoneNumber)
        {
            Console.WriteLine($"[Smartphone] {Brand} {Model} телефонує на номер {phoneNumber}...");
        }

        // Приховування методу базового класу (new), а НЕ перевизначення
        public new string GetDeviceType()
        {
            return "Смартфон (Smartphone)";
        }
    }

    // 3. Похідний клас Laptop
    public class Laptop : Device
    {
        private string _processor;

        public string Processor
        {
            get => _processor;
            set => _processor = string.IsNullOrWhiteSpace(value) ? "Unknown CPU" : value;
        }

        // Виклик конструктора базового класу через base(...)
        public Laptop(string brand, string model, string processor)
            : base(brand, model)
        {
            _processor = "Unknown CPU";
            Processor = processor;
        }

        // Перевизначення віртуального методу
        public override void PowerOn()
        {
            Console.WriteLine($"[Laptop] Ноутбук {Brand} {Model} ({Processor}) проходить POST і завантажує Windows...");
        }

        // Унікальний метод класу Laptop
        public void RunProgram(string programName)
        {
            Console.WriteLine($"[Laptop] На {Brand} {Model} запущено програму \"{programName}\".");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ЛАБОРАТОРНА РОБОТА №6 (Варіант 8: Device -> Smartphone -> Laptop)\n");

            // 1. Створення об'єктів базового та похідних класів
            Console.WriteLine("1. Створення об'єктів та виклик PowerOn() через власні типи");
            Device device = new Device("Xiaomi", "Smart Hub");
            Smartphone smartphone = new Smartphone("Samsung", "Galaxy S24", 6.2);
            Laptop laptop = new Laptop("Lenovo", "LOQ 15", "Intel Core i5-12450HX");

            device.PowerOn();
            smartphone.PowerOn();
            laptop.PowerOn();

            // 2. Унікальні методи похідних класів
            Console.WriteLine("\n2. Унікальні методи похідних класів");
            smartphone.MakeCall("+380671234567");
            laptop.RunProgram("Visual Studio Code");

            // 3. Поліморфізм: виклик через посилання базового типу
            Console.WriteLine("\n3. Поліморфізм (virtual / override) через List<Device>");
            List<Device> devices = new List<Device> { device, smartphone, laptop };

            foreach (Device d in devices)
            {
                // Викликається версія методу фактичного типу об'єкта
                Console.Write($"  {d.GetType().Name,-11} -> ");
                d.PowerOn();
            }

            // 4. Різниця між override та new
            Console.WriteLine("\n4. Різниця між override та new");
            Smartphone phoneRef = smartphone;   // посилання типу Smartphone
            Device deviceRef = smartphone;      // посилання типу Device на той самий об'єкт

            Console.WriteLine("override (PowerOn) — результат однаковий для обох посилань:");
            Console.Write("  Smartphone-посилання: ");
            phoneRef.PowerOn();
            Console.Write("  Device-посилання:     ");
            deviceRef.PowerOn();

            Console.WriteLine("\nnew (GetDeviceType) — результат залежить від типу посилання:");
            Console.WriteLine($"  Smartphone-посилання: {phoneRef.GetDeviceType()}");
            Console.WriteLine($"  Device-посилання:     {deviceRef.GetDeviceType()}");

            Console.WriteLine("\nLaptop не приховує GetDeviceType(), тому використовує метод базового класу:");
            Console.WriteLine($"  {laptop.GetDeviceType()}");

            Console.WriteLine("\nПрограму завершено.");
        }
    }
}