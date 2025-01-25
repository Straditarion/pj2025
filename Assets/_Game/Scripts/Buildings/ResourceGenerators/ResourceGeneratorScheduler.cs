using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneratorScheduler : MonoBehaviour
{
    public static ResourceGeneratorScheduler Instance;

    [SerializeField] private float timeStep;
    
    private readonly List<ResourceGenerator> _conveyors = new();
    private float _timer;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ConveyorScheduler.");
        
        Instance = this;
    }

    public void RegisterConveyor(ResourceGenerator conveyor)
    {
        _conveyors.Add(conveyor);
    }
    
    public void RemoveConveyor(ResourceGenerator conveyor)
    {
        _conveyors.Remove(conveyor);
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if ( _timer < timeStep )
            return;
        
        _timer -= timeStep;

        foreach (var conveyor in _conveyors)
        {
            conveyor.ExecuteStep(timeStep);
        }
    }
}