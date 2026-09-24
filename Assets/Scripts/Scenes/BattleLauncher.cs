using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    [SerializeField] private BattleCatalogSO _battleCatalog;
    [SerializeField] private string _battleSceneName = "Battle";

    public void StartRandomBattle()
    {
        if (_battleCatalog == null)
        {
            Debug.LogError("BattleLauncher needs a BattleCatalogSO.", this);
            return;
        }

        BattleSettingsSO randomBattle = _battleCatalog.GetRandom(BattleSelection.Current);

        if (randomBattle == null)
        {
            Debug.LogError("The battle catalog contains no battles.", this);
            return;
        }

        StartBattle(randomBattle);
    } 

    public void StartBattle(BattleSettingsSO settings)
    {
        if (settings == null)
        {
            Debug.LogError("BattleLauncher needs a BattleSettingsSO.", this);
            return;
        }

        if (SceneTransitionController.Instance == null)
        {
            Debug.LogError("Starting a battle needs a SceneTransitionController.", this);
            return;
        }

        BattleSelection.Select(settings);
        SceneTransitionController.Instance.LoadScene(_battleSceneName);
    }
}
