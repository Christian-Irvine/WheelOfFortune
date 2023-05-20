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
        const string TXTFILE = "wheelOfFortuneTest.txt";
        const int PLAYERCOUNT = 38;

        static void Main()
        {
            Players[] playersArray = SetupPlayers();
            
            Introduction();
            TaskMenu(playersArray);
        }

        static Players[] SetupPlayers()
        {
            Players[] playersArray;

            //Initializes the stream reader to read from txt files.
            playersArray = new Players[PLAYERCOUNT];
            StreamReader sr = new StreamReader(@TXTFILE);

            //Adds every players information to playersArray struct array.
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                playersArray[i].lastName = sr.ReadLine();
                playersArray[i].firstName = sr.ReadLine();
                playersArray[i].interest = sr.ReadLine();
                playersArray[i].score = Convert.ToInt32(sr.ReadLine());
            }

            sr.Close();

            return playersArray;
        }

        static void Introduction()
        {
            Console.WriteLine("Hello! and welcome to The Wheel of fortune.\n");
            WriteInstructions();
            Console.WriteLine("Good luck and have fun!\n\n");

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }

        static void WriteInstructions()
        {
            Console.WriteLine("To play this game you spin a wheel, to get a certain amount of money between -$3000 and $5000.");
            Console.WriteLine("Once you have spun you have to try and guess a word character by character, or have a go at the whole word.");
            Console.WriteLine("If you guess the character correctly your score is the spin amount multiplied by the number of occurrences of that character.");
            Console.WriteLine("If you select a vowel your score is reduced by the spin amount multiplied by the number of occurrences of that vowel.");
            Console.WriteLine("The score at the end of the guessing is added (or subtracted) to your total score.\n");
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
            Console.WriteLine("-= Name Lister =-\n");

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

            //While a swap has occured.
            while (swapOccurred)
            {
                swapOccurred = false;

                //For every contestant.
                for (int contestants = 0; contestants < playersArray.Length - 1; contestants++)
                {
                    //Find the earlist possible letter that can be checked to be swapped, with a maximum to avoid 'Index Out Of Array' errors.
                    int maxLength = FindShortestStringLength(playersArray[contestants].lastName, playersArray[contestants + 1].lastName);
                    int letterIndex = FindEarlistDifferentLetter(maxLength, playersArray[contestants + 1].lastName, playersArray[contestants].lastName);

                    //Checks if a swap is reqired and swaps if needed.
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
            Console.WriteLine("-= Interest Changer =-\n");
            bool correctAnswer;

            do
            {
                Console.WriteLine("Would you like to list the contestants? [Y or N]\n");
                string answer = Console.ReadLine();

                correctAnswer = false;

                if (answer.ToLower() == "y" || answer.ToLower() == "n")
                {
                    correctAnswer = true;

                    if (answer.ToLower() == "y")
                    {
                        Console.WriteLine("\n");
                        ListContestants(playersArray);
                        Console.WriteLine("\n");
                    }
                }

            } while (!correctAnswer);

            Console.WriteLine("\nPlease input the name of the player you want to change interests of.\nOr you can leave it blank to exit.\nPlease use the format [FirstName Surname]\n");

            correctAnswer = true;

            do
            {
                string input = Console.ReadLine();
                string[] splitInput = input.Split(' ');
                
                //If the input is not empty;
                if (input != "")
                {
                    bool foundPlayer = false;

                    //For every player in the Struct Array.
                    for (int i = 0; i < playersArray.Length; i++)
                    {
                        //If the player name is in the Array of players.
                        if (playersArray[i].firstName.ToLower() == splitInput[0].ToLower() && playersArray[i].lastName.ToLower() == splitInput[1].ToLower())
                        {
                            foundPlayer = true;
                            string oldInterest = playersArray[i].interest;

                            Console.WriteLine($"{input} currently {oldInterest} \nWhat would you like {input}s new interest to be?\n");

                            //Sets new interest to input.
                            string newInterest = Console.ReadLine();
                            playersArray[i].interest = newInterest;

                            Console.WriteLine($"{input}s new interest is {newInterest}.\n");

                            //Change the interest of the person in the txt file.
                            UpdateTextToStructArray(playersArray);

                            Console.WriteLine("Press enter to continue.");
                        }
                    }

                    if (!foundPlayer)
                    {
                        Console.WriteLine("Sorry that isn't correct. \nPlease try again or leave it blank to exit");
                        correctAnswer = false;
                    }
                }

                else
                {
                    correctAnswer = true;
                }

            } while (!correctAnswer);
        }

        static void UpdateTextToStructArray(Players[] playersArray)
        {
            string[] txtFile = new string[PLAYERCOUNT * 4];

            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                txtFile[i * 4] = playersArray[i].lastName;
                txtFile[i * 4 + 1] = playersArray[i].firstName;
                txtFile[i * 4 + 2] = playersArray[i].interest;
                txtFile[i * 4 + 3] = playersArray[i].score.ToString();
            }

            StreamWriter sw = new StreamWriter(@TXTFILE);

            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                sw.WriteLine(txtFile[i * 4]);
                sw.WriteLine(txtFile[i * 4 + 1]);
                sw.WriteLine(txtFile[i * 4 + 2]);
                sw.WriteLine(txtFile[i * 4 + 3]);
            }

            sw.Close();
        }

        static void PickFinalists()
        {

        }

        static void PickPlayer()
        {

        }
    }
}