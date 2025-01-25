using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TObject>
{
    public Action<Vector2Int, TObject> OnValueChanged { get; set; }
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
        OnValueChanged?.Invoke(key, value);
    }
    
    public void Remove(Vector2Int key)
    {
        if (!_values.Remove(key, out var value)) 
            return;
        OnObjectRemoved?.Invoke(key, value);
        OnValueChanged?.Invoke(key, value);
    }

    public void Replace(Vector2Int key, TObject value)
    {
        if(!_values.ContainsKey(key))
            return;
        _values[key] = value;
        OnValueChanged?.Invoke(key, value);
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