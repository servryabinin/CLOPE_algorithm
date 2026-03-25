namespace ConsoleApp1
{
    /// <summary>
    /// Связывает транзакцию с кластером.
    /// </summary>
    public class ClusterAssignment
    {
        /// <summary>
        /// Транзакция.
        /// </summary>
        public Transaction Transaction { get; }

        /// <summary>
        /// Кластер, к которому принадлежит транзакция.
        /// </summary>
        public Cluster Cluster { get; set; }

        /// <summary>
        /// Конструктор назначения.
        /// </summary>
        public ClusterAssignment(Transaction t, Cluster c)
        {
            Transaction = t;
            Cluster = c;
        }
    }
}