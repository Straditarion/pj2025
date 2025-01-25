using UnityEngine;

public class Conveyor : Building
{
    [Header("Conveyor Settings")]
    [SerializeField]
    private float _conveyorSpeed;
    
    public bool IsTurn;
    
    [SerializeField]
    public Conveyor ConveyorAlternativePrefab;

    public Resource Item { get; set; }

    protected override void OnInit()
    {
        ConveyorScheduler.Instance.RegisterConveyor(this);
    }

    private void OnDestroy()
    {
        ConveyorScheduler.Instance.RemoveConveyor(this);
    }
    
    public override bool CanTakeItem(Resource item) => Item == null;
    public override void TakeItem(Resource item) => Item = item;

    public override void ExecuteStep(float deltaTime)
    {
        if(Item == null)
            return;

        var distanceToCenter = Vector2.Distance(Item.transform.position, transform.position);

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
        var travelDirection = (transform.position - Item.transform.position).normalized;
        Item.transform.position += travelDirection * travelDistance;
    }
}