using System;
using System.Collections.Generic;

namespace statki
{
    internal class Program
    {
        public static Player player1;
        public static Player player2;
        public static int player1Wins = 0;
        public static int player2Wins = 0;
        public static List<string> player1Shots = new List<string>();
        public static List<string> player2Shots = new List<string>(); 

        static void Main(string[] args)
        {
            Console.Clear();
            StartGame();
        }

        static void StartGame()
        {
            Board board = new Board(10);
            player1 = new Player(board);
            player2 = new Player(board);

            // Clear the lists of shots
            player1Shots.Clear();
            player2Shots.Clear();

            // Set up ships for player 1
            Console.WriteLine("Player 1, place your ships:");
            PlaceShips(player1, 1);
            Console.Clear();
            Console.WriteLine("When player 2 is ready press enter");
            Console.ReadKey();

            // Set up ships for player 2
            Console.WriteLine("Player 2, place your ships:");
            PlaceShips(player2, 2);
            Console.Clear();
            Console.WriteLine("When player 1 is ready press enter");
            Console.ReadKey();

            // Start the game
            PlayGame(player1, player2);

        }
        static void PlaceShips(Player player, int playerNumber)
        {
            player.PlaceShipsOnBoard();
        }


        static void PlayGame(Player player1, Player player2)
        {
            bool player1Turn = true;

            while (true)
            {
                Console.Clear(); 
                if (player1Turn)
                {
                    Console.WriteLine("Player 1's turn:");
                    player1.DisplayBoards();
                    string input;
                    do
                    {
                        Console.WriteLine("Player 1, enter coordinates to shoot (e.g., A5):");
                        input = Console.ReadLine().ToUpper(); 
                        if (player1Shots.Contains(input) == true) 
                        {
                            Console.WriteLine("You already shot in this location.");
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                        }
                    }
                    while (!ValidateInput(input) || player1Shots.Contains(input));
                    player1Shots.Add(input);
                    Shoot(player2, player1, input); // Player 1 shoots at player 2
                    Console.Clear();
                    Console.WriteLine("Press Enter when player 2 is ready to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Player 2's turn:");
                    player2.DisplayBoards();
                    string input;
                    do
                    {
                        Console.WriteLine("Player 2, enter coordinates to shoot (e.g., A5):");
                        input = Console.ReadLine().ToUpper(); 
                        if (player2Shots.Contains(input)) 
                        {
                            Console.WriteLine("You already shot in this location.");
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                        }
                    }
                    while (!ValidateInput(input) || player2Shots.Contains(input));
                    player2Shots.Add(input); 
                    Shoot(player1, player2, input); // Player 2 shoots at player 1
                    Console.Clear();
                    Console.WriteLine("Press Enter when player 1 is ready to continue...");
                    Console.ReadLine();
                }


                player1Turn = !player1Turn; // Change player turn

            }
        }

        static int[] ParseCoordinates(string input)
        {
            int col = input[0] - 'A'; // Convert column letter to numeric value (A=0, B=1, ...)
            int row = int.Parse(input.Substring(1)) - 1; // Get the row number and adjust to 0-based index
            return new int[] { row, col }; ;
        }

        static bool ValidateInput(string input)
        {
            // Parse the input to get the row and column
            if (input.Length >= 2 && char.IsLetter(input[0]) && char.IsDigit(input[1]))
            {
                int col = input[0] - 'A'; 
                int row = int.Parse(input.Substring(1)) - 1;

                if (row >= 0 && row < 10 && col >= 0 && col < 10)
                    return true;
            }
            return false;
        }

        static void Shoot(Player targetPlayer, Player shootingPlayer, string input)
        {
            Console.Clear();
            // Parse the input to get the row and column
            int[] coords = ParseCoordinates(input);
            int row = coords[0];
            int col = coords[1];

            Console.WriteLine(targetPlayer == player1 ? "Player 2's turn:" : "Player 1's turn:");

            // Add the shot to the list of shots taken
            if (shootingPlayer == player1)
                player1Shots.Add(input);
            else
                player2Shots.Add(input);


            // Check if the shot hits a ship on the target player's ships board
            if (targetPlayer.ShipsBoard.Grid[row, col] == 'S')
            {
                shootingPlayer.TargetBoard.Grid[row, col] = 'X'; 
                targetPlayer.ShipsBoard.Grid[row, col] = 'X'; 
                shootingPlayer.DisplayBoards(); // Show the boards after hit
                Console.WriteLine("Hit!");
              
                if (IsGameEnded(targetPlayer))
                {
                    EndGameInfo(shootingPlayer, targetPlayer);
                    return; // End the current game loop
                }

                Console.WriteLine("You can shoot again because you hit the enemy's ship.");
                string nextInput;
                do
                {
                    Console.WriteLine("Enter coordinates to shoot again (e.g., A5)");
                    nextInput = Console.ReadLine().ToUpper();

                    if (!ValidateInput(nextInput))
                    {
                        Console.WriteLine("Invalid input format. Please enter coordinates in the format 'A5' within the range A-J and 1-10.");
                        continue;
                    }
                    if (shootingPlayer.TargetBoard.Grid[ParseCoordinates(nextInput)[0], ParseCoordinates(nextInput)[1]] != '-') 
                    {
                        Console.WriteLine("You already shot in this location.");
                        continue;
                    }

                    
                    Shoot(targetPlayer, shootingPlayer, nextInput);
                    return;
                } while (true);
            }
            else
            {
                shootingPlayer.TargetBoard.Grid[row, col] = 'O'; // miss
                Console.Clear();
                Console.WriteLine("miss");
            }


            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }


        static bool IsGameEnded(Player player)
        {
            foreach (Ship ship in player.Ships)
            {
                for (int i = 0; i < player.ShipsBoard.Size; i++)
                {
                    for (int j = 0; j < player.ShipsBoard.Size; j++)
                    {
                        if (player.ShipsBoard.Grid[i, j] == 'S')
                        {
                            return false; // At least one ship cell is still afloat
                        }
                    }
                }
            }
            return true; // All ships are sunk
        }


        static void EndGameInfo(Player winner, Player loser)
        {
            if (winner == player1)
            {
                Console.WriteLine("Player 1 wins! All ships sunk");
                player1Wins++;
            }
            else
            {
                Console.WriteLine("Player 2 wins! All ships sunk");
                player2Wins++;
            }

            Console.WriteLine("Do you want to play again? (Y/N)");
            string playAgainResponse = Console.ReadLine().ToUpper();
            if (playAgainResponse == "Y")
            {
                Console.WriteLine($"Player 1 Score: {player1Wins} \nPlayer 2 Score: {player2Wins} ");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
                StartGame(); // Start a new game
            }
            else
            {
                Console.WriteLine($"Player 1 Score: {player1Wins} \nPlayer 2 Score: {player2Wins} ");
                Console.WriteLine("Thank you for playing! Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

    }
}
