﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScheduler : Scheduler
{
    public static EnemyScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of ResourceGeneratorScheduler.");
        
        Instance = this;
    }
}