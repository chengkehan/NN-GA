using System;
using System.Collections.Generic;
using UnityEngine;

public struct Genome
{
    public float[] weights;

    public float fitness;

    public bool IsTheSameAs(Genome another)
    {
        return weights == another.weights && fitness == another.fitness;
    }

    public Genome Clone()
    {
        var genome = new Genome();
        if (weights != null)
        {
            genome.weights = new float[weights.Length];
            Buffer.BlockCopy(weights, 0, genome.weights, 0, weights.Length);
        }
        genome.fitness = fitness;
        return genome;
    }
}
