using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.Scenes
{
    public class OpeningScene : Scene
    {
        public override void Load()
        {
            
        }

        public override void Unload()
        {
            
        }

        public override void Render()
        {
            Console.WriteLine("미니게임 모음집에 오신걸 환영합니다!");
            Console.WriteLine("시작을 원하시면 엔터를 누르세요...");
        }

        public override void HandleInput(Game game)
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                game.ChangeScene(SceneType.GameSelection);
            }
        }

        public override void Update(Game game)
        {
            // 필요 시 업데이트 로직 추가
        }
    }
}
