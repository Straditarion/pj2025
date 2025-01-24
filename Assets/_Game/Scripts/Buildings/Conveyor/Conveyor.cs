using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    [Header("Conveyor Settings")]
    [SerializeField]
    private float _conveyorSpeed;

    public List<Transform> Content { get; private set; }

    protected override void OnInit()
    {
        Content = new List<Transform>();
        ConveyorScheduler.Instance.RegisterConveyor(this);
    }

    public override void ExecuteStep(float deltaTime)
    {
        
    }
}