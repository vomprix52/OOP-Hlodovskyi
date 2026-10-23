using System.Collections.Generic;

namespace Lab9v8;

/// <summary>
/// Система обробки транзакцій. Поєднує три колекції:
/// HashSet - унікальні ID, Queue - черга на обробку, Stack - історія для відміни.
/// </summary>
public class TransactionProcessor
{
    private readonly HashSet<string> _uniqueIds = new();
    private readonly Queue<Transaction> _queue = new();
    private readonly Stack<Transaction> _history = new();

    public decimal Balance { get; private set; }
    public int UniqueIdCount => _uniqueIds.Count;
    public int PendingCount => _queue.Count;
    public int HistoryCount => _history.Count;

    /// <summary>Транзакції в черзі (від голови до хвоста, порядок FIFO).</summary>
    public IEnumerable<Transaction> Pending => _queue;

    /// <summary>Історія оброблених транзакцій (вершина стеку першою, порядок LIFO).</summary>
    public IEnumerable<Transaction> History => _history;

    // ---------- HashSet<string>: унікальні ID ----------

    /// <summary>
    /// Додає транзакцію в чергу, якщо її ID ще не зустрічався.
    /// HashSet.Add повертає false для дубліката - це і є перевірка за O(1).
    /// </summary>
    public bool Submit(Transaction transaction)
    {
        if (!_uniqueIds.Add(transaction.Id))
        {
            return false;
        }

        _queue.Enqueue(transaction);
        return true;
    }

    public bool IsIdRegistered(string id) => _uniqueIds.Contains(id);

    /// <summary>Звільняє ID, щоб транзакцію можна було подати повторно.</summary>
    public bool ReleaseId(string id) => _uniqueIds.Remove(id);

    /// <summary>Копія множини ID (для операцій над множинами поза класом).</summary>
    public HashSet<string> GetIds() => new(_uniqueIds);

    // ---------- Queue<Transaction>: черга на обробку ----------

    /// <summary>Перша транзакція в черзі без видалення (Peek).</summary>
    public Transaction? PeekNext() => _queue.TryPeek(out var transaction) ? transaction : null;

    /// <summary>
    /// Бере з черги найстарішу транзакцію (Dequeue), застосовує її до балансу
    /// і додає в історію. Повертає null, якщо черга порожня.
    /// </summary>
    public Transaction? ProcessNext()
    {
        if (!_queue.TryDequeue(out var transaction))
        {
            return null;
        }

        Balance += transaction.Amount;
        transaction.Status = TransactionStatus.Processed;
        _history.Push(transaction);
        return transaction;
    }

    // ---------- Stack<Transaction>: історія для відміни ----------

    /// <summary>Остання оброблена транзакція без видалення (Peek).</summary>
    public Transaction? PeekLastProcessed() => _history.TryPeek(out var transaction) ? transaction : null;

    /// <summary>
    /// Скасовує останню оброблену транзакцію (Pop) і повертає баланс назад.
    /// Повертає null, якщо історія порожня.
    /// </summary>
    public Transaction? UndoLast()
    {
        if (!_history.TryPop(out var transaction))
        {
            return null;
        }

        Balance -= transaction.Amount;
        transaction.Status = TransactionStatus.Cancelled;
        return transaction;
    }
}
