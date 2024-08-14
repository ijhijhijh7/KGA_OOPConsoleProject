using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.Scenes
{
    public enum SceneType
    {
        Opening,
        GameSelection,
        Omok,
        Othello,
        Blackjack,
        Result,
        Size
    }

    public abstract class Scene
    {
        public abstract void Load();
        public abstract void Unload();
        public abstract void Render();
        public abstract void HandleInput(Game game);
        public abstract void Update(Game game);

    }
}
