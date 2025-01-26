using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] 
    public Vector2 SpawnDelay;
    [SerializeField] 
    public Vector2 SpawnAmount;
    [SerializeField] 
    public float SpawnDelayMul;
    [SerializeField] 
    public Vector2 SpawnAmountAdd;
    [SerializeField] 
    public float SpawnOffset;
    [SerializeField] 
    public bool SkipInit;
    [SerializeField] 
    public GameObject Enemy;

    protected float _timer;

    private void Start()
    {
        if (SkipInit)
            return;
        
        _timer = Random.Range(SpawnDelay.x, SpawnDelay.y);

        SpawnDelay *= SpawnDelayMul;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            var amount = Mathf.CeilToInt(Random.Range(SpawnAmount.x, SpawnAmount.y));

            SpawnAmount += SpawnAmountAdd;
            
            _timer = Random.Range(SpawnDelay.x, SpawnDelay.y);

            SpawnDelay *= SpawnDelayMul;
            
            var basePoint = Random.insideUnitCircle.normalized * (Player.Instance.GetSystem<BubbleManager>().Radius + 100f);

            for (int i = 0; i < amount; i++)
            {
                var subPoint = basePoint + (Random.insideUnitCircle * Mathf.Sqrt(amount) * SpawnOffset);
                
                Instantiate(Enemy, basePoint + subPoint, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            }
        }
    }
}
