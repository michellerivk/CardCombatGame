using System;

public abstract class CardAbility
{
    public AbilitySO Definition { get; }
    public Card Owner { get; private set; }
    public int Priority => Definition.Priority;

    protected CardAbility(AbilitySO definition)
    {
        Definition = definition != null
            ? definition
            : throw new ArgumentNullException(nameof(definition));
    }

    internal bool TryAttachTo(Card owner)
    {
        if (owner == null || Owner != null)
            return false;

        Owner = owner;
        OnAttached();
        return true;
    }

    internal void DetachFrom(Card owner)
    {
        if (Owner != owner)
            return;

        OnDetached();
        Owner = null;
    }

    protected void NotifyStateChanged()
    {
        Owner?.NotifyAbilityStateChanged();
    }

    protected virtual void OnAttached() { }
    protected virtual void OnDetached() { }
}
