using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class MenuScreen : GameScreen
    {
        public override void Start()
        {
            base.Start();
            ScreenManager.instance.LoadScreen(new TestScreen());

        }


    }
}
