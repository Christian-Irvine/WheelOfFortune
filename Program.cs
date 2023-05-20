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

        //Initializing rand and some constants.
        static Random rand = new Random();
        const string TXTFILE = "wheelOfFortuneTest.txt";
        const int PLAYERCOUNT = 38;

        //The entry point of the code.
        static void Main()
        {
            Players[] playersArray = SetupPlayers();
            
            Introduction();
            TaskMenu(playersArray);
        }

        //Setting up players Array of structs.
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

        //The introduction that runs at the start.
        static void Introduction()
        {
            Console.WriteLine("Hello! and welcome to The Wheel of fortune.\n");
            WriteInstructions();
            Console.WriteLine("Good luck and have fun!\n\n");

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }

        //The instructions on how to play.
        static void WriteInstructions()
        {
            Console.WriteLine("To play this game you spin a wheel, to get a certain amount of money between -$3000 and $5000.");
            Console.WriteLine("Once you have spun you have to try and guess a word character by character, or have a go at the whole word.");
            Console.WriteLine("If you guess the character correctly your score is the spin amount multiplied by the number of occurrences of that character.");
            Console.WriteLine("If you select a vowel your score is reduced by the spin amount multiplied by the number of occurrences of that vowel.");
            Console.WriteLine("The score at the end of the guessing is added (or subtracted) to your total score.\n");
        }

        //Main Menu for the user to select what to do.
        static void TaskMenu(Players[] playersArray)
        {
            bool exitCode = false;
            int padValue = 8;
            string[] taskName = new string[] { "List Contestants", "Update Players Interests", "Pick Finalists", "Pick Player", "Play Game" };
            Players[] finalists = new Players[0];
            Players player = new Players();
            bool pickedFinalists = false;
            bool pickedPlayer = false;

            do
            {
                Console.Clear();

                Console.WriteLine("Options for the menu are: \n");


                for (int i = 1; i < 6; i++)
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
                        finalists = PickFinalists(playersArray);
                        pickedFinalists = true;
                        break;
                    case 4:
                        player = PickPlayer(finalists, pickedFinalists);
                        if (player.firstName != null)
                        {
                            pickedPlayer = true;
                        }
                        break;
                    case 5:
                        TheGame(pickedPlayer, player);
                        //pickedFinalists = false;
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

        //Finds the length of the shortest of two strings.
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

        //Finds the earlist letter that is not the same in two string;
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

        //Updates a specific users interest.
        static void UpdatePlayerInterests(Players[] playersArray)
        {
            Console.WriteLine("-= Interest Changer =-\n");
            bool correctAnswer;

            //Do while not the correct answer.
            do
            {
                //Ask if you would like the list of contestants.
                Console.WriteLine("Would you like to list the contestants? [Y or N]\n");
                string answer = Console.ReadLine();

                correctAnswer = false;

                //If the correct answer was submitted.
                if (answer.ToLower() == "y" || answer.ToLower() == "n")
                {
                    correctAnswer = true;

                    //If wanting them call the list contestants method.
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

            //Do while now the correct answer.
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
                            UpdateTextFromStructArray(playersArray);

                            Console.WriteLine("Press enter to continue.");
                            Console.ReadLine();
                        }
                    }

                    //If the typed name was wrong.
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

        //Updates the text file to an array.
        static void UpdateTextFromStructArray(Players[] playersArray)
        {
            //initializes the txt file to be written into.
            StreamWriter sw = new StreamWriter(@TXTFILE);

            //Loops for each player.
            for (int i = 0; i < PLAYERCOUNT; i++)
            {
                //Writes each line in the txt to be the player Struct information;
                sw.WriteLine(playersArray[i].lastName);
                sw.WriteLine(playersArray[i].firstName);
                sw.WriteLine(playersArray[i].interest);
                sw.WriteLine(playersArray[i].score.ToString());
            }

            //Closes the txt file so other programs and itself can use it later.
            sw.Close();
        }

        //Picks 10 random players to be the finalists
        static Players[] PickFinalists(Players[] playersArray)
        {
            LoadingLoop("Generating Finalists");

            Players[] finalists = new Players[10];
            //Creates an Array of all players to subtract from.
            Players[] playersSubtract = new Players[PLAYERCOUNT];
            
            //Coppies all players array to the subtracting one.
            Array.Copy(playersArray, playersSubtract, PLAYERCOUNT);

            for (int i = 0; i < 10; i++)
            {
                //Generates a random number from the possible people on the subtract list.
                int randNum = rand.Next(PLAYERCOUNT - i);

                finalists[i] = playersSubtract[randNum];

                //Moves the last player on the list to the selected persons position and then shortens the array.
                playersSubtract[randNum] = playersSubtract[playersSubtract.Length - 1];
                Array.Resize(ref playersSubtract, playersSubtract.Length - 1);
            }

            Console.WriteLine("Congratulations to the finalists: \n");

            //Lists all the finalists.
            foreach (Players player in finalists)
            {
                Console.WriteLine(player.firstName.PadRight(18) + player.lastName);
            }

            Console.WriteLine("\n\nPress Enter to continue.");

            Console.ReadLine();

            return finalists;
        }

        //Does loading animation.
        static void LoadingLoop(string message)
        {
            Console.Clear();
            //Generating loop.
            for (int i = 0; i < 3; i++)
            {
                for (int dots = 0; dots <= 3; dots++)
                {
                    //Adds dot count dots.
                    Console.Write(message + new string('.', dots));
                    Thread.Sleep(150);
                    Console.Clear();
                }
            }
        }

        static Players PickPlayer(Players[] finalists, bool pickedFinalists)
        {
            //If the finalists are picked
            if (pickedFinalists)
            {
                Console.WriteLine("The current finalists consist of: \n");

                foreach (Players players in finalists)
                {
                    Console.WriteLine(players.firstName.PadRight(18) + players.lastName);
                }

                Console.WriteLine("\nPress Enter to pick the finalist");
                Console.ReadLine();

                LoadingLoop("Picking Winner");

                Players player = finalists[rand.Next(10)];

                Console.WriteLine($"Congradulations to {player.firstName} {player.lastName}.");
                Console.WriteLine("You get to play the game!\n");

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();

                return player;
            }

            else
            {
                Console.WriteLine("Sorry you haven't picked the finalists yet, please go back and do that.\n");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();

                return new Players();
            }
        }

        static void TheGame(bool pickedPlayer, Players player)
        {
            if (pickedPlayer)
            {
                Console.WriteLine("Woo");

                Console.ReadLine();





            }

            else
            {
                Console.WriteLine("Sorry you haven't picked a player yet, please go back and do that.\n");
                Console.WriteLine("Press Enter to continue");
                Console.ReadLine();
            }
        }
    }
}