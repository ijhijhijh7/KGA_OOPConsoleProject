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
            Console.WriteLine("Select a game to play:");
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
                    game.ChangeScene(SceneType.Blackjack);
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
