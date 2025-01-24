using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Transform BuildingsParent;
    public Dictionary<Vector2Int, Building> Buildings { get; private set; }

    public Transform ObstaclesParent;
    public Dictionary<Vector2Int, GameObject> Obstacles { get; private set; }
    
    public Transform ResourcesParent;
    public Dictionary<Vector2Int, Resource> Resources { get; private set; }
    
    public Transform EnvironmentParent;

    private void Awake()
    {
        Buildings = new Dictionary<Vector2Int, Building>();
        Obstacles = new Dictionary<Vector2Int, GameObject>();
        Resources = new Dictionary<Vector2Int, Resource>();
    }
}
