using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (_card == null)
            _card = GetComponent<Card>();

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();

        if (_card == null || _animator == null)
        {
            Debug.LogError("CardAnimator needs a Card and an Animator.", this);

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_card == null || _animator == null)
            return;

        _card.OnAttack += AnimateAttack;
        _card.OnDamage += AnimateHurt;
    }

    private void OnDisable()
    {
        if (_card == null)
            return;

        _card.OnAttack -= AnimateAttack;
        _card.OnDamage -= AnimateHurt;
    }

    private void AnimateAttack()
    {
        _animator.SetTrigger("Attack");
    }

    private void AnimateHurt()
    {
        _animator.SetTrigger("Hurt");
    }

    public void AnimateJump()
    {
        _animator.SetTrigger("Jump");
    }
}