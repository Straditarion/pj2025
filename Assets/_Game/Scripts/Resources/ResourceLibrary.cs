using System.Collections.Generic;
using UnityEngine;

public class ResourceLibrary : MonoBehaviour
{
    public static ResourceLibrary Instance;

    public Dictionary<string, Resource> Items { get; set; }

    [SerializeField] private List<Resource> _items;

    private void Awake()
    {
        Instance = this;
        Items = new Dictionary<string, Resource>();
        
        foreach (var item in _items)
        {
            Items.Add(item.Name, item);
        }
    }
}