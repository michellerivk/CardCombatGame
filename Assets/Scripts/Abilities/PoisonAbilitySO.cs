using UnityEngine;

[CreateAssetMenu(fileName = "Poison Ability", menuName = "Cards/Abilities/Poison", order = 2)]
public sealed class PoisonAbilitySO : AbilitySO
{
    public override CardAbility CreateRuntimeAbility()
    {
        return new PoisonAbility(this);
    }
}

public sealed class PoisonAbility : CardAbility, IModifyOutgoingAttack
{
    public PoisonAbility(PoisonAbilitySO definition) : base(definition) { }

    public void ModifyOutgoingAttack(AttackContext context)
    {
        // Lethal only matters when the resolver ultimately hits a card.
        context.IsLethal = true;
    }
}
