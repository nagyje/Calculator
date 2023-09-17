using System;
/////////////////////////////////////////
// 
// Author:  Joe Nagy, nagyje@etsu.edu
// Course CSCI 2210-001 - Data Structures
// Assignment: Project 7 - Calculator
//
/////////////////////////////////////////
namespace Project_7
{
    /// <summary>
    /// Program handles all the code and objects necessary to run the
    /// calculator. There are a few auxiliary methods that assist in file
    /// management and calculations: GetFile, WriteFile, Fibonacci, and Factorial.
    /// </summary>
    internal class Program
    {
        public static double answer = 0;
        /// <summary>
        /// The Main method is the driver for the calculator, and it contains all the logic
        /// required to run the loop as well as the functions of the calculator.
        /// The calculator's functions run from a dispatch table, and they are called
        /// based on the user's input. The input is split into different Strings 
        /// and sent to the dispatch table function accordingly. 
        /// The main loop is a while loop that can be cancelled by the user.
        /// Arguments are handled by checking the size of the user input, and 
        /// sending the inputs to respective functions based on that. 
        /// </summary>
        /// <param name="args">
        /// Default parameter
        /// </param>
        static void Main(string[] args)
        {
            // create necessary objects and variables
            Dictionary<String, dynamic> menuActions = new();

            List<Variable> vars = new List<Variable>();

            Stack<double> answerStack = new Stack<double>();
            answerStack.Push(0);

            bool isDouble;

            bool run = true;

            bool cont = false;

            // dispatch table
            menuActions["+"] = new Action<String, String>((num1, num2) => { answer = Double.Parse(num1) + Double.Parse(num2); answerStack.Push(answer); });
            menuActions["-"] = new Action<String, String>((num1, num2) => { answer = Double.Parse(num1) - Double.Parse(num2); answerStack.Push(answer); });
            menuActions["*"] = new Action<String, String>((num1, num2) => { answer = Double.Parse(num1) * Double.Parse(num2); answerStack.Push(answer); });
            menuActions["/"] = new Action<String, String>((num1, num2) => { answer = Double.Parse(num1) / Double.Parse(num2); answerStack.Push(answer); });
            menuActions["%"] = new Action<String, String>((num1, num2) => { answer = Double.Parse(num1) % Double.Parse(num2); answerStack.Push(answer); });
            menuActions["^2"] = new Action<String>((num1) => { answer = Double.Parse(num1) * Double.Parse(num1); answerStack.Push(answer); });
            menuActions["sqrt"] = new Action<String>((num1) => { answer = Math.Sqrt(Double.Parse(num1)); answerStack.Push(answer); });
            menuActions["^"] = new Action<String, String>((num1, num2) => { answer = Math.Pow(Double.Parse(num1), Double.Parse(num2)); answerStack.Push(answer); });
            menuActions["!"] = new Action<String>((num1) => { answer = Factorial(Double.Parse(num1)); answerStack.Push(answer); });
            menuActions["close"] = new Action<String>((var) => { run = false; });
            menuActions["clr"] = new Action<String>((var) => { answer = 0; answerStack.Push(answer); });
            menuActions["var"] = new Action<String>((var) => { vars.Add(new Variable(answer, var)); });
            menuActions["undo"] = new Action<String>((var) => { answerStack.Pop(); });
            menuActions["fib"] = new Action<String>((var) => { answer = Fibonacci(answer); answerStack.Push(answer); });
            menuActions["save"] = new Action<String>((filename) => { WriteFile(vars, filename); });
            menuActions["read"] = new Action<String>((filename) => { vars = GetFile(filename); });

            // main loop
            while (run)
            {
                // reset the continue variable 
                cont = false;

                // get answer from top of stack
                answer = answerStack.Peek();

                // print menu
                Console.Clear();
                Console.WriteLine("\n~~~~~~~~~~~~~");
                Console.WriteLine("____Answer____\n\t" + answer);
                Console.WriteLine("\n~~~~~~~~~~~~~");
                Console.WriteLine("FUNCTIONS");
                Console.WriteLine("Add: \t\t[number] + [number]");
                Console.WriteLine("Subtract: \t[number] - [number]");
                Console.WriteLine("Multiply: \t[number] * [number]");
                Console.WriteLine("Divide: \t[number] / [number]");
                Console.WriteLine("Mod: \t\t[number] % [number]");
                Console.WriteLine("Square: \t[number] ^2");
                Console.WriteLine("Square Root: \t[number] sqrt");
                Console.WriteLine("Exponentiate: \t[number] ^ [number]");
                Console.WriteLine("Factorial: \t[number] !");
                Console.WriteLine("Fibonacci: \tfib");
                Console.WriteLine("~~~~~~~~~~~~~");
                Console.WriteLine("ANSWER MANIPULATION");
                Console.WriteLine("Use Previous Answer:   \t\t substitute \"Ans\" for [number]");
                Console.WriteLine("Clear Previous Answer:  \t clr");
                Console.WriteLine("Store Answer as Variable: \t [variable] var");
                Console.WriteLine("Read Variables from File: \t [filepath] read");
                Console.WriteLine("Save Variables to File: \t [filepath] save");
                Console.WriteLine("Undo Last Function:  \t\t undo");
                Console.WriteLine("\nExit:  \t\t\t\t close");
                Console.Write("\nEnter function: ");

                // get user input and split it up
                String userInput = Console.ReadLine();
                String[] splitInput = userInput.Split(" ");

                // check for Ans or variables and convert them to their value
                for (int i = 0; i < splitInput.Length; i++)
                {
                    if (splitInput[i] == "Ans")
                    {
                        splitInput[i] = answer.ToString();
                    }
                    foreach (Variable v in vars)
                    {
                        if (splitInput[i] == v.name)
                        {
                            splitInput[i] = v.value.ToString();
                        }
                    }
                    isDouble = Double.TryParse(splitInput[i], out double result);
                    if (splitInput.Length > 1)
                    {
                        if (!isDouble && splitInput[1] != "var" && splitInput[1] != "save" && splitInput[1] != "read" && splitInput[0] != "undo" && splitInput[0] != "clr" && splitInput[0] != "close" && splitInput[0] != "fib")
                        {
                            Console.WriteLine("\n\n\nVariable not found!");
                            Thread.Sleep(1000);
                            cont = true;
                        }
                    } else if (!isDouble && splitInput[0] != "undo" && splitInput[0] != "clr" && splitInput[0] != "close" && splitInput[0] != "fib")
                    {
                        Console.WriteLine("\n\n\nVariable not found!");
                        Thread.Sleep(2000);
                        cont = true;
                    }
                        
                    i++;
                }
                // continue if variable was not found
                if (cont)
                {
                    continue;
                }

                // check length of the input to handle function parameters accordingly 
                if (splitInput.Length == 2)
                {
                    menuActions[splitInput[1]](splitInput[0]);
                } else if (splitInput.Length == 3)
                {
                    menuActions[splitInput[1]](splitInput[0], splitInput[2]);
                } else
                {
                    isDouble = Double.TryParse(splitInput[0], out double value);
                    if (isDouble)
                    {
                        answer = value;
                        answerStack.Push(answer);
                    } else
                    {
                        menuActions[splitInput[0]](splitInput[0]);
                    }
                }
                Console.WriteLine("Answer:\n" + answer);
            }
           
        }

