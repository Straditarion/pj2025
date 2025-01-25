using System.Collections.Generic;
using UnityEngine;

public class WorldContext
{
    public GameObject Parent;
    public GameObject TempParent;
    public Vector2 Spawn = new Vector2(16777216, 16777216);
    
    private readonly Dictionary<(FastNoiseExtension noise, int seed), FastNoise> _cache = new Dictionary<(FastNoiseExtension noise, int seed), FastNoise>();
    
    public FastNoise GetNoiseLib(FastNoiseExtension noise, int seed)
    {
        if (!_cache.TryGetValue((noise, seed), out var lib))
        {
            lib = noise.GetLibInstance(seed);
            _cache.Add((noise, seed), lib);
        }

        return lib;
    }
}
