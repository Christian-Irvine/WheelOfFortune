﻿namespace WheelOfFortune
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

        static string txtFile = "wheelOfFortune.txt";
        const int PLAYERCOUNT = 38;

        static void Main()
        {
            Players[] playersArray =  SetupPlayers();
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

        static void ListContestants(Players[] playersArray)
        {
            Players[] sortedPlayersArray = new Players[PLAYERCOUNT];
            Array.Copy(playersArray, sortedPlayersArray, playersArray.Length);

            sortedPlayersArray = SortContestants(playersArray);



            foreach (Players playerInfo in sortedPlayersArray)
            {
                Console.WriteLine(playerInfo.lastName.PadRight(20) + playerInfo.firstName);
            }

            Console.ReadLine();
        }

        //Sorts Players struct array based on last name alphabetically.
        static Players[] SortContestants(Players[] playersArray)
        {
            bool swap = true;

            //While not swapped.
            while (swap == false)
            {
                swap = false;

                //For every instance in the array, try to swap it.
                for (int i = 0; i < playersArray.Length - 1; i++)
                {
                    //Finds shortest last name in array to avoid 'Index Out of Bounds' error.
                    int maxLength = FindShortestStringLength(playersArray[i].lastName, playersArray[i + 1].lastName);

                    //Loop in case two letters are the same.
                    for (int diffCount = 0; diffCount < maxLength; diffCount++)
                    {
                        if (playersArray[i].lastName[diffCount] > playersArray[i + 1].lastName[diffCount])
                        {
                            Players temp = playersArray[i];

                            playersArray[i] = playersArray[i + 1];
                            playersArray[i + 1] = temp;

                            swap = true;
                        }
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