using UnityEngine;

[CreateAssetMenu(fileName = "Shield Ability", menuName = "Cards/Abilities/Shield", order = 3)]
public sealed class ShieldAbilitySO : AbilitySO
{
    [SerializeField, Min(1)] private int _blockedHits = 1;

    public int BlockedHits => _blockedHits;

    public override CardAbility CreateRuntimeAbility()
    {
        return new ShieldAbility(this);
    }
}

public sealed class ShieldAbility : CardAbility, IModifyIncomingAttack
{
    public int RemainingBlocks { get; private set; }

    public ShieldAbility(ShieldAbilitySO definition) : base(definition)
    {
        RemainingBlocks = definition.BlockedHits;
    }

    public void ModifyIncomingAttack(AttackContext context)
    {
        if (RemainingBlocks <= 0 || context.IsCancelled)
            return;

        RemainingBlocks--;
        context.IsCancelled = true;
        NotifyStateChanged();
    }
}
