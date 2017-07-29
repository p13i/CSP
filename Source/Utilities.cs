using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    public static class Utilities
    {
        public static void EnsureDimensions(int[] array, int elementCount)
        {
            EnsureNotNull(array, "Array is null.");
            
            if (array.Length != elementCount)
            {
                var msg = $"Array has given length of {array.Length} but is required to have length {elementCount}.";
                throw new ArgumentException(msg);
            }
        }

        public static void EnsureDimensions(int[][] array, int rowCount, int columnCount)
        {
            EnsureNotNull(array, "Array is null.");
            if (array.Length != rowCount)
            {
                var msg = $"Array has given length of {array.Length} but is required to have length {rowCount}.";
                throw new ArgumentException(msg);
            }
            
            foreach (var row in array)
            {
                EnsureDimensions(row, columnCount);
            }
        }

        public static void EnsureNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new ArgumentException(message);
            }
        }

        public static int[][] CSVFileToIntArray(string filename)
        {
            var rows = new List<int[]>();
            var sr = new StreamReader(filename);

            while (!sr.EndOfStream)
            {
                rows.Add(sr.ReadLine().Split(',').Select(x => int.Parse(x)).ToArray());
            }
            var grid = rows.ToArray();

            return grid;
        }
        public static void ShowTicker() {
            char[] states = {'-', '\\', '|', '/'};

            var stateIndex = 0;
            while (true)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine($"{states[stateIndex]}");
                stateIndex = (stateIndex + 1) % states.Length;
                // Sleep for one second
                System.Threading.Thread.Sleep(250);
            }
        }

        public static void CleanupTicker() {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine(" ");
        }
    }

    /// <summary>
    /// Source: https://stackoverflow.com/a/2019433/5071723
    /// </summary>
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}