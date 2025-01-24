using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] 
    public WorldBiome ParentBiome;
    
    [SerializeField]
    public int WorldSize;

    private GameObject _container;
    
    private void OnEnable()
    {
        _container = new GameObject("Container");
        _container.transform.parent = transform;

        var context = new WorldContext()
        {
            Parent = _container
        };

        var seed = Random.Range(-16777216, 16777216);
        
        for (int x = -WorldSize; x <= WorldSize; x++)
        {
            for (int y = -WorldSize; y <= WorldSize; y++)
            {
                ParentBiome.TryPlace(new Vector2Int(x, y), seed, 1f, context);
            }
        }
    }

    private void OnDisable()
    {
        if(_container != null)
            Destroy(_container);
    }
}
