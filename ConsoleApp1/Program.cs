using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

/// <summary>
/// Консольное приложение для демонстрации работы алгоритма кластеризации CLOPE.
/// Программа запрашивает у пользователя параметр r (коэффициент отталкивания),
/// выполняет кластеризацию набора транзакций и выводит результат в консоль.
///
/// Алгоритм CLOPE (Clustering with sLOPE) предназначен для кластеризации
/// категорийных и транзакционных данных и основан на максимизации глобальной функции стоимости.
///
/// Разработчик: Рябинин Сергей
/// </summary>
class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static void Main()
    {
        Console.WriteLine("=== CLOPE Clustering ===");
        Console.Write("Введите параметр r (>1): ");

        double r;
        while (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out r) || r <= 1)
        {
            Console.Write("Некорректный ввод. Введите r (>1): ");
        }

        var data = new List<Transaction>
        {
            new(new[] {"a","b"}),
            new(new[] {"a","b","c"}),
            new(new[] {"a","c","d"}),
            new(new[] {"d","e"}),
            new(new[] {"d","e","f"}),
            new(new[] {"a","b","e"}),
            new(new[] {"d","e","a"}),
        };

        var clope = new Clope(r);

        var timer = Stopwatch.StartNew();
        var result = clope.Fit(data);
        timer.Stop();
        var elapsedSec = timer.Elapsed.TotalSeconds;

        var clusters = result
            .GroupBy(x => x.Cluster)
            .Select((group, index) => new
            {
                Id = index + 1,
                Cluster = group.Key,
                Transactions = group.Select(x => x.Transaction).ToList()
            })
            .ToList();

        Console.WriteLine("\n=== Результаты кластеризации ===");

        foreach (var c in clusters)
        {
            Console.WriteLine($"\nКластер {c.Id}:");
            Console.WriteLine($"  Транзакций: {c.Cluster.N}");
            Console.WriteLine($"  S = {c.Cluster.S}, W = {c.Cluster.W}, H = {(c.Cluster.W == 0 ? 0 : (double)c.Cluster.S / c.Cluster.W):F2}");

            Console.WriteLine("  Транзакции:");

            foreach (var t in c.Transactions)
            {
                Console.WriteLine($"    [{string.Join(", ", t.Items)}]");
            }
        }

        Console.WriteLine($"\nВремя разбиеня по кластерам: {elapsedSec} секунд.");
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}