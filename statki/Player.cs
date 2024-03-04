using System;
using System.Collections.Generic;

namespace statki
{
    internal class Player
    {
        public Board ShipsBoard { get; }
        public Board TargetBoard { get; }
        public List<Ship> Ships { get; }


        public Player(Board board)
        {
            ShipsBoard = new Board(board.Size);
            TargetBoard = new Board(board.Size);
            Ships = new List<Ship>();
            InitializeShips();
        }


        private void InitializeShips()
        {
            // Create a ship of length 4
            Ship ship1 = new Ship(4);
            Ships.Add(ship1);

            // Create two ships of length 3
            Ship ship2 = new Ship(3);
            Ships.Add(ship2);
            Ship ship3 = new Ship(3);
            Ships.Add(ship3);

            //Create three ships of length 2
            Ship ship4 = new Ship(2);
            Ships.Add(ship4);
            Ship ship5 = new Ship(2);
            Ships.Add(ship5);
            Ship ship6 = new Ship(2);
            Ships.Add(ship6);

            //Create four ships of length 1
            Ship ship7 = new Ship(1);
            Ships.Add(ship7);
            Ship ship8 = new Ship(1);
            Ships.Add(ship8);
            Ship ship9 = new Ship(1);
            Ships.Add(ship9);
            Ship ship10 = new Ship(1);
            Ships.Add(ship10);
        }

        public void DisplayBoards()
        {
            Console.WriteLine("Ships Board:");
            ShipsBoard.Display();
            Console.WriteLine("Target Board:");
            TargetBoard.Display();
        }

        public void PlaceShipsOnBoard()
        {
            List<string> errorMessages = new List<string>();
            foreach (Ship ship in Ships)
            {
                bool placed = false;
                while (!placed)
                {
                    Console.Clear();
                    // View previous error messages
                    foreach (string errorMessage in errorMessages)
                    {
                        Console.WriteLine(errorMessage);
                    }
                    errorMessages.Clear();


                    Console.WriteLine($"Place your ship size {ship.Size} on the board:");
                    ShipsBoard.Display(); // View the board before introducing the ship
                    Console.WriteLine("Provide coordinates (e.g., A5) for the head of the ship:");
                    string input = Console.ReadLine().ToUpper(); // Load data and convert to uppercase

                    // Check if input has at least two characters
                    if (input.Length >= 2 && char.IsLetter(input[0]) && char.IsDigit(input[1]))
                    {
                        int col = input[0] - 'A'; // Convert column letter to numeric value (A=0, B=1, ...)
                        int row = int.Parse(input.Substring(1)) - 1; // Get the row number and adjust to 0-based index

                        Console.WriteLine($"Coordinates: {input}"); // Debugging output

                        if (row >= 0 && row < ShipsBoard.Size && col >= 0 && col < ShipsBoard.Size)
                        {
                            char orientation = 'h'; // By default, we set the orientation to horizontal

                            // For ships with a length greater than 1, we ask for orientation
                            // For a ship with a length of 1 it does not matter
                            if (ship.Size > 1)
                            {
                                Console.WriteLine("Specify the orientation (h for horizontal, v for vertical):");
                                string orientationInput = Console.ReadLine().ToLower();

                                // Debugging output
                                Console.WriteLine($"Orientation received: {orientationInput}");

                                if (orientationInput == "h" || orientationInput == "v")
                                {
                                    orientation = orientationInput[0];
                                }
                                else
                                {
                                    errorMessages.Add("Incorrect orientation. Enter 'h' for horizontal or 'v' for vertical.");
                                    continue; 
                                }
                            }

                            // Check if the placement is valid
                            bool validPlacement = IsPlacementValid(orientation, row, col, ship.Size);

                            // If valid placement, place the ship on the board
                            if (validPlacement)
                            {
                                PaintTheShipsOnTheBoard(orientation, row, col, ship.Size);
                                placed = true;
                            }
                            else
                            {
                                errorMessages.Add("Incorrect placement. Ship overlaps another ship or goes outside the board. Try again.");
                            }
                        }
                        else
                        {
                            errorMessages.Add("invalid coordinates. Enter the coordinates within the range of the board.");
                        }
                    }
                    else
                    {
                        errorMessages.Add("Incorrect input format. Enter coordinates in format (e.g., A5).");
                    }
                }
            }

            Console.Clear();
            Console.WriteLine("Wszystkie statki pomyślnie umieszczone!");
            Console.WriteLine("Pozycje twoich statków: ");
            ShipsBoard.Display();
            ClearForbiddenAreas();
            Console.WriteLine("Aby kontynuować naciśnij enter");
            Console.ReadLine();
        }

