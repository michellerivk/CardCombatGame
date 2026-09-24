public enum AttackOutcome
{
    NoEffect,
    DirectDamage,
    Prevented,
    DefenderDamaged,
    DefenderDefeated
}

public static class CombatResolver
{
    public static AttackOutcome ResolveAttack(
        Card attacker,
        Card defender,
        HealthPool opposingHealth)
    {
        if (attacker == null || attacker.IsDefeated || opposingHealth == null)
            return AttackOutcome.NoEffect;

        attacker.NotifyAttack();

        var context = new AttackContext(attacker, defender, opposingHealth);

        ApplyOutgoingAbilities(attacker, context);

        if (context.IsCancelled)
            return AttackOutcome.Prevented;

        // A zero-attack card does not count as a hit and does not consume a shield.
        if (context.Damage <= 0)
            return AttackOutcome.NoEffect;

        if (context.TargetsOpponent || defender == null || defender.IsDefeated)
        {
            opposingHealth.TakeDamage(context.Damage);
            return AttackOutcome.DirectDamage;
        }

        ApplyIncomingAbilities(defender, context);

        if (context.IsCancelled || context.Damage <= 0)
            return AttackOutcome.Prevented;

        int damage = context.IsLethal ? defender.CurrentHealth : context.Damage;
        defender.DamageCard(damage);
        return defender.IsDefeated
            ? AttackOutcome.DefenderDefeated
            : AttackOutcome.DefenderDamaged;
    }

    private static void ApplyOutgoingAbilities(Card attacker, AttackContext context)
    {
        foreach (CardAbility ability in attacker.Abilities)
        {
            if (ability is IModifyOutgoingAttack modifier)
                modifier.ModifyOutgoingAttack(context);

            if (context.IsCancelled)
                return;
        }
    }

    private static void ApplyIncomingAbilities(Card defender, AttackContext context)
    {
        foreach (CardAbility ability in defender.Abilities)
        {
            if (ability is IModifyIncomingAttack modifier)
                modifier.ModifyIncomingAttack(context);

            if (context.IsCancelled)
                return;
        }
    }
}
