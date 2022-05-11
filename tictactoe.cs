using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace projekt2
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] board = new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int round = 0;
            while (true)
            {
                int choice = 0;
                writeBoard(board);

                if (round % 2 == 0)
                {
                    choice = playerChoice(board);
                    board = move(board, choice, 'X');
                }
                else
                {
                    choice = bestMove(board);
                    board = move(board, choice, 'O');
                }

                // Kolla om någon vunnit. 
                string state = checkWin(board);

                if (state == "win")
                {
                    writeBoard(board);
                    Console.WriteLine("Du vann");
                    break;
                }

                if (state == "lose")
                {
                    writeBoard(board);
                    Console.WriteLine("Du förlorade");
                    break;
                }

                if (state == "tie")
                {
                    writeBoard(board);
                    Console.WriteLine("Det blev lika");
                    break;
                }

                round += 1;

                Console.Clear();
            }

        }

        static void writeBoard(char[] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                Console.Write($"| {board[i]} ");
                if ((i + 1) % 3 == 0)
                {
                    Console.Write("|\n");
                }
            }
        }

        // Funktion för användare att skriva in val. 
        static int playerChoice(char[] board)
        {
            Console.Write("Skriv in val: ");
            int choice = 0;
            List<int> freeList = getEmptySquare(board);
            bool valid = false;
            try
            {
                choice = int.Parse(Console.ReadLine()) -1;
                for (int i = 0; i < freeList.Count; i++)
                {
                    if (choice == freeList[i])
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                {
                    throw new Exception();
                }
            }
            catch
            {
                // Om användare inte väljer en tillåten plats eller om användare inte skriver in int så ska funktionen köras igen.   
                Console.WriteLine("Error");
                choice = playerChoice(board);
            }

            return choice;
        }

        // Funktion för att uppdatera board[]. 
        static char[] move(char[] board, int idx, char player)
        {
            board[idx] = player;

            return board;
        }

        // Funktion för att kolla om någon vunnit eller om det blivit lika. 
        static string checkWin(char[] board)
        {
            // Lista med vinstkombinationer. 
            int[,] winCombination = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
            char[] players = new char[] { 'X', 'O' };
            string win = "no";
            for (int i = 0; i < players.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[winCombination[j, 0]] == players[i] && board[winCombination[j, 1]] == players[i] && board[winCombination[j, 2]] == players[i])
                    {
                        if (players[i] == 'X')
                        {
                            win = "win";
                        }
                        else
                        {
                            win = "lose";
                        }
                    }
                }
            }

            if (getEmptySquare(board).Count == 0 && win == "no")
            {
                win = "tie";
            }

            return win;
        }

        // Funktion som returnerar alla tomma positioner. 
        static List<int> getEmptySquare(char[] board)
        {
            List<int> freeList = new List<int>();
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != 'X' && board[i] != 'O')
                {
                    freeList.Add(i);
                }
            }

            return freeList;
        }

        static int getScore(char[] board, char player)
        {
            // Om du har förlorat ska 1 returneras då motståndare vill maxmimera sin score. 
            if (checkWin(board) == "lose")
            {
                return 1;
            }
            // Om du har vunnit ska -1 returneras då du vill minimera motståndarens score. 
            if (checkWin(board) == "win")
            {
                return -1;
            }
            if (checkWin(board) == "tie")
            {
                return 0;
            }
            // Hitta alla lediga platser i den nuvarande spelplanen. 
            List<int> freeList = getEmptySquare(board);
            char[] boardCpy = new char[9];
            if (player == 'O')
            {
                int bestScore = -10000;
                for (int i = 0; i < freeList.Count; i++)
                {
                    board.CopyTo(boardCpy, 0);
                    boardCpy[freeList[i]] = player;
                    int score = getScore(boardCpy, 'X');
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                }

                return bestScore;
            }
            else if (player == 'X')
            {
                int bestScore = 10000;
                for (int i = 0; i < freeList.Count; i++)
                {
                    board.CopyTo(boardCpy, 0);
                    boardCpy[freeList[i]] = player;
                    int score = getScore(boardCpy, 'O');
                    if (score < bestScore)
                    {
                        bestScore = score;
                    }
                }
                return bestScore;
            }

            return 0;
        }

        // Sätt in O (motståndare) på alla lediga platser och räkna ut score för varje plats. 
        static int bestMove(char[] board)
        {
            List<int> freeList = getEmptySquare(board);
            int bestScore = -10000;
            int idx = freeList[0];
            char[] boardCpy = new char[9];
            for (int i = 0; i < freeList.Count; i++)
            {
                board.CopyTo(boardCpy, 0);
                boardCpy[freeList[i]] = 'O';
                int score = getScore(boardCpy, 'X');
                if (score > bestScore)
                {
                    bestScore = score;
                    idx = freeList[i];
                }
            }

            return idx;
        }
    }
}
