using System;
using ThatOneGame.Structure;

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
            Debug.LogDebug("Invoking event " + type);

            switch (type)
            {
                case EventManagerTypes.WindowSizeChanged:
                    OnWindowSizeChanged?.Invoke((EventArgs)args);
                    break;
                
            }
        }
    }
}
