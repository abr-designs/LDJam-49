using System;
using UnityEngine;

public class ShipCore : MonoBehaviour
{
    public static Action OnDied;
    [SerializeField]
    private SpriteRenderer coreSpriteRenderer;
    
    [SerializeField, Header("Colors")]
    private Color stableColor = Color.white;
    [SerializeField]
    private Color volatileColor= Color.white;

    [SerializeField, Header("Health")]
    private float startingHealth;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = startingHealth;
        
        coreSpriteRenderer.color = stableColor;
    }

    public void DamageCore(in float damage)
    {
        _currentHealth -= Mathf.Abs(damage);

        coreSpriteRenderer.color = Color.Lerp(volatileColor, stableColor, _currentHealth / startingHealth);

        if (_currentHealth > 0f)
            return;

        OnDied?.Invoke();
        
    }
    
}
