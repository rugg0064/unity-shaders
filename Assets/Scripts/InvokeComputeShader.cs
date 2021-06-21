using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InvokeComputeShader : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    public Texture2D noiseTexture;
    // Start is called before the first frame update
    void Start()
    {
        /*
        //Random.InitState(0);

        Debug.Log("Activated");

        int renderRes = 2048;
        renderTexture = new RenderTexture(renderRes, renderRes, 24);
        renderTexture.wrapMode = TextureWrapMode.Repeat;
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.antiAliasing = 1;
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        computeShader.SetFloat("ResultResolution", (float)renderRes);
        computeShader.SetTexture(0, "ResultTexture", renderTexture);

        int noiseRes = 8;
        noiseTexture = Utils.generate2dTexture(noiseRes, noiseRes);
        noiseTexture.filterMode = FilterMode.Point;
        computeShader.SetFloat("NoiseResolution", (float)noiseRes);
        computeShader.SetTexture(0, "NoiseTexture", noiseTexture);

        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        this.GetComponent<MeshRenderer>().materials[0].mainTexture = renderTexture;
        */

        Texture2D t1 = Utils.generatePerlinNoise(1024, 2);
        Texture2D t2 = Utils.generatePerlinNoise(1024, 8);
        Texture2D t3 = Utils.generatePerlinNoise(1024, 16);
        Texture2D final = new Texture2D(1024, 1024);
        for (int x = 0; x < 1024; x++)
        {
            for (int y = 0; y < 1024; y++)
            {
                Color a = t1.GetPixel(x, y);
                Color b = t2.GetPixel(x, y);
                Color c = t3.GetPixel(x, y);
                float value = (a.r * 0.85f) + (b.r * 0.10f) + (c.r * 0.05f);
                final.SetPixel(x, y, new Color(value, value, value));
            }
        }
        final.Apply();
        this.GetComponent<MeshRenderer>().materials[0].mainTexture = final;
        //this.GetComponent<MeshRenderer>().materials[0].mainTexture = Utils.generatePerlinNoise(1024, 8);
    }


}
