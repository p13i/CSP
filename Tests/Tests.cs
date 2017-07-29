using System;
using System.Threading;
using System.IO;
using System.Linq;

namespace ConstraintSatisfactionProblem
{
    internal class Tests {
        public static Action[] Methods = { 
            Suite.SanityCheck,
            Suite.AllPuzzles,
        };
        
        public static void Main(string[] args) {
            var passingTests = 0;

            foreach (var method in Tests.Methods) {
                try {
                    method.Invoke();
                    passingTests++;
                    Console.WriteLine($"Test {method.Method.Name} passed.");
                } catch (AssertionException e) {
                    Console.Error.WriteLine($"Test {method.Method.Name} failed: {e.Message}");
                }
            }

            if (passingTests == Tests.Methods.Length) {
                Console.WriteLine($"Successfully finished all {Tests.Methods.Length} tests.");
            } else {
                var failingTests = Tests.Methods.Length - passingTests;
                Console.Error.WriteLine($"{failingTests} tests failed.");
            }
        }
    }

    public static class Suite {
        public static void SanityCheck() {
            Assert.That(true == true, "C# booleans have gone crazy!");
        }

        public static void AllPuzzles() {
            string[] allTestDirectories = 
                Directory.GetDirectories("Tests/Files")
                .ToArray();

            Console.WriteLine($"Running tests from {allTestDirectories.Length} directories.");
            
            foreach (string testDirectory in allTestDirectories) {
                var problem = new RegularSudoku($"{testDirectory}/problem.txt");
                var solution = new RegularSudoku($"{testDirectory}/solution.txt");
                
                Assert.That(!problem.Equals(solution));
                
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int numberOfSteps = problem.GenerateSolution();
                watch.Stop();
                
                Assert.That(problem.Equals(solution));

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

    public class AssertionException : Exception {
        public AssertionException(string message) : base(message) {

        }
    }
}
