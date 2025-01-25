using System;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour, ISchedulable
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private int _framesPerSecond;
    
    private SpriteRenderer _spriteRenderer;

    private int SpriteIndex => Mathf.RoundToInt(Time.time * _framesPerSecond) % _sprites.Length;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[SpriteIndex];
        
        AnimationScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        AnimationScheduler.Instance.Remove(this);
    }
    
    public void ExecuteStep(float deltaTime)
    {
        if (this.enabled)
            _spriteRenderer.sprite = _sprites[SpriteIndex];
    }
}