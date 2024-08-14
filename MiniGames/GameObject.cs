using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames
{
    public abstract class GameObject
    {
        public ConsoleColor color { get; set; }
        public Point pos { get; set; }
        public char simbol { get; set; }

        //public abstract void Interaction(Game game, Player player);
    }


}
