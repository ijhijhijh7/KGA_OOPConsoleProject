using MiniGames.Scenes;
using MiniGames.Scenes.MiniGames.Scenes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames
{
    public class Game
    {
        private bool isRunning;
        private Scene[] scenes;
        private Scene currentScene;

        public void Run()
        {
            Start();
            while (isRunning)
            {
                Render();
                Input();
                Update();
            }
            End();
        }

        public void Over()
        {
            isRunning = false;
        }

        private void Start()
        {
            isRunning = true;
            scenes = new Scene[(int)SceneType.Size];

            scenes[(int)SceneType.Opening] = new OpeningScene();
            scenes[(int)SceneType.GameSelection] = new GameSelectionScene();
            scenes[(int)SceneType.Omok] = new OmokScene();
            scenes[(int)SceneType.Othello] = new OthelloScene();
            /*scenes[(int)SceneType.Blackjack] = new BlackjackScene();
            scenes[(int)SceneType.Result] = new ResultScene();*/

            currentScene = scenes[(int)SceneType.Opening];
            currentScene.Load();
        }
        private void Render()
        {
            Console.Clear();
            currentScene.Render();
        }

        private void Input()
        {
            currentScene.HandleInput(this);
        }

        private void Update()
        {
            currentScene.Update(this);
        }

        private void End()
        {
            currentScene.Unload();
            Console.WriteLine("게임이 종료되었습니다.");
        }

        public void ChangeScene(SceneType newScene)
        {
            currentScene.Unload();
            currentScene = scenes[(int)newScene];
            currentScene.Load(); 
        }
    }
}
