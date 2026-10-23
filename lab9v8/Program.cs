using System;
using System.Collections.Generic;
using System.Text;

namespace Lab9v8;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var processor = new TransactionProcessor();

        // 1. HashSet - унікальні ID транзакцій
        Console.WriteLine("1. Унікальні ID транзакцій (HashSet<string>)");

        var incoming = new List<Transaction>
        {
            new Transaction("T-001", "Зарплата", 15000m),
            new Transaction("T-002", "Оплата оренди", -6000m),
            new Transaction("T-003", "Переказ від клієнта", 2500.50m),
            new Transaction("T-002", "Оплата оренди (дубль)", -6000m),
            new Transaction("T-004", "Купівля обладнання", -4200m),
            new Transaction("T-001", "Зарплата (повтор)", 15000m),
            new Transaction("T-005", "Повернення коштів", 800m)
        };

        foreach (var t in incoming)
        {
            if (processor.Submit(t))
            {
                Console.WriteLine($"Прийнято: {t}");
            }
            else
            {
                Console.WriteLine($"Відхилено (дубль ID {t.Id}): {t.Description}");
            }
        }

        Console.WriteLine($"Надіслано: {incoming.Count}, унікальних ID: {processor.UniqueIdCount}");
        Console.WriteLine($"Contains(\"T-003\"): {processor.IsIdRegistered("T-003")}");
        Console.WriteLine($"Contains(\"T-999\"): {processor.IsIdRegistered("T-999")}");

        var current = processor.GetIds();
        var other = new HashSet<string> { "T-004", "T-005", "T-006", "T-007" };
        Console.WriteLine($"Поточні ID: {string.Join(", ", current)}");
        Console.WriteLine($"ID з іншої системи: {string.Join(", ", other)}");

        var union = new HashSet<string>(current);
        union.UnionWith(other);
        Console.WriteLine($"UnionWith: {string.Join(", ", union)}");

        var intersection = new HashSet<string>(current);
        intersection.IntersectWith(other);
        Console.WriteLine($"IntersectWith: {string.Join(", ", intersection)}");

        var difference = new HashSet<string>(current);
        difference.ExceptWith(other);
        Console.WriteLine($"ExceptWith: {string.Join(", ", difference)}");

        // 2. Queue - черга транзакцій на обробку (FIFO)
        Console.WriteLine();
        Console.WriteLine("2. Черга транзакцій на обробку (Queue<Transaction>)");
        Console.WriteLine($"У черзі: {processor.PendingCount}");
        Console.WriteLine($"Peek: {processor.PeekNext()}");

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Dequeue: {processor.ProcessNext()}, баланс: {processor.Balance:0.00}");
        }

        Console.WriteLine($"Залишилось у черзі: {processor.PendingCount}");

        // 3. Stack - історія транзакцій для відміни (LIFO)
        Console.WriteLine();
        Console.WriteLine("3. Історія транзакцій для відміни (Stack<Transaction>)");
        Console.WriteLine($"В історії: {processor.HistoryCount}");
        Console.WriteLine($"Peek: {processor.PeekLastProcessed()}");

        for (int i = 0; i < 2; i++)
        {
            Console.WriteLine($"Pop (відміна): {processor.UndoLast()}, баланс: {processor.Balance:0.00}");
        }

        Console.WriteLine($"Залишилось в історії: {processor.HistoryCount}");

        // 4. Спільна робота колекцій
        Console.WriteLine();
        Console.WriteLine("4. Обробка решти черги та повторна подача T-003");

        while (processor.PeekNext() != null)
        {
            Console.WriteLine($"Dequeue: {processor.ProcessNext()}, баланс: {processor.Balance:0.00}");
        }

        if (processor.ProcessNext() == null)
        {
            Console.WriteLine("Черга порожня, обробляти нічого");
        }

        var retry = new Transaction("T-003", "Переказ від клієнта (повторно)", 2500.50m);
        Console.WriteLine($"Submit без Remove: {processor.Submit(retry)}");
        Console.WriteLine($"Remove(\"T-003\"): {processor.ReleaseId("T-003")}");
        Console.WriteLine($"Submit після Remove: {processor.Submit(retry)}");
        Console.WriteLine($"Dequeue: {processor.ProcessNext()}, баланс: {processor.Balance:0.00}");
    }
}
