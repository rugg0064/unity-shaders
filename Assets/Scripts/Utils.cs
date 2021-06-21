using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Texture3D generate3dTexture(int sizeX, int sizeY, int sizeZ)
    {
        Texture3D t3d = new Texture3D(sizeX, sizeY, sizeZ, TextureFormat.RGB24, false);
        t3d.filterMode = FilterMode.Point;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    t3d.SetPixel(x, y, z, c, 0);
                }
            }
        }
        t3d.Apply();
        return t3d;
    }

    public static Texture2D generate2dTexture(int sizeX, int sizeY)
    {
        Texture2D t2d = new Texture2D(sizeX, sizeY);
        
        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++)
            {
                float randDirection = Random.Range(0, Mathf.PI * 2);
                Color c = new Color( (Mathf.Cos(randDirection)+1)/2 , (Mathf.Sin(randDirection)+1)/2, 0);
                //Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);
                t2d.SetPixel(x, y, c);
            }
        }
        t2d.Apply();
        return t2d;
    }
   
    public static Texture2D renderTextureToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public static Texture2D generatePerlinNoise(int outputResolution, int noiseSamples)
    {
        Texture2D noiseTexture = new Texture2D(noiseSamples, noiseSamples);
        for (int x = 0; x < noiseSamples; x++)
        {
            for (int y = 0; y < noiseSamples; y++)
            {
                float randDirection = Random.Range(0, Mathf.PI * 2);
                Color c = new Color((Mathf.Cos(randDirection) + 1) / 2, (Mathf.Sin(randDirection) + 1) / 2, 0);
                noiseTexture.SetPixel(x, y, c);
            }
        }
        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.Apply();

        ComputeShader cs = Resources.Load("Compute") as ComputeShader;
        RenderTexture rt = new RenderTexture(outputResolution, outputResolution, 24);
        rt.wrapMode = TextureWrapMode.Repeat;
        rt.filterMode = FilterMode.Point;
        rt.antiAliasing = 1;
        rt.enableRandomWrite = true;
        rt.Create();
        cs.SetFloat("ResultResolution", (float) outputResolution);
        cs.SetTexture(0, "ResultTexture", rt);

        cs.SetFloat("NoiseResolution", (float) noiseSamples);
        cs.SetTexture(0, "NoiseTexture", noiseTexture);

        cs.Dispatch(0, outputResolution / 8, outputResolution / 8, 1);

        return renderTextureToTexture2D(rt);
    }
}
