using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace Math_Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("testing 123!");
            Menu();
        }

        static void Menu()
        {
            List<string> history = new List<string>();
            var gameParemeters = ("", 0);
            int userOption = 0;
            bool gameParameterChosen = false;
            string menu = "Welcome to the math game! Please select a game mode: \n" +
    "1) Select game options \n" +
    "2) Play \n" +
    "3) View history \n" +
    "4) Exit";

            while (true)
            {
                Console.WriteLine(menu);

                if (!int.TryParse(Console.ReadLine(), out userOption))
                {
                    Console.WriteLine("Please enter a valid number (1 - 4): ");
                    continue;
                }


                if (userOption == 1)
                {
                    gameParemeters = getGameParameters();
                    gameParameterChosen = true;
                    continue;
                }
                else if (4 > userOption && userOption > 1 && gameParameterChosen == false)
                {
                    Console.WriteLine("Please select game parameters first");
                    continue;
                }
                else if (userOption == 2)
                {
                    while (true)
                    {
                        Console.WriteLine("do you want a random game? (yes/no)");
                        string userInputForRandomGame = Console.ReadLine();
                        userInputForRandomGame = userInputForRandomGame.ToLower();

                        if (userInputForRandomGame == "yes")
                        {
                            history.AddRange(playGame(gameParemeters.Item1, gameParemeters.Item2, true, 0));
                            break;
                        }
                        else if (userInputForRandomGame == "no")
                        {
                            Console.WriteLine("please choose an operator \n" +
                                "1) +\n" +
                                "2) -\n" +
                                "3) *\n" +
                                "4) /\n");

                            if (!int.TryParse(Console.ReadLine(), out int userChosenOperator))
                            {
                                Console.WriteLine("please enter 1 - 4 only");
                                continue;
                            }
                            else
                            {
                                history.AddRange(playGame(gameParemeters.Item1, gameParemeters.Item2, false, userChosenOperator - 1));
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please enter yes or no only");
                        }
                    }



                }
                else if (userOption == 3)
                {
                    if(history.Count == 0)
                    {
                        Console.WriteLine("No games played yet!");
                        continue;
                    }
                    foreach (string item in history)
                    {
                        Console.WriteLine(item);
                    }
                }
                else if (userOption == 4)
                {
                    Console.WriteLine("Thanks for playing!");
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid number ( 1 - 4 ): ");
                    continue;

                }
            }
        }

        static (string difficulty, int numberOfQuestions) getGameParameters()
        {

            string difficulty = "";
            int numberOfQuestions = 0;

            while (true)
            {
                Console.WriteLine("please enter a valid difficulty: easy, normal or hard");
                difficulty = Console.ReadLine();

                difficulty = difficulty.ToLower();
                if (difficulty != "easy" && difficulty != "normal" && difficulty != "hard")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("please enter the amount of questions you want: (5 - 99)");
                if (!int.TryParse(Console.ReadLine(), out numberOfQuestions) || numberOfQuestions < 5 || numberOfQuestions > 99)
                {
                    Console.WriteLine("Please enter a valid number between 5 and 99");
                    continue;
                }
                else
                {
                    break;
                }

            }
            return (difficulty, numberOfQuestions);

        }



        static List<string> playGame(string difficulty, int numberOfQuestions, bool randomGameBool, int userChosenOperator)
        {
            List<string> history = new List<string>();
            Random rand = new Random();
            Stopwatch sw = new Stopwatch();
            int correctCtr = 0;
            sw.Start();
            for (int i = 0; i < numberOfQuestions; i++)
            {

                int firstNumber = 0;
                int secondNumber = 0;
                if (difficulty == "easy")
                {
                    firstNumber = rand.Next(1, 11);
                    secondNumber = rand.Next(1, 11);
                }
                else if (difficulty == "normal")
                {
                    firstNumber = rand.Next(10, 100);
                    secondNumber = rand.Next(10, 100);
                }
                else if (difficulty == "hard")
                {
                    firstNumber = rand.Next(100, 1000);
                    secondNumber = rand.Next(100, 1000);
                }

                if (randomGameBool)
                {
                    int randomOperator = rand.Next(0, 4);
                    history.Add(AskQuestion(firstNumber, secondNumber, randomOperator));
                }
                else
                {
                    history.Add(AskQuestion(firstNumber, secondNumber, userChosenOperator));
                }
                if (history.Last().Contains("Correct!"))
                {
                    correctCtr++;
                }
            }
            sw.Stop();
            Console.WriteLine("Your score is: " + correctCtr + "/" + numberOfQuestions);
            Console.WriteLine("The total time taken(seconds) is: " + sw.Elapsed.TotalSeconds);
            Console.WriteLine("press enter to continue");
            Console.ReadLine();
            return history;
        }
        static string AskQuestion(int firstNumber, int secondNumber, int randomOperator)
        {

            String operatorSymbol = "";
            int realAnswer = 0;
            if (randomOperator == 0)
            {
                operatorSymbol = "+";
                realAnswer = firstNumber + secondNumber;
            }
            else if (randomOperator == 1)
            {
                operatorSymbol = "-";
                realAnswer = firstNumber - secondNumber;
            }
            else if (randomOperator == 2)
            {
                operatorSymbol = "*";
                realAnswer = firstNumber * secondNumber;
            }
            else if (randomOperator == 3)
            {
                operatorSymbol = "/";
                while (true)
                {
                    if (firstNumber % secondNumber == 0)
                    {
                        realAnswer = firstNumber / secondNumber;
                        break;
                    }
                    else
                    {
                        if (firstNumber < secondNumber)
                        {
                            int temp = secondNumber;
                            secondNumber = firstNumber;
                            firstNumber = temp;
                        }
                        firstNumber++;

                    }

                }

            }
            Console.WriteLine($"What is {firstNumber} {operatorSymbol} {secondNumber} ?");
            int userAnswerInt = 0;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out userAnswerInt))
                {
                    Console.WriteLine("Please enter a valid number");
                    continue;


                }
                else
                {
                    break;
                }

            }

            string result = realAnswer == userAnswerInt ? "Correct!" : $"Wrong! The correct answer is {realAnswer}";
            Console.WriteLine(result);
            return $"Question: {firstNumber} {operatorSymbol} {secondNumber}\t | Your answer: {userAnswerInt}\t | Result: {result}";

        }


    }
}
