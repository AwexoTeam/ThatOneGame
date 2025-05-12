using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class TestScreen : TiledScreen
    {
        public override void Start()
        {
            AddGameObject(new Player());
            AddGameObject(new BattleUI());
            
            base.Start();
        }
    }
}
