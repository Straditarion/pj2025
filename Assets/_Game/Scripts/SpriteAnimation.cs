using System;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour, ISchedulable
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private int _framesPerSecond;
    
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    private int _currentIndex = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[_currentIndex];
        
        AnimationScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        AnimationScheduler.Instance.Remove(this);
    }
    
    public void ExecuteStep(float deltaTime)
    {
        // _timer += deltaTime;
        // if(_timer < 1f / _framesPerSecond)
        //     return;
        // _timer -= 1f / _framesPerSecond;

        _currentIndex = (_currentIndex + 1) % _sprites.Length;
        _spriteRenderer.sprite = _sprites[_currentIndex];
    }
}