using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgGame.Structure;

namespace RpgGame.Managers
{
    public enum EventManagerTypes
    {
        PreHook,
        PostHook,

        PreInit,
        PostInit,

        EngineInitizile,
        EngineContentLoad,
        EngineStart,

        Scenequeued,
        SceneLoaded,
        SceneChanged,
        SceneUnloaded,

        WindowSizeChanged,
    };

    public static class EventManager
    {
        #region delegates
        public delegate void _PreHook();
        public delegate void _PostHook();

        public delegate void _PreInit();
        public delegate void _PostInit();

        public delegate void _EngineInitizile();
        public delegate void _EngineContentLoad();
        public delegate void _EngineStart();

        public delegate void _Scenequeued(int id);
        public delegate void _SceneLoaded(int id);
        public delegate void _SceneChanged(int newId, int oldId);
        public delegate void _SceneUnloaded(int id);

        public delegate void _WindowSizeChanged(EventArgs e);
        #endregion

        #region events
        public static event _PreHook OnPreHook;
        public static event _PostHook OnPostHook;
        public static event _PreInit OnPreInit;
        public static event _PostInit OnPostInit;

        public static event _EngineInitizile OnEngineInitizile;
        public static event _EngineContentLoad OnEngineContentLoad;
        public static event _EngineStart OnEngineStart;

        public static event _Scenequeued OnScenequeued;
        public static event _SceneLoaded OnSceneLoaded;
        public static event _SceneChanged OnSceneChanged;
        public static event _SceneUnloaded OnSceneUnloaded;

        public static event _WindowSizeChanged OnWindowSizeChanged;
        #endregion

        public static void Invoke(EventManagerTypes type, params object[] args)
        {
            Debug.LogDebug("Invoking event " + type);

            switch (type)
            {
                case EventManagerTypes.PreHook:
                    OnPreHook?.Invoke();
                    break;
                case EventManagerTypes.PostHook:
                    OnPostHook?.Invoke();
                    break;
                case EventManagerTypes.PreInit:
                    OnPreInit?.Invoke();
                    break;
                case EventManagerTypes.PostInit:
                    OnPostInit?.Invoke();
                    break;
                case EventManagerTypes.EngineInitizile:
                    OnEngineInitizile?.Invoke();
                    break;
                case EventManagerTypes.EngineContentLoad:
                    OnEngineContentLoad?.Invoke();
                    break;
                case EventManagerTypes.EngineStart:
                    OnEngineStart?.Invoke();
                    break;

                case EventManagerTypes.Scenequeued:
                    OnScenequeued?.Invoke((int)args[0]);
                    break;
                case EventManagerTypes.SceneLoaded:
                    OnSceneLoaded?.Invoke((int)args[0]);
                    break;
                case EventManagerTypes.SceneChanged:
                    OnSceneChanged?.Invoke((int)args[0], (int)args[1]);
                    break;
                case EventManagerTypes.SceneUnloaded:
                    OnSceneUnloaded?.Invoke((int)args[0]);
                    break;

                case EventManagerTypes.WindowSizeChanged:
                    OnWindowSizeChanged?.Invoke((EventArgs)args[0]);
                    break;

            }
        }
    }
}
