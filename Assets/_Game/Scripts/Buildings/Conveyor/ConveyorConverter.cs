using UnityEngine;

public class ConveyorConverter : MonoBehaviour
{
    private void Start()
    {
        GridManager.Instance.Buildings.OnObjectAdded += ConveyorAdded;
    }

    private void ConveyorAdded(Vector2Int position, Building building)
    {
        if(building is not Conveyor conveyor) 
            return;

        ConvertAddedForward(position, conveyor);
        ConveyorAddedSelf(position, conveyor);
    }

    private void ConvertAddedForward(Vector2Int position, Conveyor conveyor)
    {
        position = position + conveyor.Forward;
        if (!GridManager.Instance.Buildings.TryGetValue(position, out var nextBuilding)) 
            return;

        if(nextBuilding is not Conveyor nextConveyor)
            return;

        if (nextConveyor.IsTurn)
            return;

        var left = conveyor.Rotation == 0 ? 3 : conveyor.Rotation - 1;
        var right = ( conveyor.Rotation + 1 ) % 4;
        if (nextConveyor.Rotation != left && nextConveyor.Rotation != right)
            return;

        if (GridManager.Instance.Buildings.TryGetValue(position + nextConveyor.Back, out var other))
        {
            if(nextConveyor.Rotation == other.Rotation)
                return;
        }
        
        ReplaceWithTurn(position, nextConveyor, nextConveyor.Rotation == right);
    }

    private void ConveyorAddedSelf(Vector2Int position, Conveyor conveyor)
    {
        var forward = GridManager.Instance.Buildings.TryGetValue(position + conveyor.Forward, out var buildingForward);
        var right = GridManager.Instance.Buildings.TryGetValue(position + conveyor.Right, out var buildingRight);
        var back = GridManager.Instance.Buildings.TryGetValue(position + conveyor.Back, out var buildingBack);
        var left = GridManager.Instance.Buildings.TryGetValue(position + conveyor.Left, out var buildingLeft);
        
        var isLeftGood = left && buildingLeft is Conveyor && buildingLeft.Forward == conveyor.Right;
        var isRightGood = right && buildingRight is Conveyor && buildingRight.Forward == conveyor.Left;
        
        if (isLeftGood && isRightGood)
            return;
        
        if (!isLeftGood && !isRightGood)
            return;
        
        if ( forward && back && buildingBack is Conveyor && buildingForward is Conveyor && 
             buildingBack.Forward == conveyor.Forward && buildingForward.Forward == conveyor.Forward )
            return;
        
        if ( back && buildingBack is Conveyor && buildingBack.Forward == conveyor.Forward )
            return;
        
        ReplaceWithTurn(position, conveyor, isRightGood);
    }

    private void ReplaceWithTurn(Vector2Int position, Conveyor replacedConveyor, bool mirror)
    {
        var newBuilding = Instantiate(replacedConveyor.ConveyorAlternativePrefab, (Vector3Int)position, Quaternion.identity);
        newBuilding.transform.SetParent(GridManager.Instance.BuildingsParent);
        newBuilding.Initialize(position, replacedConveyor.Rotation);
        newBuilding.SpriteRenderer.flipY = mirror;
        Destroy(replacedConveyor.gameObject);
        GridManager.Instance.Buildings.Replace(position, newBuilding);
    }
}