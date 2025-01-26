using UnityEngine;

public class Conveyor : Building
{
    [Header("Conveyor Settings")]
    [SerializeField]
    private float _conveyorSpeed;
    
    public bool IsTurn;
    public bool JunctionSender;
    public bool JunctionReceiver;
    
    [SerializeField]
    public Conveyor ConveyorAlternativePrefab;

    public Resource Item { get; set; }

    protected override void OnInit()
    {
        ConveyorScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        ConveyorScheduler.Instance.Remove(this);
        
        if(Item == null)
            return;
        
        GlobalInventoryState.Instance.AddResource(Item);
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
            var sender = JunctionSender ? Forward : Vector2Int.zero;
            
            var forward = GridManager.Instance.Buildings.TryGetValue(GridPosition + Forward + sender, out var other);
            if (!forward || !other.CanTakeItem(Item)) 
                return;
            
            if(JunctionSender && (other is not Conveyor otherConveyor || !otherConveyor.JunctionReceiver))
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