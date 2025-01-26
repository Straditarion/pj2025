using System;
using Unity.VisualScripting;
using UnityEngine;

public class Ammunition : MonoBehaviour, ISchedulable
{
    [Serializable]
    public class Special
    {
        public Enemy Enemy;
        public float Damage;
    }
    
    [SerializeField] 
    public float Damage;
    [SerializeField] 
    public Special[] SpecialDamage;
    [SerializeField] 
    public float Speed;
    [SerializeField] 
    public float Radius;
    [SerializeField] 
    public float Range;
    [SerializeField] 
    public Resource Resource;

    private float _dstTravelled;
    
    private void Start()
    {
        EnemyScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        EnemyScheduler.Instance.Remove(this);
    }

    public void ExecuteStep(float deltaTime)
    {
        var pointA = (Vector2)transform.position;
        
        transform.position += transform.right * Speed * deltaTime;
        
        _dstTravelled += Speed * deltaTime;

        if (_dstTravelled > Range)
        {
            Destroy(gameObject);
            return;
        }
        
        var pointB = (Vector2)transform.position;

        var delta = (pointB - pointA);
        var edge = delta.magnitude;
        delta /= edge;
        
        foreach (var enemy in Enemy.Enemies)
        {
            var edelta = (Vector2)enemy.transform.position - pointA;
            var t = Vector2.Dot(delta, edelta) / edge;
            t = Mathf.Clamp01(t);
            
            var pointC = Vector2.Lerp(pointA, pointB, t);
            
            var dstSq = ((Vector2)enemy.transform.position - pointC).sqrMagnitude;

            var r = Radius + enemy.Radius;
            
            if (dstSq < r * r)
            {
                var damage = Damage;

                foreach (var special in SpecialDamage)
                {
                    if (enemy.gameObject.name.StartsWith(special.Enemy.gameObject.name))
                    {
                        damage = special.Damage;
                        break;
                    }
                }
                
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
    }
}
