using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private Dictionary<Type, PlayerSystem> _playerSystems = new();
    
    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("Only one instance of Player is allowed");

        foreach (var playerSystem in gameObject.GetComponents<PlayerSystem>())
        {
            _playerSystems.Add(playerSystem.GetType(), playerSystem);
            playerSystem.InjectPlayer(this);
        }
        
        Instance = this;
    }

    public TSystem GetSystem<TSystem>() where TSystem : PlayerSystem
    {
        return (TSystem)_playerSystems[typeof(TSystem)];
    }
}
