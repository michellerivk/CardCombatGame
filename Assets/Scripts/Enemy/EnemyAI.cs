using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum AIType{ placeFromDeck, handRandomPlace, handDefensive, handAttacking }
    [SerializeField] private AIType _enemyAIType;

    public event Action<AIType> OnAIChanged;

    private void DecideAI()
    {
        switch (_enemyAIType)
        {
            case AIType.placeFromDeck:
            // Probably won't use - too simple and unfair
                break;

            case AIType.handRandomPlace:
                break;

            case AIType.handDefensive:
                break;

            case AIType.handAttacking:
                break;
        }

        OnAIChanged?.Invoke(_enemyAIType);
    }
}
