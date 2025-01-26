using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public Sprite[] Sprites;
    public float TimePerFrame;
    public AudioClip ExplosionSound;
    public float ExplosionSoundVolume;

    private float _timer;

    private void Start()
    {
        _timer = Sprites.Length * TimePerFrame;
        
        SoundPlayer.Instance.Play(ExplosionSound, ExplosionSoundVolume);
    }

    private void Update()
    {
        Sprite.sprite = Sprites[Mathf.Clamp(Mathf.FloorToInt(_timer / TimePerFrame), 0, Sprites.Length - 1)];
        
        if(_timer <= 0f)
            Destroy(gameObject);
        
        _timer -= Time.deltaTime;
    }
}
