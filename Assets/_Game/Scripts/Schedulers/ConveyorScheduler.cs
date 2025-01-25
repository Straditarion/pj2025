using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScheduler : Scheduler<Conveyor>
{
    public static ConveyorScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ConveyorScheduler.");
        
        Instance = this;
    }
}