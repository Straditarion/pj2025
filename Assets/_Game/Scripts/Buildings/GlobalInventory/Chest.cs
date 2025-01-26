using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : Building
{
    public static Action OnGlobalResourceAmountChanged { get; set; }

    public IReadOnlyDictionary<string, int> Content => _content;
    
    private Dictionary<string, int> _content = new();
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

        foreach (var kvp in _content)
        {
            GlobalInventoryState.Instance.AddResource(kvp.Key, kvp.Value, false);
        }
        
        OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public override bool CanTakeItem(Resource item) => true;

    public override void TakeItem(Resource item)
    {
        AddItem(item.Name);
        Destroy(item.gameObject);
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
        
        foreach (var kvp in _content)
        {
            var newResource = Instantiate(ResourceLibrary.Instance.Items[kvp.Key]);
            newResource.transform.position = output.transform.position;
            output.TakeItem(newResource);
            RemoveItem(kvp.Key);
            break;
        }

        _step = (_step + 1) % (int.MaxValue - 1);
    }

    public void AddItem(string itemName, bool triggerEvent = true)
    {
        _content.TryAdd(itemName, 0);
        _content[itemName]++;
        
        if(triggerEvent)
            OnGlobalResourceAmountChanged?.Invoke();
    }

    public void RemoveItem(string itemName, bool triggerEvent = true)
    {
        if(!_content.TryGetValue(itemName, out var value))
            return;
        
        if(value == 0)
            return;

        _content[itemName]--;
        
        if(triggerEvent)
            OnGlobalResourceAmountChanged?.Invoke();
    }
}