using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.Scenes
{
    public class OthelloScene : Scene
    {
        private Board board;
        private int curX, curY;
        private bool Blackturn;

        private static readonly (int dx, int dy)[] Directions =
        {
            (0, 1), (1, 0), (0, -1), (-1, 0), // 수평 & 수직
            (1, 1), (1, -1), (-1, 1), (-1, -1) // 대각선
        };

        public override void Load()
        {
            board = new Board(8);
            InitializeBoard();
            curX = 0;
            curY = 0;
            Blackturn = true;
        }

        public override void Unload()
        {

        }

        public void InitializeBoard()
        {
            int size = board.Size(0);
            int mid = size / 2;

            // 중앙에 초기 돌 배치
            board.Set(mid - 1, mid - 1, 2);
            board.Set(mid, mid, 2);
            board.Set(mid - 1, mid, 1);
            board.Set(mid, mid - 1, 1);
        }

        private void CountStones(out int blackCount, out int whiteCount)
        {
            blackCount = 0;
            whiteCount = 0;

            for (int x = 0; x < board.Size(0); x++)
            {
                for (int y = 0; y < board.Size(1); y++)
                {
                    if (board.Get(x, y) == 1)
                    {
                        blackCount++;
                    }
                    else if (board.Get(x, y) == 2)
                    {
                        whiteCount++;
                    }
                }
            }
        }


        public override void Render()
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine("    < 오델로 >");
            Console.WriteLine(Blackturn ? "흑돌 차례" : "백돌 차례");

            int blackCount, whiteCount;
            CountStones(out blackCount, out whiteCount);
            Console.WriteLine($"흑돌: {blackCount}개, 백돌: {whiteCount}개");

            string boardText = "";
            for (int i = 0; i < board.Size(0); i++)
            {
                for (int j = 0; j < board.Size(1); j++)
                {
                    if (i == curY && j == curX) boardText += "▣ ";
                    else if (board.Get(i, j) == 0) boardText += "□ ";
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

            if (!HasValidMoves())
            {
                Console.WriteLine("착수할 수 있는 수가 없습니다. 턴을 넘깁니다.");
                Thread.Sleep(1000);
                Blackturn = !Blackturn;
                Console.WriteLine(Blackturn ? "흑돌 차례" : "백돌 차례");
                return;
            }

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
                    if (board.IsEmpty(curY, curX) && IsValidMove(curY, curX))
                    {
                        board.Set(curY, curX, Blackturn ? 1 : 2);
                        FlipStone(curY, curX);
                        Blackturn = !Blackturn;
                    }
                    else
                    {
                        Console.WriteLine("유효하지 않은 착수입니다. 다시 시도하세요.");
                        Thread.Sleep(1000);
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

        private bool IsValidMove(int x, int y)
        {
            int player = Blackturn ? 1 : 2;
            return board.IsEmpty(x, y) && Check(x, y, player);
        }

        private void FlipStone(int x, int y)
        {
            int player = Blackturn ? 1 : 2;
            int opponent = Blackturn ? 2 : 1;

            foreach (var (dx, dy) in Directions)
            {
                List<(int, int)> toFlip = new List<(int, int)>();
                int nx = x + dx;
                int ny = y + dy;

                while (nx >= 0 && ny >= 0 && nx < board.Size(0) && ny < board.Size(1))
                {
                    if (board.Get(nx, ny) == opponent)
                    {
                        toFlip.Add((nx, ny));
                    }
                    else if (board.Get(nx, ny) == player)
                    {
                        foreach (var (fx, fy) in toFlip)
                        {
                            board.Set(fx, fy, player);
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }

                    nx += dx;
                    ny += dy;
                }
            }
        }

        private bool HasValidMoves()
        {
            int player = Blackturn ? 1 : 2;

            for (int x = 0; x < board.Size(0); x++)
            {
                for (int y = 0; y < board.Size(1); y++)
                {
                    if (board.IsEmpty(x, y) && Check(x, y, player))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        

        private bool Check(int x, int y, int player)
        {
            foreach (var (dx, dy) in Directions)
            {
                if (CheckDirection(x, y, player, dx, dy))
                {
                    return true;
                }
            }
            return false;
        }


        private bool CheckDirection(int x, int y, int player, int deltaX, int deltaY)
        {
            int opponent = player == 1 ? 2 : 1; // 상대방 돌
            int nx = x + deltaX;
            int ny = y + deltaY;
            bool hasOpponentBetween = false;

            // 보드를 벗어나지 않고 탐색
            while (nx >= 0 && ny >= 0 && nx < board.Size(0) && ny < board.Size(1))
            {
                if (board.Get(nx, ny) == opponent)
                {
                    hasOpponentBetween = true; // 사이에 상대방 돌이 있음
                }
                else if (board.Get(nx, ny) == player)
                {
                    return hasOpponentBetween; // 중간에 상대방 돌이 있고, 끝에 본인 돌이 있으면 착수 가능
                }
                else
                {
                    break; // 빈 칸을 만나면 탐색 중지
                }

                nx += deltaX;
                ny += deltaY;
            }

            return false; // 착수 불가능
        }

        private bool IsBoardFull()
        {
            for (int x = 0; x < board.Size(0); x++)
            {
                for (int y = 0; y < board.Size(1); y++)
                {
                    if (board.IsEmpty(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void Update(Game game)
        {
            int blackCount, whiteCount;
            CountStones(out blackCount, out whiteCount);

            // 보드가 가득 찼거나 양쪽 플레이어가 둘 곳이 없는 경우
            if (IsBoardFull() || (!HasValidMoves() && !CanOpponentMove()))
            {
                Console.Clear();
                Console.WriteLine("게임 종료");
                Console.WriteLine($"흑돌: {blackCount}개, 백돌: {whiteCount}개");

                if (blackCount > whiteCount)
                {
                    Console.WriteLine("흑돌 승리!");
                }
                else if (whiteCount > blackCount)
                {
                    Console.WriteLine("백돌 승리!");
                }
                else
                {
                    Console.WriteLine("무승부!");
                }

                Console.WriteLine("아무 키나 눌러 게임선택창으로 돌아가기");
                Console.ReadKey();
                game.ChangeScene(SceneType.GameSelection);
            }
        }

        private bool CanOpponentMove()
        {
            Blackturn = !Blackturn;
            bool canMove = HasValidMoves();
            Blackturn = !Blackturn;
            return canMove;
        }
    }
}