using System;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    protected Vector2Int _size;
    
    protected SpriteRenderer _spriteRenderer;
    protected int _rotation;
    
    public abstract void ExecuteStep(float deltaTime);

    public void Initialize(int rotation)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _rotation = rotation;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, _rotation * 90);
        
        OnInit();
    }

    protected virtual void OnInit()
    {
        
    }
}