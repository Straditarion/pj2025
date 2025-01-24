using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New World Biome", menuName = "World Biome")]
public class WorldBiome : ScriptableObject
{
    [Serializable]
    public class ChildBiome
    {
        [SerializeField] 
        public WorldBiome Biome;
        [SerializeField] 
        public Vector2 NoiseRange;
    }
    
    [Header("Region")]
    [SerializeField] 
    public FastNoiseExtension Noise;
    [SerializeField] 
    public Vector2 NoiseRange;
    [SerializeField] 
    public float NoiseThreshold;

    [Header("Child")] 
    [SerializeField] 
    public ChildBiome[] ChildBiomes;
    
    [SerializeField]
    public WorldTile Tile;

    public float Evaluate(Vector2Int position, int seed, float mask, WorldContext context)
    {
        var noiseLib = context.GetNoiseLib(Noise, seed);

        var noiseValue = noiseLib.GetNoise(position.x, position.y);
        
        var noise = (noiseValue - NoiseRange.x) / (NoiseRange.y - NoiseRange.x);
        noise = Mathf.Clamp01(noise) * mask;
        
        return noise - NoiseThreshold;
    }
    
    public bool TryPlace(Vector2Int position, int seed, float mask, WorldContext context)
    {
        var noiseLib = context.GetNoiseLib(Noise, seed);

        var noiseValue = noiseLib.GetNoise(position.x, position.y);
        
        var noise = (noiseValue - NoiseRange.x) / (NoiseRange.y - NoiseRange.x);
        noise = Mathf.Clamp01(noise) * mask;

        if (noise < NoiseThreshold)
            return false;

        ChildBiome bestChildBiome = null;
        var bestChildBiomeMask = 0f;
        var bestChildBiomeRating = 0f;

        foreach (var childBiome in ChildBiomes)
        {
            var childNoise = (noiseValue - childBiome.NoiseRange.x) / (childBiome.NoiseRange.y - childBiome.NoiseRange.x);
            childNoise = Mathf.Clamp01(childNoise) * mask;
            
            var childRating = childBiome.Biome.Evaluate(position, seed, childNoise, context);

            if (childRating >= bestChildBiomeRating)
            {
                bestChildBiome = childBiome;
                bestChildBiomeMask = childNoise;
                bestChildBiomeRating = childRating;
            }
        }
        
        if(bestChildBiome != null)
            if (bestChildBiome.Biome.TryPlace(position, seed, bestChildBiomeMask, context))
                return true;

        Tile.Instantiate(position, context.Parent);
        return true;
    }
}
