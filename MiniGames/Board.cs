using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames
{
    public class Board
    {
        private int[,] board;

        public Board(int size)
        {
            board = new int[size, size];
        }

        public int Get(int x, int y)
        {
            return board[x, y];
        }

        public void Set(int x, int y, int value)
        {
            board[x, y] = value;
        }

        public int Size(int level)
        {
            return board.GetLength(level);
        }

        public bool IsEmpty(int x, int y)
        {
            return board[x, y] == 0;
        }
    }
}
