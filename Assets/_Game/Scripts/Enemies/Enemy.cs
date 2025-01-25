using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, ISchedulable
{
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

    protected FastNoise _noise;

    protected float _cooldown;
    
    private void Start()
    {
        _noise = Noise.GetLibInstance(GetInstanceID());
        
        EnemyScheduler.Instance.Register(this);
        
        OnInit();
    }

    protected virtual void OnInit() { }

    private void OnDestroy()
    {
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
}
