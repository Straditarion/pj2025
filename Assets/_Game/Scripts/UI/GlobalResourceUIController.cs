using UnityEngine;

public class GlobalResourceUIController : MonoBehaviour
{
    [SerializeField] private GameObject _resourceLabelPrefab;

    private void Awake()
    {
        Chest.OnGlobalResourceAmountChanged += OnGlobalResourceAmountChanged;
    }
    
    private void OnGlobalResourceAmountChanged()
    {
        var chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
        GlobalInventoryState.Instance.ResourceStash.Clear();
        
        foreach (var chest in chests)
        {
            foreach (var resource in chest.Content)
            {
                GlobalInventoryState.Instance.ResourceStash.TryAdd(resource.Name, new ResourceStash
                {
                    Resource = resource,
                    Amount = 0,
                });

                GlobalInventoryState.Instance.ResourceStash[resource.Name].Amount += 1;
            }
        }
        
        foreach (var resource in GlobalInventoryState.Instance.VirtualChest)
        {
            GlobalInventoryState.Instance.ResourceStash.TryAdd(resource.Name, new ResourceStash
            {
                Resource = resource,
                Amount = 0,
            });

            GlobalInventoryState.Instance.ResourceStash[resource.Name].Amount += 1;
        }
        
        //update UI
    }
}
