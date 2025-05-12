using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOneGame.GameCode
{
    public enum EventManagerTypes
    {
        WindowSizeChanged,
    };

    public static class EventManager
    {
        public delegate void _WindowSizeChanged(EventArgs e);

        public static event _WindowSizeChanged OnWindowSizeChanged;

        public static void Invoke(EventManagerTypes type, object args)
        {
            switch (type)
            {
                case EventManagerTypes.WindowSizeChanged:
                    OnWindowSizeChanged?.Invoke((EventArgs)args);
                    break;
                
            }
        }
    }
}
