using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmployeePayrollUsingThreads
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Employee Payroll Using Threads");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks
            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin First Task: ");
                GetLongestWord(words);
            },//close first action
            () =>
            {
                Console.WriteLine("Begin Second Task: ");
                GetMostCommonWords(words);
            },
            () =>
            {
                Console.WriteLine("Begin Third Task: ");
                GetMostCommonWords(words);
            }//close third action
            );//close parallel,invoke
            #endregion
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);
        }

        private static string[] CreateWordArray(string uri)
        {
            Console.WriteLine($"Retrieving from {uri}");
            //Download a web page the easy way
            string blog = new WebClient().DownloadString(uri);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();
            Console.WriteLine($"Task 1 -- The longest words is {longestWord}.");
            return longestWord;
        }
    }
}
