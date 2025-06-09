using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using RpgGame.Managers;
using RpgGame.Script.Manager;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Scenes
{
    public class MenuScene : Scene
    {
        GraphicsDevice graphics => Engine.graphics.GraphicsDevice;
        
        public override void Start()
        {
            var playButton = new Button();
            playButton.Text = "Play";
            playButton.Visual.Width = 300;

            int startScene = SceneManager.instance.GetScreenId<StartScene>();
            playButton.Click += (s,e) => { SceneManager.instance.LoadScreen(startScene); }; ;
            playButton.Anchor(Anchor.Center);

            GUI.AddElement(playButton);
        }
    }
}
