using System;
using System.Threading;

class Tetris
{
    static int boardWidth = 10;
    static int boardHeight = 20;
    static char[,] board = new char[boardHeight, boardWidth];
    static int currentX = boardWidth / 2;
    static int currentY = 0;
    static char tetromino = 'O';

    static char[,] tetrominoShape =
    {
        { ' ', 'O', ' ', ' ' },
        { ' ', 'O', ' ', ' ' },
        { ' ', 'O', 'O', ' ' },
        { ' ', ' ', ' ', ' ' }
    };

    static void Main()
    {
        Console.WindowHeight = boardHeight + 1;
        Console.WindowWidth = boardWidth * 2;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;

        InitializeBoard();
        InitializeTetromino();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        MoveTetromino(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveTetromino(1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        if (!MoveTetromino(0, 1))
                        {
                            // Si no se puede mover más abajo, detén el juego.
                            return;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        RotateTetromino();
                        break;
                }
            }

            Thread.Sleep(300); // Controla la velocidad de caída del tetromino.
        }

        Console.SetCursorPosition(0, boardHeight + 1);
        Console.WriteLine("Juego terminado. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }

    static void InitializeBoard()
    {
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                board[y, x] = ' ';
            }
        }
    }

    static void InitializeTetromino()
    {
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y, x + currentX] = tetromino;
                }
            }
        }
    }

    static void DrawBoard()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                Console.Write(board[y, x]);
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    static bool MoveTetromino(int deltaX, int deltaY)
    {
        // Borra la posición actual del tetromino en el tablero.
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y + currentY, x + currentX] = ' ';
                }
            }
        }

        // Calcula la nueva posición.
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;

        // Verifica si la nueva posición es válida.
        if (IsMoveValid(newX, newY))
        {
            // Actualiza la posición del tetromino.
            currentX = newX;
            currentY = newY;

            // Dibuja el tetromino en la nueva posición.
            for (int y = 0; y < tetrominoShape.GetLength(0); y++)
            {
                for (int x = 0; x < tetrominoShape.GetLength(1); x++)
                {
                    if (tetrominoShape[y, x] == 'O')
                    {
                        board[y + currentY, x + currentX] = tetromino;
                    }
                }
            }

            // Dibuja el tablero actualizado.
            DrawBoard();

            return true;
        }

        return false;
    }

    static bool IsMoveValid(int newX, int newY)
    {
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    int boardX = x + newX;
                    int boardY = y + newY;

                    if (boardX < 0 || boardX >= boardWidth || boardY >= boardHeight)
                    {
                        return false; // Movimiento inválido
                    }
                }
            }
        }

        return true;
    }

    static void RotateTetromino()
    {
        char[,] rotatedTetromino = new char[tetrominoShape.GetLength(1), tetrominoShape.GetLength(0)];

        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                rotatedTetromino[x, y] = tetrominoShape[y, tetrominoShape.GetLength(1) - 1 - x];
            }
        }

        if (IsRotationValid(rotatedTetromino))
        {
            // Borra la posición actual del tetromino en el tablero.
            for (int y = 0; y < tetrominoShape.GetLength(0); y++)
            {
                for (int x = 0; x < tetrominoShape.GetLength(1); x++)
                {
                    if (tetrominoShape[y, x] == 'O')
                    {
                        board[y + currentY, x + currentX] = ' ';
                    }
                }
            }

            // Actualiza la forma del tetromino rotado.
            tetrominoShape = rotatedTetromino;

            // Dibuja el tetromino en la nueva posición.
            for (int y = 0; y < tetrominoShape.GetLength(0); y++)
            {
                for (int x = 0; x < tetrominoShape.GetLength(1); x++)
                {
                    if (tetrominoShape[y, x] == 'O')
                    {
                        board[y + currentY, x + currentX] = tetromino;
                    }
                }
            }

            // Dibuja el tablero actualizado.
            DrawBoard();
        }
    }

    static bool IsRotationValid(char[,] rotatedTetromino)
    {
        for (int y = 0; y < rotatedTetromino.GetLength(0); y++)
        {
            for (int x = 0; x < rotatedTetromino.GetLength(1); x++)
            {
                if (rotatedTetromino[y, x] == 'O')
                {
                    int boardX = x + currentX;
                    int boardY = y + currentY;

                    if (boardX < 0 || boardX >= boardWidth || boardY >= boardHeight || boardY < 0 || boardX < 0)
                    {
                        return false; // Rotación inválida
                    }
                }
            }
        }

        return true;
    }
}





