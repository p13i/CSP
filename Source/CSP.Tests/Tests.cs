using System;
using System.Diagnostics;
using System.Threading;

namespace ConstraintSatisfactionProblem
{
    /// <summary>
    /// A simple class to run unit tests for this project.
    /// </summary>
    internal class Tests {
        // These are all the methods that will be run by the Main method
        // Register new test cases here!
        public static Tuple<string, Action>[] Methods = { 
            new Tuple<string, Action>("Sanity Check", SanityCheck),

            new Tuple<string, Action>("Simple 1", () => RunTestCase("simple-1")),
            new Tuple<string, Action>("Simple 2", () => RunTestCase("simple-2")),
            new Tuple<string, Action>("Expert 1", () => RunTestCase("expert-1")),
            new Tuple<string, Action>("Expert 2", () => RunTestCase("expert-2")),
        };
        
        public static void Main() {
            // This is the number of tests that have passed so far
            int passingTestsCount = 0;

            // Invoke each of the test methods
            foreach (Tuple<string, Action> testNameAndmethod in Methods) {
                string name = testNameAndmethod.Item1;
                Action method = testNameAndmethod.Item2;
                try {
                    
                    Console.WriteLine();
                    Console.WriteLine($"- Running {name}...");

                    method();

                    // The test passed
                    passingTestsCount++;
                    Console.WriteLine($"|-- PASS {name}");

                } catch (AssertionException e) {
                    Console.Error.WriteLine($"|-- FAIL {name}: {e.Message}");
                }
            }

            // If the number of tests that passed equals those that we supplied, then this project is okay
            if (passingTestsCount == Methods.Length) {
                Console.WriteLine($"Successfully finished all {Tests.Methods.Length} tests.");
            
            // Else, at least one test failed
            } else {
                var failingTestCount = Methods.Length - passingTestsCount;
                Console.Error.WriteLine($"{failingTestCount} tests failed.");
            }
        }
        
        // A simple sanity check
        public static void SanityCheck() {
            Assert.That(true == true, "C# booleans have gone crazy!");
        }

        // Runs all the puzzles found in the Tests/Files directory
        public static void RunTestCase(string testDirectoryName) {
            // Construct the problem and solution copies of the Sudoku puzzle
            var problem = new RegularSudoku($"Files/{testDirectoryName}/problem.txt");
            var solution = new RegularSudoku($"Files/{testDirectoryName}/solution.txt");
            
            // The problem should not yet be solved!
            Assert.That(!problem.Equals(solution));
            
            Thread updatesThread = new Thread(PrintUpdates);
            updatesThread.Start();

            // Track the execution time of function
            Stopwatch watch = Stopwatch.StartNew();
            int numberOfSteps = problem.GenerateSolution();
            watch.Stop();

            updatesThread.Abort();
            
            // Now, the problem should be solved
            Assert.That(problem.Equals(solution));

            // Everything went well...
            Console.WriteLine($"|-- Finished test in {testDirectoryName} after {numberOfSteps} steps in {watch.ElapsedMilliseconds / 1000} seconds.");
        }

        // Prints updates for the given testName every interval milliseconds
        // (Run this function on its own thread)
        public static void PrintUpdates() {
            int interval = 5000;
            int updateNumber = 0;
            while (true) {
                Thread.Sleep(interval);
                updateNumber++;
                Console.WriteLine($"|-- Still working... Elapsed seconds: {updateNumber * interval / 1000}");
            }
        }
    }

    public static class Assert {
        public static void That(bool expression, string message = "No message.") {
            if (!expression) {
                throw new AssertionException($"Assertion failed: {message}");
            }
        }
    }

    // Simple subclass of Exception
    public class AssertionException : Exception {
        public AssertionException(string message) : base(message) {

        }
    }
}
