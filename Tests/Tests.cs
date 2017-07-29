using System;
using System.Threading;
using System.IO;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    /// <summary>
    /// A simple class to run unit tests for this project.
    /// </summary>
    internal class Tests {
        // These are all the methods that will be run by the Main method
        public static Action[] Methods = { 
            Suite.SanityCheck,
            Suite.AllPuzzles,
        };
        
        public static void Main() {
            // This is the number of tests that have passed so far
            var passingTests = 0;

            // Invoke each of the test methods
            foreach (var method in Tests.Methods) {
                try {
                    
                    method.Invoke();

                    // The test passed
                    passingTests++;
                    Console.WriteLine($"Test {method.Method.Name} passed.");

                } catch (AssertionException e) {
                    Console.Error.WriteLine($"Test {method.Method.Name} failed: {e.Message}");
                }
            }

            // If the number of tests that passed equals those that we supplied, then this project is okay
            if (passingTests == Tests.Methods.Length) {
                Console.WriteLine($"Successfully finished all {Tests.Methods.Length} tests.");
            
            // Else, at least one test failed
            } else {
                var failingTests = Tests.Methods.Length - passingTests;
                Console.Error.WriteLine($"{failingTests} tests failed.");
            }
        }
    }

    /// <summary>
    /// Contains all the methods that form our test suite
    /// </summary>
    public static class Suite {
        // A simple sanity check
        public static void SanityCheck() {
            Assert.That(true == true, "C# booleans have gone crazy!");
        }

        // Runs all the puzzles found in the Tests/Files directory
        public static void AllPuzzles() {
            // Get all directory names
            string[] allTestDirectories = 
                Directory.GetDirectories("Tests/Files")
                .ToArray();

            Console.WriteLine($"Running tests from {allTestDirectories.Length} directories.");
            
            // Run each test case
            foreach (string testDirectory in allTestDirectories) {
                var problem = new RegularSudoku($"{testDirectory}/problem.txt");
                var solution = new RegularSudoku($"{testDirectory}/solution.txt");
                
                // The problem should not yet be solved!
                Assert.That(!problem.Equals(solution));
                
                // Track the execution time of function
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int numberOfSteps = problem.GenerateSolution();
                watch.Stop();
                
                // Now, the problem should be solved
                Assert.That(problem.Equals(solution));

                // Everything went well...
                Console.WriteLine($"Finished test in {testDirectory} after {numberOfSteps} steps in {watch.ElapsedMilliseconds / 1000} seconds.");
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
