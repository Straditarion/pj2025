using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneratorScheduler : Scheduler<ResourceGenerator>
{
    public static ResourceGeneratorScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ResourceGeneratorScheduler.");
        
        Instance = this;
    }
}