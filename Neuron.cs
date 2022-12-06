using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
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

    private float[] _weights = null;
    public float[] weights
    {
        private set
        {
            _weights = value;
        }
        get
        {
            return _weights;
        }
    }

    public Neuron(int numInputs)
    {
        this.numInputs = numInputs + 1/*bias*/;

        weights = new float[this.numInputs];

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Utils.RandomClamped();
        }
    }

    public float Output(float[] inputs)
    {
        if (inputs == null || inputs.Length != numInputs - 1)
        {
            Debug.LogError("Unexpecte");
            return 0;
        }

        float output = 0;

        for (int i = 0; i < inputs.Length; i++)
        {
            output += inputs[i] * weights[i];
        }

        output += -1/*bias*/ * weights[numInputs - 1];

        output = Sigmoid(output, 1);

        return output;
    }

    private float Sigmoid(float netInput, float response)
    {
        return (1 / (1 + Mathf.Exp(-netInput / response)));
    }
}
