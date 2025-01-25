using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class AirFactory : Building
{
    [Header("Generation Settings")] 
    [SerializeField]
    private Resource _fuel;
    [SerializeField]
    private float _production;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private SpriteAnimation _animation;

    private float _timer;
    
    protected override void OnInit()
    {
        AirFactoryScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        AirFactoryScheduler.Instance.Remove(this);
    }

    public override bool CanTakeItem(Resource item) => item.gameObject.name.StartsWith(_fuel.gameObject.name) && _timer <= 0f;

    public override void TakeItem(Resource item)
    {
        Destroy(item.gameObject);
        
        _timer = _duration;
        _animation.enabled = true;
    }
    
    public override void ExecuteStep(float deltaTime)
    {
        if (_timer <= 0f)
        {
            _animation.enabled = false;
            return;
        }

        Player.Instance.GetSystem<BubbleManager>().Volume += _production * deltaTime;

        _timer -= deltaTime;
    }
}