using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scheduler : MonoBehaviour
{
    [SerializeField] private float timeStep;
    
    private readonly List<ISchedulable> _values = new();
    private float _timer;

    public void Register(ISchedulable value)
    {
        _values.Add(value);
    }
    
    public void Remove(ISchedulable value)
    {
        _values.Remove(value);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if ( _timer < timeStep )
            return;
        
        _timer -= timeStep;
        
        foreach (var value in _values)
        {
            value.ExecuteStep(timeStep);
        }
    }

    private void OnValidate()
    {
        transform.name = GetType().Name;
    }
}