using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Smelter : Building
{
    [Serializable]
    public class Recipe
    {
        public Resource Input;
        public Resource Output; 
        public float Duration;
    }
    
    [Header("Generation Settings")] 
    [SerializeField]
    private Resource _fuel;
    [SerializeField]
    private Recipe[] _recipes;
    [SerializeField]
    private SpriteAnimation _animation;

    private Resource _currentResource;
    private bool _needsFuel;
    private float _timer;
    
    protected override void OnInit()
    {
        _needsFuel = true;
        
        BuildingScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        BuildingScheduler.Instance.Remove(this);
    }

    public override bool CanTakeItem(Resource item)
    {
        var can = item.gameObject.name.StartsWith(_fuel.gameObject.name) && _needsFuel;

        foreach (var recipe in _recipes)
            can = can || (item.gameObject.name.StartsWith(recipe.Input.gameObject.name) && !_needsFuel);
        
        return can && _currentResource == null && _timer <= 0f;
    }

    public override void TakeItem(Resource item)
    {
        Destroy(item.gameObject);

        if (item.gameObject.name.StartsWith(_fuel.gameObject.name))
            _needsFuel = false;

        foreach (var recipe in _recipes)
        {
            if (!item.gameObject.name.StartsWith(recipe.Input.gameObject.name))
                continue;

            _currentResource = recipe.Output;
            _timer = recipe.Duration;
            _animation.enabled = true;
            break;
        }
    }

    public override void ExecuteStep(float deltaTime)
    {
        if (_timer <= 0f)
        {
            if (_currentResource != null)
            {
                var (_, output) = GetIOConveyors();

                foreach (var conveyor in output)
                {
                    if (conveyor.CanTakeItem(_currentResource))
                    {
                        conveyor.TakeItem(Instantiate(_currentResource, conveyor.transform.position, Quaternion.identity));
                        Debug.Log("F");
                        _currentResource = null;
                        _needsFuel = true;
                        break;
                    }
                }
            }
            
            _animation.enabled = false;
            return;
        }

        _timer -= deltaTime;
    }
}