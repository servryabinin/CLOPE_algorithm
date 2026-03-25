namespace ConsoleApp1
{
    /// <summary>
    /// Класс, представляющий кластер в алгоритме CLOPE.
    /// Хранит статистику по элементам и транзакциям.
    /// </summary>
    public class Cluster
    {
        /// <summary>
        /// Частота появления каждого элемента в кластере.
        /// </summary>
        public Dictionary<string, int> Occ { get; } = new();

        /// <summary>
        /// Суммарное количество элементов (площадь).
        /// </summary>
        public int S { get; private set; } = 0;

        /// <summary>
        /// Количество уникальных элементов (ширина).
        /// </summary>
        public int W { get; private set; } = 0;

        /// <summary>
        /// Количество транзакций в кластере.
        /// </summary>
        public int N { get; private set; } = 0;

        /// <summary>
        /// Добавляет транзакцию в кластер.
        /// </summary>
        public void Add(Transaction t)
        {
            foreach (var item in t.Items)
            {
                if (!Occ.ContainsKey(item))
                {
                    Occ[item] = 0;
                    W++;
                }

                Occ[item]++;
                S++;
            }

            N++;
        }

        /// <summary>
        /// Удаляет транзакцию из кластера.
        /// </summary>
        public void Remove(Transaction t)
        {
            foreach (var item in t.Items)
            {
                if (!Occ.ContainsKey(item)) continue;

                Occ[item]--;
                S--;

                if (Occ[item] == 0)
                {
                    Occ.Remove(item);
                    W--;
                }
            }

            N--;
        }

        /// <summary>
        /// Вычисляет прирост функции стоимости при добавлении транзакции.
        /// </summary>
        public double DeltaAdd(Transaction t, double r)
        {
            int sNew = S + t.Length;
            int wNew = W;

            foreach (var item in t.Items)
            {
                if (!Occ.ContainsKey(item))
                    wNew++;
            }

            double before = (W == 0) ? 0 : (double)S * N / Math.Pow(W, r);
            double after = (double)sNew * (N + 1) / Math.Pow(wNew, r);

            return after - before;
        }

        /// <summary>
        /// Проверяет, является ли кластер пустым.
        /// </summary>
        public bool IsEmpty => N == 0;
    }
}