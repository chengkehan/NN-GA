using System;

public class GenAlg
{
    private Genome[] chromosome = null;

    private int numberGenomes = 0;

    private int numberWeightsPerGenome = 0;

    private float totalFitness = 0;

    private float bestFitness = 0;

    private float averageFitness = 0;

    private float worstFitness = 0;

    private int fittestGenome = 0;

    private float mutationRate = 0;

    private float crossoverRate = 0;

    private int generationCount = 0;

    public GenAlg(int numberGenomes, float mutationRate, float crossoverRate, int numberWeightsPerGenome)
    {
        this.numberGenomes = numberGenomes;
        this.mutationRate = mutationRate;
        this.crossoverRate = crossoverRate;
        this.numberWeightsPerGenome = numberWeightsPerGenome;

        Reset();

        chromosome = new Genome[numberGenomes];
        for (int genomeI = 0; genomeI < chromosome.Length; genomeI++)
        {
            chromosome[genomeI] = new Genome();
            chromosome[genomeI].weights = new float[numberWeightsPerGenome];
            for (int weightI = 0; weightI < chromosome[genomeI].weights.Length; weightI++)
            {
                chromosome[genomeI].weights[weightI] = Utils.RandomClamped();
            }
        }
    }

    private void Mutate(Genome genome)
    {
        for (int weightI = 0; weightI < genome.weights.Length; weightI++)
        {
            if (Utils.RandomFloat() < mutationRate)
            {
                genome.weights[weightI] += Utils.RandomClamped() * Utils.MAX_PERTURBATION;
            }
        }
    }

    private Genome GetGenomeRoulette()
    {
        float slice = Utils.RandomFloat() * totalFitness;
        float fitnessSoFar = 0;

        for (int genomeI = 0; genomeI < chromosome.Length; genomeI++)
        {
            fitnessSoFar += chromosome[genomeI].fitness;
            if (fitnessSoFar >= slice)
            {
                return chromosome[genomeI];
            }
        }

        throw new Exception("Unexpected");
    }

    private void Crossover(Genome mum, Genome dad, Genome baby1, Genome baby2)
    {
        if (Utils.RandomFloat() > crossoverRate || mum.IsTheSameAs(dad))
        {
            baby1.weights = new float[numberWeightsPerGenome];
            Buffer.BlockCopy(mum.weights, 0, baby1.weights, 0, numberWeightsPerGenome);

            baby2.weights = new float[numberWeightsPerGenome];
            Buffer.BlockCopy(dad.weights, 0, baby2.weights, 0, numberWeightsPerGenome);
            return;
        }

        baby1.weights = new float[numberWeightsPerGenome];
        baby2.weights = new float[numberWeightsPerGenome];

        int cp = Utils.RandomInt(0, numberWeightsPerGenome);

        for (int i = 0; i < cp; i++)
        {
            baby1.weights[i] = mum.weights[i];
            baby2.weights[i] = dad.weights[i];
        }

        for (int i = cp; i < numberWeightsPerGenome; i++)
        {
            baby1.weights[i] = dad.weights[i];
            baby2.weights[i] = mum.weights[i];
        }
    }

    public void Epoch()
    {
        Reset();

        Array.Sort(chromosome, GenomeComparer);

        CalculateBestWorstAvTot();

        Genome[] newChromosome = new Genome[numberGenomes];

        if (Utils.NUM_COPIES_ELITE * Utils.NUM_ELITE % 2 != 0)
        {
            throw new Exception("Unexpected");
        }

        GrabNBest(Utils.NUM_ELITE, Utils.NUM_COPIES_ELITE, newChromosome);

        int numGenerated = Utils.NUM_ELITE * Utils.NUM_COPIES_ELITE;
        while (numGenerated < numberGenomes)
        {
            Genome mum = GetGenomeRoulette();
            Genome dad = GetGenomeRoulette();

            Genome baby1 = new Genome();
            Genome baby2 = new Genome();

            Crossover(mum, dad, baby1, baby2);

            Mutate(baby1);
            Mutate(baby2);

            newChromosome[numGenerated++] = baby1;
            newChromosome[numGenerated++] = baby2;
        }

        chromosome = newChromosome;
    }

    private void GrabNBest(int numElite, int numCopies, Genome[] newChromosome)
    {
        int elites = numElite;
        while (numElite-- > 0)
        {
            for (int i = 0; i < numCopies; i++)
            {
                newChromosome[i * elites + numElite] = chromosome[numberGenomes - 1 - numElite].Clone();
            }
        }
    }

    private void CalculateBestWorstAvTot()
    {
        float highestSoFar = 0;
        float lowestSoFar = 9999999;

        for (int genomeI = 0; genomeI < numberGenomes; genomeI++)
        {
            if (chromosome[genomeI].fitness > highestSoFar)
            {
                highestSoFar = chromosome[genomeI].fitness;
                fittestGenome = genomeI;
                bestFitness = highestSoFar;
            }

            if (chromosome[genomeI].fitness < lowestSoFar)
            {
                lowestSoFar = chromosome[genomeI].fitness;
                worstFitness = lowestSoFar;
            }

            totalFitness += chromosome[genomeI].fitness;
        }

        averageFitness = totalFitness / numberGenomes;
    }

    private int GenomeComparer(Genome x, Genome y)
    {
        if (x.fitness < y.fitness)
        {
            return -1;
        }
        else if (x.fitness > y.fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void Reset()
    {
        totalFitness = 0;
        bestFitness = 0;
        worstFitness = 9999999;
        averageFitness = 0;
        fittestGenome = 0;
    }
}
