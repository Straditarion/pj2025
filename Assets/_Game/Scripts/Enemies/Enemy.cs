using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, ISchedulable
{
    public static HashSet<Enemy> Enemies = new HashSet<Enemy>();
    
    [SerializeField] 
    public float Health;
    [SerializeField] 
    public float Radius;
    [SerializeField] 
    public float MoveSpeed;
    [SerializeField] 
    public float TurnSpeed;
    [SerializeField] 
    public float TurnClamp;
    [SerializeField] 
    public FastNoiseExtension Noise;
    [SerializeField] 
    public float NoiseIntensity;
    [SerializeField] 
    public Vector2 CooldownRange;
    [SerializeField] 
    public float BiteDamage;
    [SerializeField] 
    public GameObject Bubble;
    [SerializeField] 
    public Transform HpBar;
    [SerializeField] 
    public AudioClip DeathSound;
    [SerializeField] 
    public float DeathSoundVolume;

    protected FastNoise _noise;

    protected float _cooldown;

    protected float _health;
    
    private int _hashCode;

    public void TakeDamage(float damage)
    {
        _health -= damage * 1.001f;
        
        HpBar.localScale = new Vector3(Mathf.Clamp01(_health / Health), 1f, 1f);

        if (_health <= 0)
        {
            Destroy(gameObject);

            if (DeathSound != null)
                SoundPlayer.Instance.Play(DeathSound, DeathSoundVolume);
        }
    }
    
    private void Start()
    {
        _hashCode = HashCode.Combine(GetInstanceID());
        
        _health = Health;
        
        _noise = Noise.GetLibInstance(GetInstanceID());

        Enemies.Add(this);
        
        EnemyScheduler.Instance.Register(this);
        
        OnInit();
    }

    protected virtual void OnInit() { }

    private void OnDestroy()
    {
        Enemies.Remove(this);

        EnemyScheduler.Instance.Remove(this);
    }
    
    public virtual void ExecuteStep(float deltaTime)
    {
        var bubbleManager = Player.Instance.GetSystem<BubbleManager>();

        if (_cooldown <= 0f && Vector2.Distance(transform.position, bubbleManager.Visuals.position) <= bubbleManager.Radius)
        {
            _cooldown = Random.Range(CooldownRange.x, CooldownRange.y);
            
            bubbleManager.Volume -= BiteDamage;
        }
        
        _cooldown -= deltaTime;
            
        Bubble.SetActive(_cooldown > 0f);
        
        AdvanceTowards(bubbleManager.Visuals.position, deltaTime, _cooldown > 0f);
    }

    protected void AdvanceTowards(Vector2 position, float deltaTime, bool invert)
    {
        var direction = (position - (Vector2)transform.position).normalized;
        
        var noise = _noise.GetNoise(transform.position.x, transform.position.y);
        
        direction = Quaternion.Euler(0f, 0f, noise * NoiseIntensity) * direction;
        
        if (invert)
            direction *= -1f;
        
        var signedAngle = Vector2.SignedAngle(transform.right, direction);
        
        transform.eulerAngles += Vector3.forward * Mathf.Clamp(signedAngle, -TurnClamp, TurnClamp) * TurnSpeed * deltaTime;
        
        transform.position += transform.right * MoveSpeed * deltaTime;
    }

    public virtual Vector2 Predict(Vector2 position, Ammunition ammunition)
    {
        var dst = Vector2.Distance(position, transform.position);
        var t = dst / ammunition.Speed;

        return transform.position + transform.right * MoveSpeed * t;
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}
