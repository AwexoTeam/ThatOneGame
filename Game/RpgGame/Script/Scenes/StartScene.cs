using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using RpgGame.Components;
using RpgGame.Managers;
using RpgGame.Script.Components;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Scenes
{
    public class StartScene : Scene
    {
        Map map;

        public StartScene()
        {
        
        }

        public override void Start()
        {
            map = TiledManager.GetMap("");
            if (map == null)
                Debug.LogError("Could not locate start scene");

            GameObject mapObj = new GameObject();
            mapObj.AddComponent(map);

            GameObject player = new GameObject();
            Player playerComp = new Player();

            Renderer render = new Renderer(null, 80, 0, 0);
            BoxCollider hitBox = new BoxCollider(0, 0, 16, 16);
            BoxCollider collisonBox = new BoxCollider(0, 8, 16, 8);
            MainPlayerUI ui = new MainPlayerUI();

            playerComp.collisionBox = collisonBox;
            playerComp.hitBox = hitBox;

            player.AddComponent(playerComp);
            player.AddComponent(render);
            player.AddComponent(hitBox);
            player.AddComponent(collisonBox);
            player.AddComponent(ui);

            Instantiate(mapObj);
            Instantiate(player);

            base.Start();

        }
    }
}
