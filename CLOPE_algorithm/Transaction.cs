namespace ConsoleApp1
{
    /// <summary>
    /// Класс транзакции (набор категорийных признаков).
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Элементы транзакции.
        /// </summary>
        public IReadOnlyList<string> Items { get; }

        /// <summary>
        /// Количество элементов.
        /// </summary>
        public int Length => Items.Count;

        /// <summary>
        /// Конструктор транзакции.
        /// </summary>
        public Transaction(IEnumerable<string> items)
        {
            Items = items.ToList();
        }
    }
}