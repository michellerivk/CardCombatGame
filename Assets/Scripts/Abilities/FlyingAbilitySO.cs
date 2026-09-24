using UnityEngine;

[CreateAssetMenu(fileName = "Flying Ability", menuName = "Cards/Abilities/Flying", order = 1)]
public sealed class FlyingAbilitySO : AbilitySO
{
    public override CardAbility CreateRuntimeAbility()
    {
        return new FlyingAbility(this);
    }
}

public sealed class FlyingAbility : CardAbility, IModifyOutgoingAttack
{
    public FlyingAbility(FlyingAbilitySO definition) : base(definition) { }

    public void ModifyOutgoingAttack(AttackContext context)
    {
        context.TargetsOpponent = true;
    }
}
