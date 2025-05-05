using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.Definations.Structure
{
    public class Scene
    {
        public string name;
        public List<GameObject> gameObjects;

        public Scene()
        {
            gameObjects = new List<GameObject>();
        }

        public Scene(string name)
        {
            this.name = name;
            gameObjects = new List<GameObject>();
        }

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent(Game game)
        {

        }
    }
}
