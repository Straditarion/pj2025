using System;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class BuildingScheduler : Scheduler<Building>
=======
public class AirFactoryScheduler : Scheduler
>>>>>>> f79c205a3f4d81ecf9eab2f50a2eb43fab203ed2
{
    public static BuildingScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ResourceGeneratorScheduler.");
        
        Instance = this;
    }
}