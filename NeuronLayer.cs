using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronLayer
{
    private int _numNeurons = 0;
    public int numNeurons
    {
        private set
        {
            _numNeurons = value;
        }
        get
        {
            return _numNeurons;
        }
    }

    private Neuron[] _neurons = null;
    public Neuron[] neurons
    {
        private set
        {
            _neurons = value;
        }
        get
        {
            return _neurons;
        }
    }

    public NeuronLayer(int numNeurons, int numInputsPerNeuron)
    {
        this.numNeurons = numNeurons;

        neurons = new Neuron[numNeurons];

        for (int i = 0; i < numNeurons; i++)
        {
            neurons[i] = new Neuron(numInputsPerNeuron);
        }
    }

    public int NumberWeights()
    {
        return numNeurons * neurons[0].numInputs;
    }

    public float[] Output(float[] inputs)
    {
        float[] outputs = new float[numNeurons];

        for (int i = 0; i < numNeurons; i++)
        {
            outputs[i] = neurons[i].Output(inputs);
        }

        return outputs;
    }
}
