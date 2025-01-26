using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : Building
{
    private List<Resource> _content = new();

    private int _step;
    
    protected override void OnInit()
    {
        BuildingScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        BuildingScheduler.Instance.Remove(this);

        foreach (var resource in _content)
        {
            Destroy(resource.gameObject);
        }
    }
    
    public override bool CanTakeItem(Resource item) => true;

    public override void TakeItem(Resource item)
    {
        _content.Add(item);
        item.transform.position = Vector2.one * 7777;
    }

    public override void ExecuteStep(float deltaTime)
    {
        if(_content.Count == 0)
            return;
        
        var (_, outputs) = GetIOConveyors();
        var availableOutputs = outputs.Where(x => x.CanTakeItem(null)).ToList();

        var output = availableOutputs[_step % availableOutputs.Count];
        
        foreach (var resource in _content)
        {
            resource.transform.position = output.transform.position;
            output.TakeItem(resource);
            _content.Remove(resource);
            break;
        }

        _step = (_step + 1) % (int.MaxValue - 1);
    }
}