        /// <summary>
        /// Calculates the factorial of a given number using 
        /// a recursive call, decrementing each time.
        /// </summary>
        /// <param name="num">
        /// number to be factorialized
        /// </param>
        /// <returns>
        /// factorialized number
        /// </returns>
        public static double Factorial(double num)
        {
            num = (int)num;
            if (num == 1)
                return 1;
            else
                return num * Factorial(num - 1);
        }

        /// <summary>
        /// Calculates the Fibonacci sequence value at the 
        /// given position, returns 0 and 1 for 
        /// num = 1, num = 2 respectively.
        /// </summary>
        /// <param name="num">
        /// Position in Fibonacci sequence
        /// </param>
        /// <returns>
        /// Value of requested position
        /// </returns>
        public static int Fibonacci(double num)
        {
            int number = (int)num;
            if (number == 1)
            {
                return 0;
            } else if (number == 2)
            {
                return 1;
            }
            int a = 0;
            int b = 1;
            int c = 0;
            for (int i = 2; i < number; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c;
        }

        /// <summary>
        /// Sends each variable in a list to a specified
        /// file after formatting the variable info
        /// "name:value"
        /// </summary>
        /// <param name="list">
        /// List of Variable objects
        /// </param>
        /// <param name="filename">
        /// File path user wants to write to
        /// </param>
        public static void WriteFile(List<Variable> list, String filename)
        {
            foreach (Variable v in list)
            {
                File.AppendAllText(filename, "\n" + v.GetInfo());
            }
        }

        /// <summary>
        /// Retrieves the contents of a file and reads each
        /// line into a new Variable object. They are then added
        /// to a list and returned.
        /// </summary>
        /// <param name="filename">
        /// File path of text variable list
        /// </param>
        /// <returns>
        /// New list of variables
        /// </returns>
        public static List<Variable> GetFile(String filename)
        {
            List<Variable> vars = new List<Variable>();
            String data = File.ReadAllText(filename);
            String[] splitdata = data.Split("\n");
            foreach (String line in splitdata)
            {
                String[] splitline = line.Split(":");
                if (splitline.Length == 2)
                {
                    vars.Add(new Variable(Double.Parse(splitline[1]), splitline[0]));
                }
            }
            return vars;
        }
    }
}