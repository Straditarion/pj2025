using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : PlayerSystem
{
    private void Update()
    {
        Building();
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
}