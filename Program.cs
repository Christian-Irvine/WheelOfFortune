using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices;

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
        const string TXTFILE = "wheelOfFortune.txt";
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
            string[] taskName = new string[] { "List Contestants", "Update Players Interests", "Pick Finalists", "Pick Player", "Play Game", "Display Podium" };
            Players[] finalists = new Players[0];
            Players player = new Players();
            bool pickedFinalists = false;
            bool pickedPlayer = false;

            //Run menu until exit program command.
            do
            {
                Console.Clear();

                Console.WriteLine("Options for the menu are: \n");

                //Display the menu to the screen.
                for (int i = 1; i < taskName.Length + 1; i++)
                {
                    Console.Write(i.ToString().PadRight(padValue));
                    Console.WriteLine($"{taskName[i - 1]}");
                }
                Console.Write("0".PadRight(padValue));
                Console.WriteLine("Exit Program\n");
                Console.WriteLine("Please input a number");

                int input = Convert.ToInt32(Console.ReadLine());

                Console.Clear();

                //Check what input number means.
                switch (input)
                {
                    //Exit the program.
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
                        bool playedGame = TheGame(pickedPlayer, player, playersArray);

                        if (playedGame)
                        {
                            pickedFinalists = false;
                            pickedPlayer = false;
                        }
                        break;

                    case 6:
                        DisplayPodium(playersArray);
                        break;

                    //Incorrect number.
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
            Console.WriteLine("-= All Contestants =-\n");

            Console.WriteLine("Surnames:".PadRight(23) + "First Names:".PadRight(23) + "Interest:".PadRight(23) + "Score:");
            Console.WriteLine("-------------------------------------------------------------------------------");

            //Writes each player to the screen.
            foreach (Players playerInfo in sortedPlayersArray)
            {
                Console.WriteLine(playerInfo.lastName.PadRight(20) + " | " + playerInfo.firstName.PadRight(20) + " | " + playerInfo.interest.PadRight(20) + " | " + playerInfo.score.ToString().PadLeft(12));
            }

            Console.ReadLine();
        }

        //Sorts Players struct array based on last name alphabetically. (This was written before we covered the .Compare() Method, though it does still work for each letter even if inefficient.)
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
            //If the finalists are picked.
            if (pickedFinalists)
            {
                Console.WriteLine("The current finalists consist of: \n");

                //Write out finalists to screen.
                foreach (Players players in finalists)
                {
                    Console.WriteLine(players.firstName.PadRight(18) + players.lastName);
                }

                Console.WriteLine("\nPress Enter to pick the finalist");
                Console.ReadLine();

                //Plays loop of picking player.
                LoadingLoop("Picking Winner");

                //Picks random winner.
                Players player = finalists[rand.Next(10)];

                Console.WriteLine($"Congradulations to {player.firstName} {player.lastName}.");
                Console.WriteLine("You get to play the game!\n");

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();

                return player;
            }

            //If you haven't picked the finalists yet.
            else
            {
                Console.WriteLine("Sorry you haven't picked the finalists yet, please go back and do that.\n");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();

                //Empty player to do a check with.
                return new Players();
            }
        }

        //The actual gameplay itself.
        static bool TheGame(bool pickedPlayer, Players player, Players[] playersArray)
        {
            if (pickedPlayer)
            {
                Console.WriteLine($"Its time to play the game with our lucky contestant {player.firstName}");

                bool gameRunning = true, wordGuessed = false;
                int score = 0;

                string word = PickWord();
                //Creates char array of empty
                char[] wordGuess = new string('_', word.Length).ToCharArray();
                char[] vowels = { 'A', 'E', 'I', 'O', 'U' };
                char[] guessedLetters = new char[0];

                //While the word hasn't been guessed.
                while (!wordGuessed)
                {
                    Console.WriteLine("Press Enter to spin the wheel!!");
                    Console.ReadLine();

                    //Generates random number on wheel.
                    int pickedNumber = SpinTheWheel();

                    Console.WriteLine($"\n{player.firstName} rolled {pickedNumber:C0}!\n");
                    Console.WriteLine("Press Enter to continue.");                 
                    Console.ReadLine();

                    Console.Clear();

                    Console.WriteLine($"Your word is:");
                    Console.WriteLine(wordGuess);
                    Console.WriteLine($"\nGuess a letter, or the whole word for {pickedNumber:C0}");
                    Console.Write("Current guessed letters are: ");
                    foreach (char letterGuess in guessedLetters)
                    {
                        Console.Write($"{letterGuess} ");
                    }
                    Console.WriteLine("\n");

                    string input = Console.ReadLine().ToUpper();

                    //If single character guess.
                    if (input.Length == 1)
                    {
                        bool isVowel = false;
                        char letter = Convert.ToChar(input);

                        bool letterIsGuessed = false;

                        foreach (char letterGuess in guessedLetters)
                        {
                            if (letterGuess == letter)
                            {
                                letterIsGuessed = true;
                            }
                        }

                        //Putting on guessed letters array.
                        if (!letterIsGuessed)
                        {
                            Array.Resize(ref guessedLetters, guessedLetters.Length + 1);
                            guessedLetters[guessedLetters.Length - 1] = letter;

                            foreach (char vowel in vowels)
                            {
                                //input was a vowel.
                                if (vowel == letter)
                                {
                                    isVowel = true;
                                }
                            }

                            int letterCount = 0;

                            for (int i = 0; i < word.Length; i++)
                            {
                                //If word contains letter replace guess with it in that spot.
                                if (letter == word[i])
                                {
                                    wordGuess[i] = letter;

                                    letterCount++;
                                }
                            }

                            score += ProcessCharResults(letterCount, pickedNumber, isVowel, letter, player, playersArray);

                            Console.WriteLine($"\nYour current score is {score}.");
                            Console.Write($"Your current word is ");
                            Console.WriteLine(wordGuess);
                        }

                        else
                        {
                            Console.WriteLine($"Sorry, you've already guessed {letter}.");
                        }
                    }

                    //If not single character.
                    else
                    {
                        //If correct.
                        if (input == word)
                        {
                            wordGuess = word.ToCharArray();
                            wordGuessed = true;
                        }

                        else
                        {
                            Console.WriteLine($"Sorry {input} is not the word.");
                        }
                    }

                    //If word is fully guessed by convering char array to string.
                    if (new string(wordGuess) == word) // this is allways returning false
                    {
                        wordGuessed = true;
                    }
                }

                for (int i = 0; i < playersArray.Length; i++)
                {
                    if (player.firstName == playersArray[i].firstName && player.lastName == playersArray[i].lastName)
                    {
                        playersArray[i].score += score;
                    }
                }

                //Updates the text file with results.
                UpdateTextFromStructArray(playersArray);

                Console.WriteLine($"Congratulations! {player.firstName}, you completed the word '{word}'.");
                Console.WriteLine($"Your total score is {score}.\n");

                Console.WriteLine($"(Updated {player.firstName} {player.lastName}s total score.)\n");

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                Console.Clear();

                DisplayPodium(playersArray);

                //Returns that the game has run.
                return true;
            }

            else
            {
                Console.WriteLine("Sorry you haven't picked a player yet, please go back and do that.\n");
                Console.WriteLine("Press Enter to continue");
                Console.ReadLine();

                //Returns that the game hasn't run yet.
                return false;
            }
        }

        //Simulating spinning the wheel.
        static int SpinTheWheel()
        {
            //Array of possible numbers
            int[] numbers = { -3000, -2000, 300, 400, 500, 600, 700, 800, 900, 1000, 1200, 1400, 1500, 1600, 1800, 2000, 2250, 2500, 2750, 3000, 3500, 4000, 4500, 5000 };
            int pickedNumber = 0;
            
            //Wheel animation for loop.
            for (int i = 0; i < 30; i++)
            {
                Console.Clear();
                pickedNumber = numbers[rand.Next(0, numbers.Length)];

                //Makes a cool looking circle.
                Console.WriteLine("*+*+*+*+*");
                Console.WriteLine("+       +");
                Console.WriteLine("* " + pickedNumber.ToString().PadRight(5) + " *");
                Console.WriteLine("+       +");
                Console.WriteLine("*+*+*+*+*");

                //Waits based on for loop position.
                Thread.Sleep(i * 4);
            }

            Thread.Sleep(2000);

            return pickedNumber;
        }

        //Randomly generating a word from text file of words.
        static string PickWord()
        {
            string pickedWord = null;
            int wordCount = 0;
            const string WORDSFILE = "words.txt";

            //Open the words text file.
            StreamReader sr = new StreamReader(@WORDSFILE);

            //Get total word count.
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                wordCount++;
            }

            sr.Close();

            StreamReader sr2 = new StreamReader(@WORDSFILE);

            int wordIndex = rand.Next(wordCount);

            //Run until at word index.
            for (int i = 0; i <= wordIndex; i++)
            {
                sr2.ReadLine();

                //If at word index, set that as word.
                if (i == wordIndex)
                {
                    pickedWord = sr2.ReadLine();
                }
            }

            sr2.Close();

            return pickedWord;
        }

        //Calculate what the input of char means.
        static int ProcessCharResults(int letterCount, int pickedNumber, bool isVowel, char letter, Players player, Players[] playersArray)
        {
            //Displaying how many times the letter appeared.
            switch (letterCount)
            {
                case 0:
                case >= 2:
                    Console.WriteLine($"{letter} appeared {letterCount} times.");
                    break;

                case 1:
                    Console.WriteLine($"{letter} appeared {letterCount} time.");
                    break;
            }

            int result;

            //If you didn't pick a vowel.
            if (!isVowel)
            {
                result = letterCount * pickedNumber;
            }

            //If is a vowel.
            else
            {
                if (pickedNumber < 0)
                {
                    result = pickedNumber;
                }

                else
                {
                    result = letterCount * pickedNumber * -1;
                }
            }

            //If result than 0.
            if (result < 0)
            {
                Console.WriteLine($"\nSorry you lose {result} points");
            }

            else if (result > 0)
            {
                Console.WriteLine($"\nWoohoo. You gain {result} points!");
            }

            else
            {
                Console.WriteLine("\nSorry you didn't get it, you don't gain any points.");
            }

            return result;
        }

        //Write the top 5 players to the screen. 
        static void DisplayPodium(Players[] playersArray)
        {
            //Calculate the top 5 players.
            Players[] podiumPlayers = CalculatePodium(playersArray);
            

            Console.WriteLine("The current podium is:");

            //Write top 5 to screen.
            for (int i = 0; i < podiumPlayers.Length; i++)
            {
                //Format scores nicely
                Console.WriteLine((podiumPlayers[i].firstName + " " + podiumPlayers[i].lastName).PadRight(30) + podiumPlayers[i].score.ToString().PadLeft(6));
            }

            Console.WriteLine("\nPress Enter to continue.");
            Console.ReadLine();
        }

        //Sort players by score and return top 5.
        static Players[] CalculatePodium(Players[] playersArray)
        {
            Players[] podiumPlayers = new Players[playersArray.Length];
            Array.Copy(playersArray, podiumPlayers, playersArray.Length);

            for (int i = 0; i < podiumPlayers.Length; i++)
            {
                for (int player = 0; player < podiumPlayers.Length - 1; player++)
                {
                    //If score is bigger swap them.
                    if (podiumPlayers[player].score < podiumPlayers[player + 1].score)
                    {
                        Players temp = podiumPlayers[player];

                        podiumPlayers[player] = podiumPlayers[player + 1];
                        podiumPlayers[player + 1] = temp;
                    }
                }
            }

            //Sets array to only be the top 5
            Array.Resize(ref podiumPlayers, 5);
            return podiumPlayers;
        }
    }
}