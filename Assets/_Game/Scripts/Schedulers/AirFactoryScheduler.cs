using System;
using System.Collections.Generic;
using UnityEngine;

public class AirFactoryScheduler : Scheduler<AirFactory>
{
    public static AirFactoryScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ResourceGeneratorScheduler.");
        
        Instance = this;
    }
}