        private bool IsPlacementValid(char orientation, int row, int col, int shipSize)
        {
            bool validPlacement = true;

            if (orientation == 'h')
            {
                if (col + shipSize > ShipsBoard.Size)
                {
                    validPlacement = false;
                }
                else
                {
                    for (int i = col; i < col + shipSize; i++)
                    {
                        if (ShipsBoard.Grid[row, i] != '-')
                        {
                            validPlacement = false;
                            break;
                        }
                    }
                }
            }
            else if (orientation == 'v')
            {
                if (row + shipSize > ShipsBoard.Size)
                {
                    validPlacement = false;
                }
                else
                {
                    for (int i = row; i < row + shipSize; i++)
                    {
                        if (ShipsBoard.Grid[i, col] != '-')
                        {
                            validPlacement = false;
                            break;
                        }
                    }
                }
            }

            return validPlacement;
        }

        private void PaintTheShipsOnTheBoard(char orientation, int row, int col, int shipSize)
        {
            if (orientation == 'h')
            {
                for (int i = col; i < col + shipSize; i++)
                {
                    ShipsBoard.Grid[row, i] = 'S'; // 'S' represents the ship
                }
            }
            else if (orientation == 'v')
            {
                for (int i = row; i < row + shipSize; i++)
                {
                    ShipsBoard.Grid[i, col] = 'S'; // 'S' represents the ship
                }
            }

            MarkForbiddenAreas();
        }


        private void MarkForbiddenAreas()
        {
            for (int row = 0; row < ShipsBoard.Size; row++)
            {
                for (int col = 0; col < ShipsBoard.Size; col++)
                {
                    if (ShipsBoard.Grid[row, col] == 'S')
                    {
                        // Check the box above
                        if (row > 0)
                        {
                            if (ShipsBoard.Grid[row - 1, col] != 'S')
                                ShipsBoard.Grid[row - 1, col] = 'B'; // 'B' means forbidden area
                        }

                        // Check the box below
                        if (row < ShipsBoard.Size - 1)
                        {
                            if (ShipsBoard.Grid[row + 1, col] != 'S')
                                ShipsBoard.Grid[row + 1, col] = 'B';
                        }

                        // Check the box on the left
                        if (col > 0)
                        {
                            if (ShipsBoard.Grid[row, col - 1] != 'S')
                                ShipsBoard.Grid[row, col - 1] = 'B';
                        }

                        // Check the box on the right
                        if (col < ShipsBoard.Size - 1)
                        {
                            if (ShipsBoard.Grid[row, col + 1] != 'S')
                                ShipsBoard.Grid[row, col + 1] = 'B';
                        }

                        // Mark the field on the diagonal left and above
                        if (row > 0 && col > 0 && ShipsBoard.Grid[row - 1, col - 1] != 'S')
                            ShipsBoard.Grid[row - 1, col - 1] = 'B';

                        // Mark the field on the diagonal right and above
                        if (row > 0 && col < ShipsBoard.Size - 1 && ShipsBoard.Grid[row - 1, col + 1] != 'S')
                            ShipsBoard.Grid[row - 1, col + 1] = 'B';

                        // Marl the field on the diagonal left and below
                        if (row < ShipsBoard.Size - 1 && col > 0 && ShipsBoard.Grid[row + 1, col - 1] != 'S')
                            ShipsBoard.Grid[row + 1, col - 1] = 'B';

                        // Marl the field on the diagonal right and below
                        if (row < ShipsBoard.Size - 1 && col < ShipsBoard.Size - 1 && ShipsBoard.Grid[row + 1, col + 1] != 'S')
                            ShipsBoard.Grid[row + 1, col + 1] = 'B';
                    }
                }
            }
        }

        private void ClearForbiddenAreas()
        {
            for (int row = 0; row < ShipsBoard.Size; row++)
            {
                for (int col = 0; col < ShipsBoard.Size; col++)
                {
                    if (ShipsBoard.Grid[row, col] == 'B')
                    {
                        ShipsBoard.Grid[row, col] = '-';
                    }
                }
            }
        }

       

    }
}
