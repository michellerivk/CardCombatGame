public sealed class AttackContext
{
    public Card Attacker { get; }
    public Card Defender { get; }
    public HealthPool OpposingHealth { get; }

    public int Damage { get; set; }
    public bool TargetsOpponent { get; set; }
    public bool IsLethal { get; set; }
    public bool IsCancelled { get; set; }

    public AttackContext(Card attacker, Card defender, HealthPool opposingHealth)
    {
        Attacker = attacker;
        Defender = defender;
        OpposingHealth = opposingHealth;
        Damage = attacker != null ? attacker.AttackPower : 0;
        TargetsOpponent = defender == null || defender.IsDefeated;
    }
}
