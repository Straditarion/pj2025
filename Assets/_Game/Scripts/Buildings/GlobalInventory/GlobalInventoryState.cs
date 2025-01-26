using System.Collections.Generic;
using UnityEngine;

public class GlobalInventoryState : MonoBehaviour
{
    public static GlobalInventoryState Instance;
    public List<Resource> VirtualChest { get; private set; }
    public Dictionary<string, ResourceStash> ResourceStash { get; private set; }
    
    private void Awake()
    {
        VirtualChest = new ();
        ResourceStash = new();
        Instance = this;
    }
}

public class ResourceStash
{
    public Resource Resource;
    public int Amount;
}