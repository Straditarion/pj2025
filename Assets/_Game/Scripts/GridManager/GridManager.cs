using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public Transform BuildingsParent;
    public Grid<Building> Buildings { get; private set; }

    public Transform ObstaclesParent;
    public Grid<GameObject> Obstacles { get; private set; }
    
    public Transform ResourcesParent;
    public Grid<Resource> Resources { get; private set; }
    
    public Transform EnvironmentParent;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("Multiple instances of GridManager");

        Buildings = new();
        Obstacles = new();
        Resources = new();
        
        Instance = this;
    }
}

public class Grid<TObject>
{
    public Action OnValueChanged { get; set; }
    public Action<Vector2Int, TObject> OnObjectAdded { get; set; }
    public Action<Vector2Int, TObject> OnObjectRemoved { get; set; }

    private Dictionary<Vector2Int, TObject> _values = new();

    public TObject Get(Vector2Int key)
    {
        return _values.TryGetValue(key, out var value) ? value : default(TObject);
    }
    
    public void Add(Vector2Int key, TObject value)
    {
        _values.Add(key, value);
        OnObjectAdded?.Invoke(key, value);
        OnValueChanged?.Invoke();
    }
    
    public void Remove(Vector2Int key)
    {
        if (!_values.Remove(key, out var value)) 
            return;
        OnObjectRemoved?.Invoke(key, value);
        OnValueChanged?.Invoke();
    }
    
    public bool Contains(Vector2Int key)
    {
        return _values.ContainsKey(key);
    }

    public bool TryGetValue(Vector2Int key, out TObject value)
    {
        return _values.TryGetValue(key, out value);
    }
}
