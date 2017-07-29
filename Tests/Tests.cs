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
        // Register new test cases here!
        public static Tuple<string, Action>[] Methods = { 
            new Tuple<string, Action>("Sanity Check", Tests.SanityCheck),

            new Tuple<string, Action>("Simple 1", () => Tests.RunTestCase("simple-1")),
            new Tuple<string, Action>("Simple 2", () => Tests.RunTestCase("simple-2")),
            new Tuple<string, Action>("Expert 1", () => Tests.RunTestCase("expert-1")),
            new Tuple<string, Action>("Expert 2", () => Tests.RunTestCase("expert-2")),
        };
        
        public static void Main() {
            // This is the number of tests that have passed so far
            var passingTests = 0;

            // Invoke each of the test methods
            foreach (var testNameAndmethod in Tests.Methods) {
                string name = testNameAndmethod.Item1;
                Action method = testNameAndmethod.Item2;
                try {
                    
                    method.Invoke();

                    // The test passed
                    passingTests++;
                    Console.WriteLine($"- Test {name} passed.");

                } catch (AssertionException e) {
                    Console.Error.WriteLine($"- Test {name} failed: {e.Message}");
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
        
        // A simple sanity check
        public static void SanityCheck() {
            Assert.That(true == true, "C# booleans have gone crazy!");
        }

        // Runs all the puzzles found in the Tests/Files directory
        public static void RunTestCase(string testDirectoryName) {
            // Construct the problem and solution copies of the Sudoku puzzle
            var problem = new RegularSudoku($"Tests/Files/{testDirectoryName}/problem.txt");
            var solution = new RegularSudoku($"Tests/Files/{testDirectoryName}/solution.txt");
            
            // The problem should not yet be solved!
            Assert.That(!problem.Equals(solution));
            
            // Track the execution time of function
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int numberOfSteps = problem.GenerateSolution();
            watch.Stop();
            
            // Now, the problem should be solved
            Assert.That(problem.Equals(solution));

            // Everything went well...
            Console.WriteLine($"|-- Finished test in {testDirectoryName} after {numberOfSteps} steps in {watch.ElapsedMilliseconds / 1000} seconds.");
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
