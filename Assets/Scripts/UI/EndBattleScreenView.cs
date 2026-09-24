using TMPro;
using UnityEngine;

public class EndBattleScreenView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _screenVisual;
    [SerializeField] private Animator _anim;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private BattleController _battleController;

    void Awake()
    {
        // This GameObject must stay active so it remains subscribed to the battle event.
        //SetVisible(false);
    }

    void OnEnable()
    {
        _battleController.OnBattleEnded += EnableEndBattleScreen;

        // Also handles enabling this view after the battle already ended.
        if (_battleController.Result.HasValue)
            EnableEndBattleScreen(_battleController.Result.Value);
    }

    void OnDisable()
    {
        _battleController.OnBattleEnded -= EnableEndBattleScreen;
    }

    private void EnableEndBattleScreen(BattleResult result)
    {
        string newText = result == BattleResult.Defeat ? "Lost" : "Won";

        _resultText.text = "You " + newText + "!";

        _anim.SetTrigger("Appear");

        //SetVisible(true);
    }

    // private void SetVisible(bool visible)
    // {
    //     _screenVisual.alpha = visible ? 1f : 0f;
    //     _screenVisual.interactable = visible;
    //     _screenVisual.blocksRaycasts = visible;
    // }

    public void MainMenuButton()
    {
    }
    public void PlayAgainButton()
    {
    }
    public void NewGameButton()
    {
    }
}
