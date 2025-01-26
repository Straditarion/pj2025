using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Turret : Building
{
    [SerializeField]
    private Ammunition[] _ammunition;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _spread;
    [SerializeField]
    private float _cooldown;
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private AudioClip _shootShound;
    [SerializeField]
    private float _shootSoundVolume;

    private Ammunition _loadedAmmunition;
    private float _timer;
    
    protected override void OnInit()
    {
        BuildingScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        BuildingScheduler.Instance.Remove(this);
    }

    public override bool CanTakeItem(Resource item)
    {
        var can = false;

        foreach (var ammunition in _ammunition)
            can = can || (item.gameObject.name.StartsWith(ammunition.Resource.gameObject.name));
        
        return can && _loadedAmmunition == null;
    }

    public override void TakeItem(Resource item)
    {
        Destroy(item.gameObject);

        foreach (var ammunition in _ammunition)
        {
            if (!item.gameObject.name.StartsWith(ammunition.Resource.gameObject.name))
                continue;

            _loadedAmmunition = ammunition;
            break;
        }
    }
    
    public override void ExecuteStep(float deltaTime)
    {
        if (_timer <= 0f)
        {
            if (_loadedAmmunition != null)
            {
                Enemy closestEnemy = null;
                var closestEnemyDistanceSq = float.MaxValue;
                
                foreach (var enemy in Enemy.Enemies)
                {
                    var distanceSq = ((Vector2)transform.position - (Vector2)enemy.transform.position).sqrMagnitude;

                    if (distanceSq < closestEnemyDistanceSq && distanceSq < _range * _range)
                    {
                        closestEnemy = enemy;
                        closestEnemyDistanceSq = distanceSq;
                    }
                }

                if (closestEnemy != null)
                {
                    SoundPlayer.Instance.Play(_shootShound, _shootSoundVolume);
                    
                    _head.transform.eulerAngles += Vector3.forward * Vector2.SignedAngle(_head.transform.right,  (closestEnemy.Predict(_head.transform.position, _loadedAmmunition) - (Vector2)_head.transform.position).normalized);

                    _head.transform.eulerAngles += Vector3.forward * Random.Range(-_spread, _spread);

                    var ammunitionInstance = Instantiate(_loadedAmmunition, _head.transform.position, _head.transform.rotation);

                    _loadedAmmunition = null;
                
                    _timer = _cooldown;
                }
            }
            
            return;
        }
        
        _timer -= deltaTime;
    }
}