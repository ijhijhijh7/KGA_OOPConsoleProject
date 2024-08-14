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
                                Console.WriteLine(currentStone == 1 ? "흑돌 승" : "백돌 승");
                                Console.WriteLine("아무 키나 누르면 선택 씬으로 돌아갑니다.");
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

            public override void Update(Game game) { }

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
            private bool IsForbiddenMove(int x, int y)
            {
                if (board.Get(x, y) != 0)
                {
                    return false; // 위치가 비어있지 않다면 금수가 아님
                }

                int openThreeCount = 0;
                int openFourCount = 0;
                bool isOverline = false;

                // 모든 방향 검사
                int[][] directions = new int[][]
                {
                    new int[] { 1, 0 }, // 수평
                    new int[] { 0, 1 }, // 수직
                    new int[] { 1, 1 }, // 대각선 (\)
                    new int[] { 1, -1 } // 대각선 (/)
                };

                foreach (var direction in directions)
                {
                    int deltaX = direction[0];
                    int deltaY = direction[1];

                    // 열린 3과 열린 4의 개수 세기
                    openThreeCount += CountOpenThree(x, y, deltaX, deltaY);
                    openFourCount += CountOpenFour(x, y, deltaX, deltaY);

                    // 육목 검사
                    if (CheckOverline(x, y, deltaX, deltaY))
                    {
                        isOverline = true;
                    }
                }

                // 금수 조건 검사
                if (openThreeCount > 1 || openFourCount > 1 || isOverline)
                {
                    return true;
                }

                return false;
            }

            private int CountOpenThree(int x, int y, int deltaX, int deltaY)
            {
                int count = 0;

                for (int offset = -4; offset <= 4; offset++)
                {
                    int stoneCount = 0;
                    bool openEnds = false;
                    bool[] ends = { false, false }; // Check if both ends are open

                    for (int i = 0; i < 5; i++)
                    {
                        int newX = x + (offset + i) * deltaX;
                        int newY = y + (offset + i) * deltaY;

                        if (newX < 0 || newY < 0 || newX >= board.Size(0) || newY >= board.Size(1))
                            break;

                        if (board.Get(newX, newY) == 1) // 흑돌
                        {
                            stoneCount++;
                        }
                        else if (board.Get(newX, newY) == 0)
                        {
                            if (i == 0)
                                ends[0] = true; // Left end is open
                            else if (i == 4)
                                ends[1] = true; // Right end is open
                        }
                    }

                    openEnds = ends[0] && ends[1];

                    if (stoneCount == 3 && openEnds)
                        count++;
                }

                return count;
            }

            private int CountOpenFour(int x, int y, int deltaX, int deltaY)
            {
                int count = 0;

                for (int offset = -4; offset <= 4; offset++)
                {
                    int stoneCount = 0;
                    bool openEnd = false;

                    for (int i = 0; i < 5; i++)
                    {
                        int newX = x + (offset + i) * deltaX;
                        int newY = y + (offset + i) * deltaY;

                        if (newX < 0 || newY < 0 || newX >= board.Size(0) || newY >= board.Size(1))
                            break;

                        if (board.Get(newX, newY) == 1) // 흑돌
                        {
                            stoneCount++;
                        }
                        else if (board.Get(newX, newY) == 0)
                        {
                            openEnd = true;
                        }
                    }

                    if (stoneCount == 4 && openEnd)
                        count++;
                }

                return count;
            }

            private bool CheckOverline(int x, int y, int deltaX, int deltaY)
            {
                int stoneCount = 0;

                // 현재 위치에서 양쪽으로 6개 이상 연속된 돌이 있는지 확인
                for (int offset = -5; offset <= 5; offset++)
                {
                    int newX = x + offset * deltaX;
                    int newY = y + offset * deltaY;

                    if (newX < 0 || newY < 0 || newX >= board.Size(0) || newY >= board.Size(1))
                        continue;

                    if (board.Get(newX, newY) == 1) // 흑돌
                    {
                        stoneCount++;
                    }
                    else
                    {
                        stoneCount = 0;
                    }

                    if (stoneCount > 5)
                    {
                        return true; // 육목 금수 조건
                    }
                }

                return false;
            }
        }
    }

}
