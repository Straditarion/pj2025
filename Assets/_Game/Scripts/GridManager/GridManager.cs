using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    public Transform BuildingsParent;
    public Grid<Building> Buildings { get; private set; }

    public Transform ObstaclesParent;
    public Grid<GameObject> Obstacles { get; private set; }
    
    public Transform ResourcesParent;
    public Grid<Resource> Resources { get; private set; }
    
    public Transform EnvironmentParent;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("Multiple instances of GridManager");

        Buildings = new();
        Obstacles = new();
        Resources = new();
        
        Instance = this;
    }
}