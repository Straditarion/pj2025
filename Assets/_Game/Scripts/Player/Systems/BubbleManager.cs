using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(100)]
public class BubbleManager : PlayerSystem
{
    [SerializeField]
    public float Volume;
    [SerializeField]
    public float Exponent;
    [SerializeField]
    public Transform Visuals;
    [SerializeField]
    public WorldGenerator World;

    private float _radius;

    public float Radius => _radius;

    void Start()
    {
        Visuals.gameObject.SetActive(true);
        
        Visuals.position = (Vector3)World.Spawn;
        Visuals.position -= Vector3.forward * 2f;
    }
    
    private void Update()
    {
        _radius = Mathf.Pow(Volume, Exponent);
        
        Visuals.localScale = Vector3.one * _radius * 2f;
    }

    public bool IsWithinBubble(Vector2 position, float size)
    {
        var distance = (position - (Vector2)Visuals.position).magnitude;
        
        return distance - size >= _radius;
    }
}