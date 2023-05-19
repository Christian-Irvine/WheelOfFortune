namespace WheelOfFortune
{
    internal class Program
    {
        static void Main()
        {
            Introduction();
        }

        static void Introduction()
        {
            TaskMenu();
        }

        static void TaskMenu()
        {
            bool exitCode = false;
            int padValue = 8;
            string[] taskName = new string[] { "List Contestants", "Update Players Interests", "Pick Finalists", "Pick Player"};

            do
            {
                Console.Clear();

                Console.WriteLine("Options for the menu are: \n");


                for (int i = 1; i < 5; i++)
                {
                    Console.Write(i.ToString().PadRight(padValue));
                    Console.WriteLine($"{taskName[i - 1]}");
                }
                Console.Write("0".PadRight(padValue));
                Console.WriteLine("Exit Program\n");
                Console.WriteLine("Please input a number");

                int input = Convert.ToInt32(Console.ReadLine());

                Console.Clear();

                switch (input)
                {
                    case 0:
                        exitCode = true;
                        break;
                    case 1:
                        ListContestants();
                        break;
                    case 2:
                        //Task2(input);
                        break;
                    case 3:
                        //Task3(input);
                        break;
                    case 4:
                        //Task4(input);
                        break;
                    default:
                        Console.WriteLine("Not a valid number, please try again");
                        Thread.Sleep(1000);
                        break;
                }
            } while (exitCode == false);
        }

        static void ListContestants()
        {

        }
    }
}