using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Реализация алгоритма кластеризации CLOPE (Clustering with sLOPE).
    /// Выполняет разбиение транзакционных данных на кластеры
    /// с использованием глобальной функции стоимости.
    /// </summary>
    public class Clope
    {
        /// <summary>
        /// Коэффициент отталкивания (repulsion), регулирующий плотность кластеров.
        /// </summary>
        private readonly double _r;

        /// <summary>
        /// Список текущих кластеров.
        /// </summary>
        private readonly List<Cluster> _clusters = new();

        /// <summary>
        /// Сообщение об ошибке для некорректного значения r.
        /// </summary>
        private readonly string exceptionMessage = "r must be > 1";

        /// <summary>
        /// Конструктор алгоритма CLOPE.
        /// </summary>
        /// <param name="r">Коэффициент отталкивания (должен быть больше 1)</param>
        public Clope(double r)
        {
            if (r <= 1)
                throw new ArgumentException(exceptionMessage);

            _r = r;
        }

        /// <summary>
        /// Выполняет кластеризацию заданного набора транзакций.
        /// </summary>
        /// <param name="transactions">Набор транзакций</param>
        /// <returns>Список назначений транзакций к кластерам</returns>
        public List<ClusterAssignment> Fit(IEnumerable<Transaction> transactions)
        {
            var assignments = new List<ClusterAssignment>();

            foreach (var t in transactions)
            {
                Cluster bestCluster = null;
                double bestDelta = double.MinValue;

                foreach (var c in _clusters)
                {
                    double delta = c.DeltaAdd(t, _r);

                    if (delta > bestDelta)
                    {
                        bestDelta = delta;
                        bestCluster = c;
                    }
                }

                var newCluster = new Cluster();
                double newDelta = newCluster.DeltaAdd(t, _r);

                if (newDelta > bestDelta)
                {
                    newCluster.Add(t);
                    _clusters.Add(newCluster);
                    assignments.Add(new ClusterAssignment(t, newCluster));
                }
                else
                {
                    bestCluster.Add(t);
                    assignments.Add(new ClusterAssignment(t, bestCluster));
                }
            }

            bool moved;

            do
            {
                moved = false;

                foreach (var a in assignments)
                {
                    var t = a.Transaction;
                    var current = a.Cluster;

                    current.Remove(t);

                    Cluster bestCluster = null;
                    double bestDelta = double.MinValue;

                    foreach (var c in _clusters)
                    {
                        double delta = c.DeltaAdd(t, _r);

                        if (delta > bestDelta)
                        {
                            bestDelta = delta;
                            bestCluster = c;
                        }
                    }

                    var newCluster = new Cluster();
                    double newDelta = newCluster.DeltaAdd(t, _r);

                    if (newDelta > bestDelta)
                    {
                        newCluster.Add(t);
                        _clusters.Add(newCluster);
                        a.Cluster = newCluster;
                        moved = true;
                    }
                    else
                    {
                        bestCluster.Add(t);

                        if (bestCluster != current)
                        {
                            a.Cluster = bestCluster;
                            moved = true;
                        }
                    }
                }

                _clusters.RemoveAll(c => c.IsEmpty);

            } while (moved);

            return assignments;
        }
    }
}