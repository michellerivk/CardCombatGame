using UnityEngine;
using UnityEngine.UI;

public class BattleThemeView : MonoBehaviour
{
    [SerializeField] private Renderer _deskRenderer;
    [SerializeField] private Image _backgroundImage;

    public void Apply(Material deskMaterial, Sprite background)
    {
        if (_deskRenderer != null && deskMaterial != null)
            _deskRenderer.sharedMaterial = deskMaterial;

        if (_backgroundImage != null && background != null)
            _backgroundImage.sprite = background;
    }
}
