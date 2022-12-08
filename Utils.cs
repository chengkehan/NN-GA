using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float MAX_PERTURBATION = 0.3f;

    public static int NUM_COPIES_ELITE = 1;

    public static int NUM_ELITE = 4;

    public static int NUM_INPUTS = 4;

    public static int NUM_OUTPUTS = 2;

    public static int NUM_HIDDEN_LAYERS = 1;

    public static int NUM_NEURONS_PER_HIDDEN_LAYER = 10;

    public static float MUTATION_RATE = 0.1f;

    public static float CROSSOVER_RATE = 0.7f;

    public static int NUM_TICKS = 2000;

    public static float RandomClamped()
    {
        return Random.Range(-1, 1);
    }

    public static float RandomFloat()
    {
        return Random.value;
    }

    public static int RandomInt(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }
}
