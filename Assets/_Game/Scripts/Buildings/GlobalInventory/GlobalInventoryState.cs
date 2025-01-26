using System.Collections.Generic;
using UnityEngine;

public class GlobalInventoryState : MonoBehaviour
{
    public static GlobalInventoryState Instance;
    public Dictionary<string, int> VirtualChest { get; private set; }
    public Dictionary<string, int> Resources { get; private set; }

    [SerializeField] private List<ResourceStash> _startResources = new();
    
    private void Awake()
    {
        VirtualChest = new ();
        Resources = new();
        Instance = this;
    }

    private void Start()
    {
        AddResources(_startResources);
    }

    public bool HasResources(List<ResourceStash> resourceList)
    {
        foreach (var stash in resourceList) 
        {
            if (!Resources.TryGetValue(stash.Resource.Name, out var value))
                return false;

            if (value < stash.Amount)
                return false;
        }
        return true;
    }
    
    public void RemoveResources(List<ResourceStash> resourceList)
    {
        var listCopy = new List<ResourceStash>(resourceList);

        var toRemove = new List<ResourceStash>();
        foreach (var stash in listCopy)
        {
            if(!VirtualChest.TryGetValue(stash.Resource.Name, out var virtualStash))
                continue;

            var delta = Mathf.Min(virtualStash, stash.Amount);
            VirtualChest[stash.Resource.Name] -= delta;
            stash.Amount = delta;
            
            if(stash.Amount <= 0)
                toRemove.Add(stash);
        }
        
        toRemove.ForEach( x => listCopy.Remove(x) );
        toRemove.Clear();

        if (listCopy.Count == 0)
        {
            Chest.OnGlobalResourceAmountChanged?.Invoke();
            return;
        }
        
        var chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);

        foreach (var chest in chests)
        {
            foreach (var chestKvp in chest.Content)
            {
                foreach (var stash in listCopy)
                {
                    if (stash.Resource.Name != chestKvp.Key)
                        continue;
                    
                    var delta = Mathf.Min(chestKvp.Value, stash.Amount);
                    stash.Amount -= delta;

                    if (stash.Amount <= 0)
                        listCopy.Remove(stash);
                    
                    break;
                }
                
                if(listCopy.Count == 0)
                    break;
            }
            
            if(listCopy.Count == 0)
                break;
        }
        
        Chest.OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public void AddResources(List<ResourceStash> resourceList)
    {
        foreach (var stash in resourceList)
        {
            VirtualChest.TryAdd(stash.Resource.Name, 0);
            VirtualChest[stash.Resource.Name] += stash.Amount;
        }
        
        Chest.OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public void AddResource(string name, bool triggerEvent = true)
    {
        AddResource(name, 1, triggerEvent);
    }
    
    public void AddResource(string name, int count, bool triggerEvent = true)
    {
        VirtualChest.TryAdd(name, 0);
        VirtualChest[name] += count;
        
        if(triggerEvent)
            Chest.OnGlobalResourceAmountChanged?.Invoke();
    }
}

[System.Serializable]
public class ResourceStash
{
    public Resource Resource;
    public int Amount;
}