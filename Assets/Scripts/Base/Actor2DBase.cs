using UnityEngine;

public abstract class Actor2DBase : MonoBehaviour
{
    private new Transform transform;
    private SpriteRenderer _spriteRenderer;

    //====================================================================================================================//
    
    protected virtual void Start()
    {
        transform = gameObject.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //====================================================================================================================//
    
    public void SetColor(in Color color)
    {
        _spriteRenderer.color = color;
    }
    public void SetSpriteOrder(in int orderInLayer)
    {
        _spriteRenderer.sortingOrder = orderInLayer;
    }

    public int GetSortingOrder() => _spriteRenderer.sortingOrder;
    public Bounds GetSpriteBounds() => _spriteRenderer.bounds;
}
