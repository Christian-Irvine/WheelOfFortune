namespace WheelOfFortune
{
    internal class Program
    {
        struct Players
        {
            public string lastName;
            public string firstName;
            public string interest;
            public int score;
        }

        static Players[] playersArray;
        static string txtFile = "wheelOfFortune.txt";

        static void Main()
        {
            int playerCount = 38;

            SetupPlayers(playerCount);
            Introduction();
        }

        static void SetupPlayers(int playerCount)
        {
            playersArray = new Players[playerCount];
            StreamReader sr = new StreamReader(@txtFile);

            for (int i = 0; i < playerCount; i++)
            {
                playersArray[i].lastName = sr.ReadLine();
                playersArray[i].firstName = sr.ReadLine();
                playersArray[i].interest = sr.ReadLine();
                playersArray[i].score = Convert.ToInt32(sr.ReadLine());
            }
        }

        static void Introduction()
        {
            TaskMenu();
        }

        static void TaskMenu()
        {
            bool exitCode = false;
            int padValue = 8;
            string[] taskName = new string[] { "List Contestants", "Update Players Interests", "Pick Finalists", "Pick Player" };

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
                        UpdatePlayerInterests();
                        break;
                    case 3:
                        PickFinalists();
                        break;
                    case 4:
                        PickPlayer();
                        break;
                    default:
                        Console.WriteLine("Not a valid number, please try again");
                        Thread.Sleep(1000);
                        break;
                }
            } while (!exitCode);
        }

        static void ListContestants()
        {
            foreach (Players playerInfo in playersArray)
            {
                Console.WriteLine(playerInfo.firstName);
            }

            Console.ReadLine();
        }

        static void UpdatePlayerInterests()
        {

        }

        static void PickFinalists()
        {

        }

        static void PickPlayer()
        {

        }
    }
}