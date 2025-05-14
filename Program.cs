using System;

namespace TicTacToeCommandLine
{
    class Program
    {
        static int[,] board = new int[3, 3];
        static int currentPlayer = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                InitializeBoard();
                currentPlayer = 1;
                PrintBoard();

                while (true)
                {
                    if (currentPlayer == 1)
                    {
                        Console.WriteLine("Player X's Turn (Enter row and column as 'row col'):");
                        PlayerMove();
                    }
                    else
                    {
                        Console.WriteLine("AI's Turn...");
                        AIMove();
                    }

                    PrintBoard();

                    if (CheckWin())
                    {
                        Console.WriteLine(currentPlayer == 1 ? "Player X wins!" : "AI (Player O) wins!");
                        break;
                    }

                    if (IsBoardFull())
                    {
                        Console.WriteLine("It's a draw!");
                        break;
                    }

                    currentPlayer *= -1;
                }

                Console.WriteLine("Press any key to play again...");
                Console.ReadKey();
            }
        }

        static void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    board[i, j] = 0;
        }

        static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine("Tic-Tac-Toe Board:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] == 1 ? " X " : board[i, j] == -1 ? " O " : " . ");
                    if (j < 2) Console.Write("|");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine("---+---+---");
            }
            Console.WriteLine();
        }

        static void PlayerMove()
        {
            while (true)
            {
                string[] input = Console.ReadLine().Trim().Split(' ');
                if (input.Length != 2) continue;

                bool validRow = int.TryParse(input[0], out int row);
                bool validCol = int.TryParse(input[1], out int col);

                if (validRow && validCol && row >= 0 && row < 3 && col >= 0 && col < 3 && board[row, col] == 0)
                {
                    board[row, col] = currentPlayer;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid move, try again:");
                }
            }
        }

        static void AIMove()
        {
            (int bestRow, int bestCol) = FindBestMove();
            board[bestRow, bestCol] = currentPlayer;
        }

        static (int, int) FindBestMove()
        {
            int bestScore = int.MinValue;
            int bestRow = -1, bestCol = -1;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                    {
                        board[i, j] = -1;
                        int score = Minimax(board, 0, false);
                        board[i, j] = 0;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = i;
                            bestCol = j;
                        }
                    }

            return (bestRow, bestCol);
        }

        static int Minimax(int[,] board, int depth, bool isMaximizing)
        {
            if (CheckWin())
                return isMaximizing ? -1 : 1;
            if (IsBoardFull())
                return 0;

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                    {
                        board[i, j] = isMaximizing ? -1 : 1;
                        int score = Minimax(board, depth + 1, !isMaximizing);
                        board[i, j] = 0;
                        bestScore = isMaximizing ? Math.Max(score, bestScore) : Math.Min(score, bestScore);
                    }

            return bestScore;
        }

        static bool CheckWin()
        {
            for (int i = 0; i < 3; i++)
                if (board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;
            for (int i = 0; i < 3; i++)
                if (board[0, i] != 0 && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return true;
            if (board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;
            if (board[0, 2] != 0 && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;
            return false;
        }

        static bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                        return false;
            return true;
        }
    }
}
