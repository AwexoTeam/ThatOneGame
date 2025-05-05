using MapEditor.Scripts.Definations.Enums;
using MapEditor.Scripts.Definations.Structure;
using MapEditor.Scripts.Scenes.TestSceneObj;
using MapEditor.Scripts.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.Scenes
{
    public class TestScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            gameObjects.Add(new MenuPanel());
        }
    }
}
