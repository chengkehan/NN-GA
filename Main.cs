using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private NeuralNet[] population = null;

    private GenAlg ga = null;

    private int ticksCount = 0;

    private void Start()
    {
        population = new NeuralNet[100];
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new NeuralNet();
        }

        ga = new GenAlg(population.Length, Utils.MUTATION_RATE, Utils.CROSSOVER_RATE, population[0].NumberWeights());

        for (int i = 0; i < population.Length; i++)
        {
            population[0].PutWeights(ga.chromosome[i].weights);
        }
    }

    private void Update()
    {
        if (ticksCount++ < Utils.NUM_TICKS)
        {
            // evolution of generation
            {
                for (int i = 0; i < population.Length; i++)
                {
                    // using random data as input, just for test.
                    // in real project, a meaningful input data is required.
                    float[] rndInput = new float[Utils.NUM_INPUTS];
                    for (int j = 0; j < Utils.NUM_INPUTS; j++)
                    {
                        rndInput[j] = Utils.RandomFloat();
                    }

                    float[] output = population[i].Update(rndInput);

                    // Now we get outputs from NeuralNet.
                    // Using outputs to control game unit,
                    // meanwhile, checking whether the goal of game unit is achieved.
                    // Add fitness for those ones.

                    // pseudocode
                    {
                        bool goalAchieved = false;/*checking conditions here*/
                        if (goalAchieved)
                        {
                            // increment fitness of neural net
                            // population[i].fitness++;
                        }

                        // pass fitness to genome
                        //ga.chromosome[i].fitness = population[i].fitness;
                    }
                }
            }
        }
        else
        {
            ticksCount = 0;

            // pick elites to generate new population
            {
                ga.Epoch();

                for (int i = 0; i < population.Length; i++)
                {
                    population[i].PutWeights(ga.chromosome[i].weights);
                    // reset data of neural net for a new generation
                }
            }
        }
    }
}
