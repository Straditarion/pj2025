using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : PlayerSystem
{
    [SerializeField] 
    public float MovementSpeed;
    [SerializeField] 
    public float ZoomSpeedAdd;
    [SerializeField] 
    public float ZoomSpeedMul;
    [SerializeField] 
    public float ZoomInit = 10;
    [SerializeField] 
    public Camera PlayerCamera;
    [SerializeField] 
    public WorldGenerator World;

    private float _zoom;

    private void Awake()
    {
        _zoom = ZoomInit;
    }

    private void Update()
    {
        Building();
        Navigation();
    }

    private void Building()
    {
        var buildingSystem = _player.GetSystem<BuildingSystem>();
        var worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var worldPointInt = new Vector2Int(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y));
        
        buildingSystem.UpdateGhostPosition(worldPointInt);
        
        if (Mouse.current.leftButton.isPressed)
            buildingSystem.Place(worldPointInt);

        if (Mouse.current.rightButton.isPressed)
            buildingSystem.Remove(worldPointInt);
        
        if (Keyboard.current[Key.R].wasPressedThisFrame)
            buildingSystem.Rotate();
    }

    private void Navigation()
    {
        var scroll = Mouse.current.scroll.ReadValue().y;

        _zoom += scroll * (ZoomSpeedAdd + (ZoomSpeedMul * _zoom));
        _zoom = Mathf.Clamp(_zoom, 0f, (World.WorldSize + 0.5f) * (1f / Mathf.Max(1f, PlayerCamera.aspect)));
        
        PlayerCamera.orthographicSize = _zoom;
        
        var movement = Vector2.zero;
        
        if(Keyboard.current[Key.W].isPressed)
            movement += Vector2.up;
        if(Keyboard.current[Key.S].isPressed)
            movement += Vector2.down;
        if(Keyboard.current[Key.D].isPressed)
            movement += Vector2.right;
        if(Keyboard.current[Key.A].isPressed)
            movement += Vector2.left;
        
        movement *= MovementSpeed * _zoom * Time.deltaTime;

        var position = (Vector2)transform.position + movement;
        
        position.x = Mathf.Clamp(position.x, _zoom * PlayerCamera.aspect - (World.WorldSize + 0.5f), (World.WorldSize + 0.5f) - _zoom * PlayerCamera.aspect);
        position.y = Mathf.Clamp(position.y, _zoom - (World.WorldSize + 0.5f), (World.WorldSize + 0.5f) - _zoom);
        
        transform.position = (Vector3)position;
    }
}