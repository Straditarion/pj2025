using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalResourceUIController : MonoBehaviour
{
    [SerializeField] private GameObject _resourceLabelPrefab;

    private List<GameObject> _labels = new();
    
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
        
        foreach (var stash in GlobalInventoryState.Instance.VirtualChest.Values)
        {
            GlobalInventoryState.Instance.ResourceStash.TryAdd(stash.Resource.Name, new ResourceStash
            {
                Resource = stash.Resource,
                Amount = 0,
            });

            GlobalInventoryState.Instance.ResourceStash[stash.Resource.Name].Amount += stash.Amount;
        }

        var resources = GlobalInventoryState.Instance.ResourceStash.Values.ToList();
        resources.Sort((a, b) => a.Resource.OrderIndex > b.Resource.OrderIndex ? 1 : -1);
        
        _labels.ForEach( Destroy );

        foreach (var resource in resources)
        {
            var newLabel = Instantiate(_resourceLabelPrefab, transform);
            newLabel.GetComponentsInChildren<Image>()[1].sprite = resource.Resource.Sprite;
            newLabel.GetComponentInChildren<TMP_Text>().text = resource.Amount.ToString();
            _labels.Add(newLabel);
        }
    }
}
