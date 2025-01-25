using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScheduler : Scheduler<Building>
{
    public static BuildingScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ResourceGeneratorScheduler.");
        
        Instance = this;
    }
}