using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationHelper : MonoBehaviour
{
    private NeuralNet _neuralNet = null;
    private NeuralNet neuralNet
    {
        get
        {
            if (_neuralNet == null)
            {
                _neuralNet = new NeuralNet(5, 2, 1, 4);
            }
            return _neuralNet;
        }
    }

    public void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        {
            GL.LoadOrtho();

            DrawNeuralNet(neuralNet);
        }
        GL.PopMatrix();
    }

    private void OnGUI()
    {
        DrawWeights();
    }

    #region NeuralNet Drawer

    private const float MARGIN = 80;

    private const float SPACE = 60;

    private const float GAP = 100;

    private void DrawWeights()
    {
        GUI.Label(new Rect(0, 0, 20, 200), "34234");
    }

    private void DrawNeuralNet(NeuralNet neuralNet)
    {
        DrawLinks(neuralNet);
        DrawInputs(neuralNet);
        DrawNeuronLayers(neuralNet);
        
    }

    private void DrawLinks(NeuralNet neuralNet)
    {
        for (int i = 0; i < neuralNet.neuronLayers.Length; i++)
        {
            for (int j = 0; j < neuralNet.neuronLayers[i].neurons.Length; j++)
            {
                var neuronPoint = NeuronPoint(neuralNet, i, j);

                if (i == 0)
                {
                    for (int k = 0; k < neuralNet.numInputs; k++)
                    {
                        var inputPoint = InputPoint(k);
                        GL_Line(Color.white, inputPoint, neuronPoint);
                    }
                }
                else
                {
                    for (int k = 0; k < neuralNet.neuronLayers[i - 1].neurons.Length; k++)
                    {
                        var previousLayerNeuronPoint = NeuronPoint(neuralNet, i - 1, k);
                        GL_Line(Color.white, previousLayerNeuronPoint, neuronPoint);
                    }
                }
            }
        }
    }

    private void DrawInputs(NeuralNet neuralNet)
    {
        for (int i = 0; i < neuralNet.numInputs; i++)
        {
            GL_Rect(Color.white, InputPoint(i), 20);
        }
    }

    private void DrawNeuronLayers(NeuralNet neuralNet)
    {
        for (int i = 0; i < neuralNet.neuronLayers.Length; i++)
        {
            DrawHiddenLayer(neuralNet, i);
        }
    }

    private void DrawHiddenLayer(NeuralNet neuralNet, int hiddenLayerIndex)
    {
        var neuronLayer = neuralNet.neuronLayers[hiddenLayerIndex];
        for (int i = 0; i < neuronLayer.numNeurons; i++)
        {
            GL_Circle(Color.white, NeuronPoint(neuralNet, hiddenLayerIndex, i), 10);
        }
    }

    private Vector2 NeuronPoint(NeuralNet neuralNet, int layerIndex, int neuronIndex)
    {
        Vector2 refPoint = Vector2.zero;
        if (neuralNet.numInputs % 2 == 0)
        {
            refPoint = InputPoint((int)(neuralNet.numInputs * 0.5f)) + InputPoint((int)(neuralNet.numInputs * 0.5f) + 1);
            refPoint *= 0.5f;
        }
        else
        {
            refPoint = InputPoint(Mathf.FloorToInt(neuralNet.numInputs * 0.5f));
        }

        refPoint.x -= (neuralNet.neuronLayers[layerIndex].numNeurons * 0.5f - 0.5f) * SPACE;
        refPoint.y += (layerIndex + 1) * GAP;

        refPoint.x += neuronIndex * SPACE;
        return refPoint;
    }

    private Vector2 InputPoint(int inputIndex)
    {
        return new Vector2(MARGIN + inputIndex * SPACE, MARGIN);
    }

    #endregion

    #region GL Primitive

    private Material _lineMaterial = null;
    private Material lineMaterial
    {
        get
        {
            if (_lineMaterial == null)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                _lineMaterial = new Material(shader);
                _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                _lineMaterial.SetInt("_ZWrite", 0);
            }
            return _lineMaterial;
        }
    }

    private void GL_Point(float px, float py)
    {
        float x = px / Screen.width;
        float y = py / Screen.height;
        GL.Vertex3(x, y, 0);
    }

    private void GL_Point(Vector2 p)
    {
        GL_Point(p.x, p.y);
    }

    private void GL_Line(Color color, float px1, float py1, float px2, float py2)
    {
        GL.Begin(GL.LINES);
        {
            GL.Color(color);
            GL_Point(px1, py1);
            GL_Point(px2, py2);
        }
        GL.End();
    }

    private void GL_Line(Color color, Vector2 p1, Vector2 p2)
    {
        GL_Line(color, p1.x, p1.y, p2.x, p2.y);
    }

    private void GL_Circle(Color color, Vector2 center, float radius)
    {
        int step = 10;
        for (int i = 0; i < 360; i += step)
        {
            float radian = i;
            float x1 = radius * Mathf.Cos(radian * Mathf.Deg2Rad);
            float y1 = radius * Mathf.Sin(radian * Mathf.Deg2Rad);

            float x2 = radius * Mathf.Cos((radian + step) * Mathf.Deg2Rad);
            float y2 = radius * Mathf.Sin((radian + step) * Mathf.Deg2Rad);

            GL_Line(color, center.x + x1, center.y + y1, center.x + x2, center.y + y2);
        }
    }

    private void GL_Rect(Color color, Vector2 center, float size)
    {
        float halfSize = size * 0.5f;
        Vector2 p1 = new Vector2(center.x - halfSize, center.y - halfSize);
        Vector2 p2 = new Vector2(center.x - halfSize, center.y + halfSize);
        Vector2 p3 = new Vector2(center.x + halfSize, center.y + halfSize);
        Vector2 p4 = new Vector2(center.x + halfSize, center.y - halfSize);
        GL_Line(color, p1, p2);
        GL_Line(color, p2, p3);
        GL_Line(color, p3, p4);
        GL_Line(color, p4, p1);
    }

    #endregion
}
