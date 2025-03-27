using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BadPracticesDemo
{
    // This code is implementing bad practices in purpose to demonstrate
    public class ProgramTest2
    {
        public static string CONNECTION_STRING = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Starting application...");
            
            var p = new ProgramTest2();
            var result = p.DoSomething(10);
            Console.WriteLine("Result: " + result);
            
            var data = p.LoadData();
            
            foreach(var d in data)
            {
                Console.WriteLine(d);
            }
            
            // Sleep for a while
            Thread.Sleep(5000);
            
            try 
            {
                p.RiskyOperation();
            }
            catch(Exception ex)
            {
                // Silently swallow exception
            }
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        
        public int DoSomething(int a)
        {
            int x = 0;
            
            // Magic numbers
            if (a > 5)
            {
                x = a * 42;
            }
            else
            {
                x = a * 13;
            }
            
            return x;
        }
        
        private List<string> LoadData()
        {
            var result = new List<string>();
            
            // Hardcoded path
            var filePath = "C:\\temp\\data.txt";
            
            if (File.Exists(filePath))
            {
                try
                {
                    // Not disposing resources properly
                    StreamReader reader = new StreamReader(filePath);
                    string line;
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.Add(line);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            
            return result;
        }
        
        public void RiskyOperation()
        {
            var random = new Random();
            var number = random.Next(0, 10);
            
            if (number > 7)
            {
                throw new Exception("Something went wrong!");
            }
            
            Console.WriteLine("Operation completed successfully");
        }
        
        // Unused method with poor naming
        private bool Flag(object o)
        {
            var x = o.ToString();
            if (x == null || x.Length < 5)
                return false;
            else
                return true;
        }
    }
    
    // Poor class structure - multiple classes in one file
}