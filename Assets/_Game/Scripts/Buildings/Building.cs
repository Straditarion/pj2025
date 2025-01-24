using UnityEngine;
using UnityEngine.Serialization;

public abstract class Building : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    public Vector2Int Size;
    
    protected SpriteRenderer _spriteRenderer;
    protected int _rotation;
    
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    
    public abstract void ExecuteStep(float deltaTime);

    public void Initialize(int rotation)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        _rotation = rotation;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, _rotation * -90);
        
        OnInit();
    }

    public void AsGhost(int rotation)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        _rotation = rotation;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, _rotation * -90);
    }
    
    protected virtual void OnInit()
    {
        
    }
}