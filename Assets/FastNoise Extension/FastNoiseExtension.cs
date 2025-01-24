using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastNoiseExtension : ScriptableObject
{

    [Header("Main Settings")]
    public FastNoise.NoiseType mainType = FastNoise.NoiseType.Simplex;
    public FastNoise.Interp interpolationType = FastNoise.Interp.Quintic;
    public float frequency = 0.01f;

    [Header("Fractal Settings")]
    public int octaves = 3;
    public float lacunarity = 2.0f;
    public float gain = 0.5f;
    public FastNoise.FractalType fractalType = FastNoise.FractalType.FBM;

    [Header("Cellular Settings")]
    public FastNoise.CellularDistanceFunction distanceFunction = FastNoise.CellularDistanceFunction.Euclidean;
    public FastNoise.CellularReturnType returnType = FastNoise.CellularReturnType.CellValue;
    public FastNoise lookupNoise;
    [Range(0, 3)] public int dstIndicieLow = 0;
    [Range(1, 4)] public int dstIndicieHigh = 1;
    public float jitter = 0.45f;

    [Header("Gradient Perturbation Settings")]
    public float gradientPerturbation = 1.0f;

    private int _hashCode;
    
    public FastNoise GetLibInstance (int seed)
    {
        FastNoise fn = new FastNoise();

        fn.SetSeed(seed);
        fn.SetFrequency(frequency);
        fn.SetInterp(interpolationType);
        fn.SetNoiseType(mainType);
        fn.SetFractalOctaves(octaves);
        fn.SetFractalLacunarity(lacunarity);
        fn.SetFractalGain(gain);
        fn.SetFractalType(fractalType);
        fn.SetCellularDistanceFunction(distanceFunction);
        fn.SetCellularReturnType(returnType);
        fn.SetCellularNoiseLookup(lookupNoise);
        fn.SetCellularDistance2Indicies(dstIndicieLow, dstIndicieHigh);
        fn.SetCellularJitter(jitter);
        fn.SetGradientPerturbAmp(gradientPerturbation);

        return fn;
    }

    private void Awake()
    {
        _hashCode = HashCode.Combine(GetInstanceID());
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}