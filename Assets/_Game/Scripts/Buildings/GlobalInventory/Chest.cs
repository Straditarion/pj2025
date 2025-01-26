using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : Building
{
    public static Action OnGlobalResourceAmountChanged { get; set; }

    public List<Resource> Content => _content;
    
    private List<Resource> _content = new();
    private int _step;
    
    protected override void OnInit()
    {
        BuildingScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        BuildingScheduler.Instance.Remove(this);

        if(_content is null)
            return;

        foreach (var resource in _content)
        {
            GlobalInventoryState.Instance.VirtualChest.TryAdd(resource.Name, new ResourceStash
            {
                Resource = resource,
                Amount = 0,
            });

            GlobalInventoryState.Instance.VirtualChest[resource.Name].Amount++;
            
            Destroy(resource.gameObject);
        }
        
        _content.Clear();
        OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public override bool CanTakeItem(Resource item) => true;

    public override void TakeItem(Resource item)
    {
        AddItem(item);
        item.transform.position = Vector2.one * 7777;
    }

    public override void ExecuteStep(float deltaTime)
    {
        if(_content.Count == 0)
            return;
        
        var (_, outputs) = GetIOConveyors();
        var availableOutputs = outputs.Where(x => x.CanTakeItem(null)).ToList();

        if(availableOutputs.Count == 0)
            return;
        
        var output = availableOutputs[_step % availableOutputs.Count];
        
        foreach (var resource in _content)
        {
            resource.transform.position = output.transform.position;
            output.TakeItem(resource);
            RemoveItem(resource);
            break;
        }

        _step = (_step + 1) % (int.MaxValue - 1);
    }

    public void AddItem(Resource item, bool triggerEvent = true)
    {
        _content.Add(item);
        
        if(triggerEvent)
            OnGlobalResourceAmountChanged?.Invoke();
    }

    public void RemoveItem(Resource item, bool triggerEvent = true)
    {
        _content.Remove(item);
        
        if(triggerEvent)
            OnGlobalResourceAmountChanged?.Invoke();
    }
}