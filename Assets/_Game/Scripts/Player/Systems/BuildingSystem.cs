using UnityEngine;

public class BuildingSystem : PlayerSystem
{
    private int _rotation;
    private Building _buildingPrefab;
    private Building _ghost;
    
    public Building _conveyorPrefab;
    
    public void Rotate()
    {
        _rotation = (_rotation + 1) % 4;
        
        var ghostPosition = _ghost == null ? Vector3.zero : _ghost.transform.position;
        SpawnGhost(_rotation, ghostPosition);
    }

    public void ChangePrefab(Building prefab)
    {
        _buildingPrefab = prefab;
    }
    
    public void Place(Vector2Int position)
    {
        if (_buildingPrefab == null)
            return;
        
        if (!CanPlaceBuildingOnPosition(position))
            return;

        var objPosition = CalculateObjectPosition(position);
        var newBuilding = Instantiate(_buildingPrefab, objPosition, Quaternion.identity);
        newBuilding.transform.SetParent(GridManager.Instance.BuildingsParent);
        
        for (var x = 0; x < _buildingPrefab.Size.x; x++)
        {
            for (var y = 0; y < _buildingPrefab.Size.y; y++)
            {
                GridManager.Instance.Buildings.Add(position + new Vector2Int(x, y), newBuilding);
            }
        }
        
        newBuilding.Initialize(_rotation);
    }

    public void Remove(Vector2Int position)
    {
        if (!GridManager.Instance.Buildings.TryGetValue(position, out var building))
            return;
        
        var gameObjectPosition = building.transform.position;
        var size = (Vector2)building.Size / 2f;
        var leftCorner = new Vector3( gameObjectPosition.x - size.x, gameObjectPosition.y - size.y, 0 ) + new Vector3(0.5f, 0.5f, 0);
        var leftCornerInt = new Vector2Int(Mathf.RoundToInt(leftCorner.x), Mathf.RoundToInt(leftCorner.y));
        
        Destroy(building.gameObject);
        
        for (var x = 0; x < _buildingPrefab.Size.x; x++)
        {
            for (var y = 0; y < _buildingPrefab.Size.y; y++)
            {
                GridManager.Instance.Buildings.Remove(leftCornerInt + new Vector2Int(x, y));
            }
        }
        
        //return resources
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
        var leftCorner = new Vector3( position.x + size.x, position.y + size.y, 0 );
        return leftCorner - new Vector3(0.5f, 0.5f, 0);
    }
    
    private bool CanPlaceBuildingOnPosition(Vector2Int position)
    {
        for (var x = 0; x < _buildingPrefab.Size.x; x++)
        {
            for (var y = 0; y < _buildingPrefab.Size.y; y++)
            {
                var testingPosition = position + new Vector2Int(x, y);
                if (GridManager.Instance.Buildings.ContainsKey(testingPosition) 
                    || GridManager.Instance.Obstacles.ContainsKey(testingPosition))
                    return false;
            }
        }
        
        //add check if not enough resources
        return true;
    }
    
    private void SpawnGhost(int rotation, Vector3 position)
    {
        if (_ghost != null)
            Destroy(_ghost.gameObject);
        
        if (_buildingPrefab == null)
            return;
        
        var newGhost = Instantiate(_buildingPrefab, position, Quaternion.identity);
        newGhost.AsGhost(rotation);
        
        var pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        newGhost.SpriteRenderer.color = GetGhostColor(pos);
        newGhost.SpriteRenderer.sortingOrder = 20;
        
        _ghost = newGhost;
    }

    private Color GetGhostColor(Vector2Int position)
    {
        return CanPlaceBuildingOnPosition(position) ? new Color(1f, 1f, 1f, .5f) : new Color(1f, 0f, 0f, .5f);
    }
    
    private void Awake()
    {
        //DEBUG
        ChangePrefab(_conveyorPrefab);
    }
}