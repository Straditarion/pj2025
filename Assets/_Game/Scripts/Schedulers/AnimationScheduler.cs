using System;

public class AnimationScheduler : Scheduler
{
    public static AnimationScheduler Instance;
    
    private void Awake()
    {
        if ( Instance != null )
            throw new Exception("Multiple instances of AnimationScheduler.");
        
        Instance = this;
    }
}