using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    [SerializeField] private BattleSettingsSO[] _battleSettings;
    [SerializeField] private string _battleSceneName = "Battle";

    public void StartDefaultBattle()
    {
        StartBattle(_battleSettings[Random.Range(0, _battleSettings.Length)]);
    } 

    // Multiple menu buttons can call this same method and supply different assets.
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
