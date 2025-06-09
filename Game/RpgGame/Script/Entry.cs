using RpgGame;
using RpgGame.Managers;
using RpgGame.Structure;
using System.Linq;

internal class Entry()
{
    internal static void Main(string[] args)
    {
        Debug.LogDebug("Debug log message! - These are to be deleted");
        Debug.LogVerbose("Verbose log message! - Dev info user dont care about");
        Debug.LogInfo("Info log message! - Info to the user");
        Debug.LogWarning("Warning log message! - For warnings");
        Debug.LogError("Error log message! - For errors... :)");

        EventManager.Invoke(EventManagerTypes.PreHook, null);
        StartAllHooks();
        new Engine().Run();
    }

    internal static void StartAllHooks()
    {
        var hooks = Utils.GetAllTypes<IHookable>();
        foreach (var hook in hooks)
        {
            string err;
            if (!hook.CanHook(hooks, out err))
            {
                Debug.LogError(err);
                continue;
            }

            CheckHookType(hook);
            hook.Run();
        }

        EventManager.Invoke(EventManagerTypes.PostHook, null);
    }

    internal static void CheckHookType(IHookable hook)
    {
        if(hook is RenderManager)
        {
            Engine.renderer = (RenderManager)hook;
            return;
        }
    }
}

