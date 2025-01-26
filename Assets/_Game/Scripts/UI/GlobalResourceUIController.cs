using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
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
        GlobalInventoryState.Instance.Resources.Clear();
        
        foreach (var chest in chests)
        {
            foreach (var kvp in chest.Content)
            {
                GlobalInventoryState.Instance.Resources.TryAdd(kvp.Key, 0);
                GlobalInventoryState.Instance.Resources[kvp.Key] += kvp.Value;
            }
        }
        
        foreach (var kvp in GlobalInventoryState.Instance.VirtualChest)
        {
            GlobalInventoryState.Instance.Resources.TryAdd(kvp.Key, 0);
            GlobalInventoryState.Instance.Resources[kvp.Key] += kvp.Value;
        }

        var resources = GlobalInventoryState.Instance.Resources.ToList();
        resources.Sort((a, b) => 
            ResourceLibrary.Instance.Items[a.Key].OrderIndex < ResourceLibrary.Instance.Items[b.Key].OrderIndex ? 1 : -1);
        
        _labels.ForEach( Destroy );

        foreach (var resource in resources)
        {
            var instance = ResourceLibrary.Instance.Items[resource.Key];
            var newLabel = Instantiate(_resourceLabelPrefab, transform);
            newLabel.GetComponentsInChildren<Image>()[1].sprite = instance.Sprite;
            newLabel.GetComponentInChildren<TMP_Text>().text = resource.Value.ToString();
            _labels.Add(newLabel);
        }
    }
}
