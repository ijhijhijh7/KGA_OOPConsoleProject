using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace MiniGames.Scenes
    {
        public class OmokScene : Scene
        {
            private Board board;
            private int curX, curY;
            private int turn;


            public override void Load()
            {
                board = new Board(19);
                curX = 0;
                curY = 0;
                turn = 0;
            }

            public override void Unload()
            {
                
            }

            public override void Render()
            {
                Console.Clear();
                Console.CursorVisible = false;

                Console.WriteLine("    < 오목 >");
                Console.Write(turn % 2 == 0 ? "흑돌 차례" : "백돌 차례");
                Console.WriteLine($"\t{turn + 1}수");
                string boardText = "";
                for (int i = 0; i < board.Size(0); i++)
                {
                    for (int j = 0; j < board.Size(1); j++)
                    {
                        if (i == curY && j == curX) boardText += "▣ ";
                        else if (board.Get(i, j) == 0) boardText += "┼ ";
                        else if (board.Get(i, j) == 1) boardText += "● ";
                        else boardText += "○ ";
                    }
                    boardText += "\n";
                }
                Console.WriteLine(boardText);
                Console.WriteLine("게임선택창으로 돌아가기 : Q");
            }

            public override void HandleInput(Game game)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        curX--;
                        break;
                    case ConsoleKey.RightArrow:
                        curX++;
                        break;
                    case ConsoleKey.UpArrow:
                        curY--;
                        break;
                    case ConsoleKey.DownArrow:
                        curY++;
                        break;
                    case ConsoleKey.Spacebar:
                        if (board.Get(curY, curX) == 0)
                        {
                            int currentStone = turn % 2 + 1;
                            board.Set(curY, curX, currentStone);

                            if (Check(currentStone))
                            {
                                Render();
                                Console.Clear();
                                Console.WriteLine(currentStone == 1 ? "흑돌 승리!" : "백돌 승리!");
                                Console.WriteLine("아무 키나 눌러 게임선택창으로 돌아가기");
                                Console.ReadKey();
                                game.ChangeScene(SceneType.GameSelection);
                            }
                            else
                            {
                                turn++;
                            }
                        }
                        break;
                    case ConsoleKey.Q:
                        game.ChangeScene(SceneType.GameSelection);
                        break;
                }

                if (curX < 0)
                {
                    curX = 0;
                }
                if (curY < 0)
                {
                    curY = 0;
                }
                if (curX >= board.Size(1))
                {
                    curX = board.Size(1) - 1;
                }
                if (curY >= board.Size(0))
                {
                    curY = board.Size(0) - 1;
                }
            }

            public override void Update(Game game) 
            {

            }

            private bool Check(int stone)
            {
                return CheckDirection(stone, 1, 0) ||
                       CheckDirection(stone, 0, 1) ||
                       CheckDirection(stone, 1, 1) ||
                       CheckDirection(stone, 1, -1);
            }

            private bool CheckDirection(int stone, int deltaX, int deltaY)
            {
                for (int i = 0; i < board.Size(0); i++)
                {
                    for (int j = 0; j < board.Size(1); j++)
                    {
                        if (IsOmok(i, j, stone, deltaX, deltaY))
                            return true;
                    }
                }
                return false;
            }

            private bool IsOmok(int startX, int startY, int stone, int deltaX, int deltaY)
            {
                int count = 0;
                bool isFiveInRow = false;
                for (int k = 0; k < board.Size(0); k++)
                {
                    int x = startX + k * deltaX;
                    int y = startY + k * deltaY;

                    if (x < 0 || y < 0 || x >= board.Size(0) || y >= board.Size(1))
                    {
                        break;
                    }

                    if (board.Get(x, y) == stone)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                    }

                    if (count == 5)
                    {
                        isFiveInRow = true;
                    }
                    else if (count > 5)
                    {
                        return false;
                    }
                }
                return isFiveInRow;
            }
        }
    }

}
