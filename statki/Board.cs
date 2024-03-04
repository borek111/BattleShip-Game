using System;

namespace statki
{
    internal class Board
    {
        public int Size { get;}
        public char[,] Grid { get;}
        public Board(int size)
        {
            this.Size = size;
            Grid = new char[size, size];
            Initialize();

        }
        public void Initialize()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j<Size;j++)
                {
                    Grid[i, j] = '-';
                }
            }
        }
        public void Display()
        {
            Console.WriteLine("    A B C D E F G H I J");
            for (int i = 0; i < Size; i++)
            {
                Console.Write($"{i + 1,3} ");
                for (int j = 0; j < Size; j++)
                {
                    char symbol = Grid[i, j];
                    if (symbol == 'S') // 'S' - ship
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // color
                        Console.Write($"{symbol} ");
                        Console.ResetColor();
                    }
                    else if (symbol == 'B') // 'B' - forbidden area
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // color
                        Console.Write($"{symbol} ");
                        Console.ResetColor();
                    }
                    else if (symbol == 'X') // Hit
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{symbol} ");
                        Console.ResetColor();
                    }
                    else if (symbol == 'O') // Miss
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{symbol} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"{symbol} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
