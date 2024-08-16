using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.Scenes
{
    public class GameSelectionScene : Scene
    {
        public override void Load()
        {
            
        }

        public override void Unload()
        {
            
        }

        public override void Render()
        {
            Console.CursorVisible = false;
            Console.WriteLine("플레이를 원하는 게임을 선택하세요:");
            Console.WriteLine("1. Omok");
            Console.WriteLine("2. Othello");
            Console.WriteLine("3. Blackjack");
        }

        public override void HandleInput(Game game)
        {
            var input = Console.ReadKey().KeyChar;
            switch (input)
            {
                case '1':
                    game.ChangeScene(SceneType.Omok);
                    break;
                case '2':
                    game.ChangeScene(SceneType.Othello);
                    break;
                case '3':
                    Console.WriteLine(" 미구현 상태입니다.");
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("\nInvalid selection. Please try again.");
                    break;
            }
        }

        public override void Update(Game game)
        {
            // 필요 시 업데이트 로직 추가
        }
    }
}
