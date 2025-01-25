using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScheduler : MonoBehaviour
{
    public static ConveyorScheduler Instance;

    [SerializeField] private float timeStep;
    
    private readonly List<Conveyor> _conveyors = new();
    private float _timer;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ConveyorScheduler.");
        
        Instance = this;
    }

    public void RegisterConveyor(Conveyor conveyor)
    {
        _conveyors.Add(conveyor);
    }
    
    public void RemoveConveyor(Conveyor conveyor)
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