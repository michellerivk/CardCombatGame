using TMPro;
using UnityEngine;

public class UIDamageIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _lifeTime;

    private RectTransform myRect;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);

        myRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        myRect.anchoredPosition += new Vector2(0f, -_moveSpeed * Time.deltaTime);
    }

    public void Initialize(int damage)
    {
        _damageText.text = $"-{damage}";
    }
    
}
