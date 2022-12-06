using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet
{
    private int _numInputs = 0;
    public int numInputs
    {
        private set
        {
            _numInputs = value;
        }
        get
        {
            return _numInputs;
        }
    }

    private int _numOutputs = 0;
    public int numOutputs
    {
        private set
        {
            _numOutputs = value;
        }
        get
        {
            return _numOutputs;
        }
    }

    private int _numHiddenLayers = 0;
    public int numHiddenLayers
    {
        private set
        {
            _numHiddenLayers = value;
        }
        get
        {
            return _numHiddenLayers;
        }
    }

    private int _numNeuronsPerHiddenLayer = 0;
    public int numNeuronsPerHiddenLayer
    {
        private set
        {
            _numNeuronsPerHiddenLayer = value;
        }
        get
        {
            return _numNeuronsPerHiddenLayer;
        }
    }

    private NeuronLayer[] _neuronLayers = null;
    public NeuronLayer[] neuronLayers
    {
        private set
        {
            _neuronLayers = value;
        }
        get
        {
            return _neuronLayers;
        }
    }

    private int numWeights = 0;

    public NeuralNet(int numInputs, int numOutputs, int numHiddenLayers, int numNeuronsPerHiddenLayer)
    {
        this.numInputs = numInputs;
        this.numOutputs = numOutputs;
        this.numHiddenLayers = numHiddenLayers;
        this.numNeuronsPerHiddenLayer = numNeuronsPerHiddenLayer;

        Create();
    }

    private void Create()
    {
        numWeights = 0;

        if (numHiddenLayers > 0)
        {
            neuronLayers = new NeuronLayer[numHiddenLayers + 1/*output layer*/];

            // first hidden layer
            neuronLayers[0] = new NeuronLayer(numNeuronsPerHiddenLayer, numInputs);
            numWeights += neuronLayers[0].NumberWeights();

            // other hidden layers
            for (int i = 1; i < neuronLayers.Length - 1/*output layer*/; i++)
            {
                neuronLayers[i] = new NeuronLayer(numNeuronsPerHiddenLayer, numNeuronsPerHiddenLayer);
                numWeights += neuronLayers[i].NumberWeights();
            }

            // output layer
            neuronLayers[neuronLayers.Length - 1] = new NeuronLayer(numOutputs, numNeuronsPerHiddenLayer);
            numWeights += neuronLayers[neuronLayers.Length - 1].NumberWeights();
        }
        else
        {
            neuronLayers = new NeuronLayer[1];

            // output layer
            neuronLayers[0] = new NeuronLayer(numOutputs, numInputs);
            numWeights += neuronLayers[0].NumberWeights();
        }
    }

    public float[] GetWeights()
    {
        float[] weights = new float[numWeights];
        int index = 0;

        for (int i = 0; i < neuronLayers.Length; i++)
        {
            for (int j = 0; j < neuronLayers[i].numNeurons; j++)
            {
                for (int k = 0; k < neuronLayers[i].neurons[j].numInputs; k++)
                {
                    weights[index] = neuronLayers[i].neurons[j].weights[k];
                    ++index;
                }
            }
        }

        return weights;
    }

    public void PutWeights(float[] weights)
    {
        int index = 0;

        for (int i = 0; i < neuronLayers.Length; i++)
        {
            for (int j = 0; j < neuronLayers[i].numNeurons; j++)
            {
                for (int k = 0; k < neuronLayers[i].neurons[j].numInputs; k++)
                {
                    neuronLayers[i].neurons[j].weights[k] = weights[index];
                    ++index;
                }
            }
        }
    }

    public int NumberWeights()
    {
        return numWeights;
    }

    public float[] Update(float[] inputs)
    {
        if (inputs == null || inputs.Length != numInputs)
        {
            return null;
        }

        float[] currentInputs = inputs;

        for (int i = 0; i < neuronLayers.Length; i++)
        {
            var neuronLayer = neuronLayers[i];
            currentInputs = neuronLayer.Output(currentInputs);
        }

        return currentInputs;
    }
}
