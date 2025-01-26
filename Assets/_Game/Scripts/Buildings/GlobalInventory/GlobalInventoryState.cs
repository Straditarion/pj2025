using System.Collections.Generic;
using UnityEngine;

public class GlobalInventoryState : MonoBehaviour
{
    public static GlobalInventoryState Instance;
    public Dictionary<string, ResourceStash> VirtualChest { get; private set; }
    public Dictionary<string, ResourceStash> ResourceStash { get; private set; }

    [SerializeField] private List<ResourceStash> _startResources = new();
    
    private void Awake()
    {
        VirtualChest = new ();
        ResourceStash = new();
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
            if (!ResourceStash.TryGetValue(stash.Resource.Name, out var value))
                return false;

            if (value.Amount < stash.Amount)
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

            var delta = Mathf.Min(virtualStash.Amount, stash.Amount);
            VirtualChest[stash.Resource.Name].Amount -= delta;
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
            var toRemoveFromChest = new List<Resource>();
            foreach (var resource in chest.Content)
            {
                foreach (var stash in listCopy)
                {
                    if (stash.Resource.Name != resource.Name)
                        continue;
                    
                    toRemoveFromChest.Add(resource);
                    stash.Amount--;

                    if (stash.Amount <= 0)
                        listCopy.Remove(stash);
                    
                    break;
                }
                
                if(listCopy.Count == 0)
                    break;
            }
            
            toRemoveFromChest.ForEach( x => chest.RemoveItem(x, false) );
            
            if(listCopy.Count == 0)
                break;
        }
        
        Chest.OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public void AddResources(List<ResourceStash> resourceList)
    {
        foreach (var stash in resourceList)
        {
            stash.Resource.transform.position = Vector2.one * 7777;
            VirtualChest.TryAdd(stash.Resource.Name, new ResourceStash
            {
                Resource = stash.Resource,
                Amount = 0,
            });

            VirtualChest[stash.Resource.Name].Amount += stash.Amount;
        }
        
        Chest.OnGlobalResourceAmountChanged?.Invoke();
    }
    
    public void AddResource(Resource resource, bool triggerEvent = true)
    {
        resource.transform.position = Vector2.one * 7777;
        VirtualChest.TryAdd(resource.Name, new ResourceStash
        {
            Resource = resource,
            Amount = 0,
        });

        VirtualChest[resource.Name].Amount++;
        
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