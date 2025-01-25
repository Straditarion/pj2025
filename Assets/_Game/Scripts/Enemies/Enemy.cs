using System;
using UnityEngine;

public class Enemy : MonoBehaviour, ISchedulable
{
    [SerializeField] 
    public float Speed;
    [SerializeField] 
    public FastNoiseExtension Noise;
    [SerializeField] 
    public float NoiseFrequency;
    [SerializeField] 
    public float NoiseIntensity;

    protected FastNoise _noise;
    
    private void Start()
    {
        EnemyScheduler.Instance.Register(this);
    }

    protected virtual void OnInit() { }

    private void OnDestroy()
    {
        EnemyScheduler.Instance.Remove(this);
    }
    
    public virtual void ExecuteStep(float deltaTime)
    {
        
    }

    protected void AdvanceTowards(Vector2 position)
    {
        
    }
}
