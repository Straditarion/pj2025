using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSystem : PlayerSystem
{
    private int _rotation;
    private Building _buildingPrefab;
    private Building _ghost;
    
    public void Rotate()
    {
        _rotation = (_rotation + 1) % 4;
        
        var ghostPosition = _ghost == null ? Vector3.zero : _ghost.transform.position;
        SpawnGhost(_rotation, ghostPosition);
    }

    public void ChangePrefab(Building prefab)
    {
        _buildingPrefab = prefab;
        
        if(_ghost != null) 
            Destroy(_ghost.gameObject);
    }
    
    public void Place(Vector2Int position)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (_buildingPrefab == null)
            return;

        if(!GlobalInventoryState.Instance.HasResources(_buildingPrefab.Cost))
            return;
        
        if (!CanPlaceBuildingOnPosition(position))
            return;

        var objPosition = CalculateObjectPosition(position);
        var newBuilding = Instantiate(_buildingPrefab, objPosition, Quaternion.identity);
        newBuilding.transform.SetParent(GridManager.Instance.BuildingsParent);
        newBuilding.Initialize(position, _rotation);
        
        GlobalInventoryState.Instance.RemoveResources(newBuilding.Cost);
        
        for (var x = 0; x < newBuilding.Size.x; x++)
        {
            for (var y = 0; y < newBuilding.Size.y; y++)
            {
                GridManager.Instance.Buildings.Add(position + new Vector2Int(x, y), newBuilding);
            }
        }
    }

    public void Remove(Vector2Int position)
    {
        if (!GridManager.Instance.Buildings.TryGetValue(position, out var building))
            return;
        
        var gameObjectPosition = building.transform.position;
        var size = (Vector2)building.Size / 2f;
        var leftCorner = new Vector3( gameObjectPosition.x - size.x, gameObjectPosition.y - size.y, 0 ) + new Vector3(0.5f, 0.5f, 0);
        var leftCornerInt = new Vector2Int(Mathf.RoundToInt(leftCorner.x), Mathf.RoundToInt(leftCorner.y));
        
        for (var x = 0; x < building.Size.x; x++)
        {
            for (var y = 0; y < building.Size.y; y++)
            {
                GridManager.Instance.Buildings.Remove(leftCornerInt + new Vector2Int(x, y));
            }
        }
        
        GlobalInventoryState.Instance.AddResources(building.Cost);
        building.OnDeconstruct();
        Destroy(building.gameObject);
    }

    public void UpdateGhostPosition(Vector2Int position)
    {
        if (_buildingPrefab == null)
            return;
        
        var objPosition = CalculateObjectPosition(position);
        
        if (_ghost == null)
            SpawnGhost(_rotation, objPosition);
        
        _ghost.transform.position = objPosition;
        _ghost.SpriteRenderer.color = GetGhostColor(position);
    }

    private Vector3 CalculateObjectPosition(Vector2Int position)
    {
        var size = (Vector2)_buildingPrefab.Size / 2f;

        if (_rotation % 2 == 1)
            size = new Vector2(size.y, size.x);
        
        var leftCorner = new Vector3( position.x + size.x, position.y + size.y, 0 );
        return leftCorner - new Vector3(0.5f, 0.5f, 0);
    }
    
    private bool CanPlaceBuildingOnPosition(Vector2Int position)
    {
        if (!Player.Instance.GetSystem<BubbleManager>().IsWithinBubble(CalculateObjectPosition(position), Mathf.Max(_buildingPrefab.Size.x, _buildingPrefab.Size.y)))
            return false;

        var size = (Vector2)_buildingPrefab.Size;
        
        if (_rotation % 2 == 1)
            size = new Vector2(size.y, size.x);
        
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var testingPosition = position + new Vector2Int(x, y);
                if (GridManager.Instance.Buildings.Contains(testingPosition) 
                    || GridManager.Instance.Obstacles.Contains(testingPosition))
                    return false;
            }
        }
        
        return GlobalInventoryState.Instance.HasResources(_buildingPrefab.Cost);
    }
    
    private void SpawnGhost(int rotation, Vector3 position)
    {
        if (_ghost != null)
            Destroy(_ghost.gameObject);
        
        if (_buildingPrefab == null)
            return;
        
        var pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        var newGhost = Instantiate(_buildingPrefab, position, Quaternion.identity);
        newGhost.AsGhost(pos, rotation);
        newGhost.SpriteRenderer.color = GetGhostColor(pos);
        newGhost.SpriteRenderer.sortingOrder = 24;
        
        _ghost = newGhost;
    }

    private Color GetGhostColor(Vector2Int position)
    {
        return CanPlaceBuildingOnPosition(position) ? new Color(1f, 1f, 1f, .5f) : new Color(1f, 0f, 0f, .5f);
    }
}