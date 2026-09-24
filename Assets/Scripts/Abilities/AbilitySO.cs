using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;

    [TextArea]
    public string abilityDescription;

    public Sprite icon;

    [SerializeField, Tooltip("Lower values run first within the same combat phase.")]
    private int _priority;

    public int Priority => _priority;

    public abstract CardAbility CreateRuntimeAbility();
}
