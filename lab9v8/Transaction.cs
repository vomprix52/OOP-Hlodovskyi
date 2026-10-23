namespace Lab9v8;

/// <summary>Статус життєвого циклу транзакції.</summary>
public enum TransactionStatus
{
    Pending,
    Processed,
    Cancelled
}

/// <summary>Фінансова транзакція. Додатна сума - надходження, від'ємна - списання.</summary>
public class Transaction
{
    public Transaction(string id, string description, decimal amount)
    {
        Id = id;
        Description = description;
        Amount = amount;
    }

    public string Id { get; }
    public string Description { get; }
    public decimal Amount { get; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public override string ToString()
    {
        string status = Status switch
        {
            TransactionStatus.Pending => "очікує",
            TransactionStatus.Processed => "оброблено",
            TransactionStatus.Cancelled => "скасовано",
            _ => "невідомо"
        };

        return $"[{Id}] {Description} ({Amount:+0.00;-0.00} грн, {status})";
    }
}
