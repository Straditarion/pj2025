using UnityEngine;

public class Conveyor : Building
{
    [Header("Conveyor Settings")]
    [SerializeField]
    private float _conveyorSpeed;
    
    protected override void OnInit()
    {
        ConveyorScheduler.Instance.RegisterConveyor(this);
    }

    public override void ExecuteStep(float deltaTime)
    {
        
    }
}