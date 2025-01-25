using UnityEngine;

public class Conveyor : Building
{
    [Header("Conveyor Settings")]
    [SerializeField]
    private float _conveyorSpeed;
    
    public bool IsTurn;
    
    [SerializeField]
    public Conveyor ConveyorAlternativePrefab;

    public Transform Item { get; set; }

    protected override void OnInit()
    {
        ConveyorScheduler.Instance.RegisterConveyor(this);
    }

    private void OnDestroy()
    {
        ConveyorScheduler.Instance.RemoveConveyor(this);
    }
    
    public override bool CanTakeItem(Transform item) => Item == null;
    public override void TakeItem(Transform item) => Item = item;

    public override void ExecuteStep(float deltaTime)
    {
        if(Item == null)
            return;

        var distanceToCenter = Vector2.Distance(Item.position, transform.position);

        if (distanceToCenter < 2 * deltaTime)
        {
            var forward = GridManager.Instance.Buildings.TryGetValue(GridPosition + Forward, out var other);
            if (!forward || !other.CanTakeItem(Item)) 
                return;
            
            other.TakeItem(Item);
            Item = null;
            return;
        }

        var travelDistance = Mathf.Min(distanceToCenter, _conveyorSpeed * deltaTime);
        var travelDirection = (transform.position - Item.position).normalized;
        Item.position += travelDirection * travelDistance;
    }
}