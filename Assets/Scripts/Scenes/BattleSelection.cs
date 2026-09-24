using UnityEngine;

// Carries only the selected definition between scenes. Runtime battle state
// remains inside the Battle scene's controllers.
public static class BattleSelection
{
    public static BattleSettingsSO Current { get; private set; }

    public static void Select(BattleSettingsSO settings)
    {
        Current = settings;
    }

    public static void Clear()
    {
        Current = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        Current = null;
    }
}
