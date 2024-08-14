using System;
using System.Collections.Generic;
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

        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("    < 오델로 >");
            Console.Write(Blackturn ? "흑돌 차례" : "백돌 차례");
            Console.WriteLine();
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
                        Blackturn = !Blackturn;
                        // 여기에 돌 뒤집기 및 승리 조건 검사 로직 추가 필요
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
            // 기본적인 유효한 이동 검사 로직을 추가합니다.
            return true; // 예시로 항상 true를 반환합니다.
        }

        public override void Update(Game game) { }
    }
}
