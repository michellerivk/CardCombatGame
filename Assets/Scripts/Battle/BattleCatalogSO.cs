using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Battle Catalog")]
public class BattleCatalogSO : ScriptableObject
{
    [SerializeField] private List<BattleSettingsSO> _battles = new();

    public BattleSettingsSO GetRandom(BattleSettingsSO excluded = null)
    {
        List<BattleSettingsSO> candidates = _battles.FindAll(
            battle => battle != null && battle != excluded);

        // If there is only one battle, allow selecting it again.
        if (candidates.Count == 0)
        {
            candidates = _battles.FindAll(battle => battle != null);
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(0, candidates.Count)];
    }
}