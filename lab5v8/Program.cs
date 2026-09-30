using System;
using System.Collections.Generic;

namespace OOP
{
    // Базовий клас
    public class Transaction
    {
        public decimal Amount { get; set; }

        public Transaction(decimal amount)
        {
            Amount = amount;
        }

        public virtual void Execute()
        {
            Console.WriteLine($"[Transaction] Обробка транзакції на суму: {Amount:F2} грн");
        }
    }

    // Похідний клас 1: Поповнення рахунку
    public class Deposit : Transaction
    {
        public string DestinationAccount { get; set; }

        public Deposit(decimal amount, string destinationAccount) : base(amount)
        {
            DestinationAccount = destinationAccount;
        }

        public override void Execute()
        {
            Console.WriteLine($"[Deposit] Поповнення рахунку {DestinationAccount} на суму: {Amount:F2} грн");
        }
    }

    // Похідний клас 2: Зняття коштів
    public class Withdrawal : Transaction
    {
        public string SourceAccount { get; set; }

        public Withdrawal(decimal amount, string sourceAccount) : base(amount)
        {
            SourceAccount = sourceAccount;
        }

        public override void Execute()
        {
            Console.WriteLine($"[Withdrawal] Зняття коштів з рахунку {SourceAccount} на суму: {Amount:F2} грн");
        }
    }

    // Похідний клас 3: Переказ коштів
    public class Transfer : Transaction
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }

        public Transfer(decimal amount, string fromAccount, string toAccount) : base(amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;
        }

        public override void Execute()
        {
            Console.WriteLine($"[Transfer] Переказ з рахунку {FromAccount} на рахунок {ToAccount} на суму: {Amount:F2} грн");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Колекція об'єктів базового типу
            List<Transaction> transactions = new List<Transaction>
            {
                new Deposit(1500.00m, "UA1234567890"),
                new Withdrawal(300.50m, "UA1234567890"),
                new Transfer(750.00m, "UA1234567890", "UA0987654321"),
                new Deposit(2000.00m, "UA0987654321")
            };

            Console.WriteLine("Виконання банківських транзакцій");
            decimal totalAmount = 0;

            // Поліморфна обробка та агрегація
            foreach (var transaction in transactions)
            {
                transaction.Execute(); // Поліморфний виклик методу
                totalAmount += transaction.Amount; // Агрегація суми
            }

            Console.WriteLine("\nРезультат агрегації");
            Console.WriteLine($"Загальна сума всіх транзакцій: {totalAmount:F2} грн");
        }
    }
}