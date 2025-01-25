using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scheduler<TBuilding> : MonoBehaviour where TBuilding : ISchedulable
{
    [SerializeField] private float timeStep;
    
    private readonly List<TBuilding> _values = new();
    private float _timer;

    public void Register(TBuilding value)
    {
        _values.Add(value);
    }
    
    public void Remove(TBuilding value)
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