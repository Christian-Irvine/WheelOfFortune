using System.ComponentModel.DataAnnotations;

namespace WheelOfFortune
{
    internal class Program
    {
        //Initializes the Players struct type
        struct Players
        {
            public string lastName;
            public string firstName;
            public string interest;
            public int score;
        }

        static Random rand = new Random();
        static string txtFile = "wheelOfFortuneTest.txt";
        const int PLAYERCOUNT = 38;

        static void Main()
        {
            Players[] playersArray = SetupPlayers();

            
            for (int i = 0; i < 200; i++)
            {
                int one = rand.Next(0, 38), two = rand.Next(0, 38);

                Players temp = playersArray[one];
                playersArray[one] = playersArray[two];
                playersArray[two] = temp;
            }
            

            Introduction();
            TaskMenu(playersArray);
        }

        static Players[] SetupPlayers()
        {
            Players[] playersArray;

            //Initializes the stream reader to read from txt files.
            playersArray = new Players[PLAYERCOUNT];
            StreamReader sr = new StreamReader(@txtFile);

            //Adds every players information to playersArray struct array.
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                playersArray[i].lastName = sr.ReadLine();
                playersArray[i].firstName = sr.ReadLine();
                playersArray[i].interest = sr.ReadLine();
                playersArray[i].score = Convert.ToInt32(sr.ReadLine());
            }

            return playersArray;
        }

        static void Introduction()
        {




        }

        static void TaskMenu(Players[] playersArray)
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
                        ListContestants(playersArray);
                        break;
                    case 2:
                        UpdatePlayerInterests(playersArray);
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

        //Lists all of the players in the playersArray.
        static void ListContestants(Players[] playersArray)
        {
            //Clones the array to create a sorted one without replacing the original.
            Players[] sortedPlayersArray = new Players[PLAYERCOUNT];
            Array.Copy(playersArray, sortedPlayersArray, playersArray.Length);

            //Calls the sorting array, returning an alphabetically sorted array of Players.
            sortedPlayersArray = SortContestants(sortedPlayersArray);

            Console.WriteLine("Surnames:".PadRight(16) + "First Names:\n");

            //Writes each player to the screen.
            foreach (Players playerInfo in sortedPlayersArray)
            {
                Console.WriteLine(playerInfo.lastName.PadRight(16) + playerInfo.firstName);
            }

            Console.ReadLine();
        }

        //Sorts Players struct array based on last name alphabetically.
        static Players[] SortContestants(Players[] playersArray)
        {
            bool swapOccurred = true;

            while (swapOccurred)
            {
                swapOccurred = false;

                for (int contestants = 0; contestants < playersArray.Length - 1; contestants++)
                {
                    int maxLength = FindShortestStringLength(playersArray[contestants].lastName, playersArray[contestants + 1].lastName);
                    int letterIndex = FindEarlistDifferentLetter(maxLength, playersArray[contestants + 1].lastName, playersArray[contestants].lastName);

                    if (playersArray[contestants].lastName[letterIndex] > playersArray[contestants + 1].lastName[letterIndex])
                    {
                        Players temp = playersArray[contestants];
                        playersArray[contestants] = playersArray[contestants + 1];
                        playersArray[contestants + 1] = temp;

                        swapOccurred = true;
                    }
                }
            }

            return playersArray;
        }

        static int FindShortestStringLength(string first, string second)
        {
            if (first.Length >= second.Length)
            {
                return second.Length;
            }

            else
            {
                return first.Length;
            }
        }

        static int FindEarlistDifferentLetter(int maxLength, string first, string second)
        {
            for (int i = 0; i < maxLength; i++)
            {
                if (first[i] != second[i])
                {
                    return i;
                }
            }

            return maxLength - 1;
        }

        static void UpdatePlayerInterests(Players[] playersArray)
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