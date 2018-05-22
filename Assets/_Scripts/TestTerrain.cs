﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTerrain : MonoBehaviour {

    private Terrain terrain;
    [SerializeField]
    private Texture2D[] textures;
    public GroundManager tc;
    private TerrainData terrainData;


    const float pi = Mathf.PI;
    int xRes, yRes;
    int xBase, yBase;
    float[,] heights;
    float[,,] maps;
    // Use this for initialization
    void Start()
    {
        GameObject light = GameObject.Find("Directional Light");
        light.GetComponent<Light>().color = Color.white;
        light.transform.localPosition = new Vector3(125, 100, 125);
        light.transform.localEulerAngles = new Vector3(90, 0, 0);


        terrain = gameObject.GetComponent<Terrain>();
        terrainData = new TerrainData();
        
        xBase = 0;
        xBase = 0;
        terrainData = terrain.terrainData;
        heights = terrainData.GetHeights(xBase, yBase,513,513);
        maps = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.heightmapHeight, 3];
        getSplatProttypes(textures);
        /*
        heights[10, 10] = 1f;
        heights[20, 20] = 0.5f;
        */
        
        for(int x = 0; x< terrainData.heightmapWidth; x++)
        {
            for( int y = 0; y < terrainData.heightmapHeight; y++)
            {
                heights[x, y] = 0f;
               // heights[x, y] = Sine2DFunction((float)x / 100, (float)y / 100, 0);
                //
            }
        }

        for (int x = 0; x < terrainData.alphamapResolution-1; x++)
        {
            for (int y = 0; y < terrainData.alphamapResolution-1; y++)
            {
                maps[x, y, getTextureIndex(heights[x, y])] = 1f;
                //
            }
        }
        terrain.terrainData.splatPrototypes = getSplatProttypes(textures);

        terrain.terrainData.SetHeights(0, 0, heights);
        terrain.terrainData.SetAlphamaps(0, 0, maps);

        /*
        //terrain.terrainData = terrainData;
        Debug.Log("heightmapWidth" + terrain.terrainData.heightmapHeight);
        Debug.Log("heightmapHeight" + terrainData.heightmapWidth);


        Debug.Log("alphamapHeight" + terrain.terrainData.alphamapHeight);
        Debug.Log("alphamapLayers" + terrain.terrainData.alphamapLayers);
        Debug.Log("alphamapTextures" + terrain.terrainData.alphamapTextures);
        Debug.Log("alphamapWidth" + terrainData.alphamapWidth);
        Debug.Log("alphamapResolution" + terrainData.alphamapResolution);
        */
    }

    private void Update()
    {
    }

    public void setHeight(float t, float strength)
    {
        for (int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for (int y = 0; y < terrainData.heightmapHeight; y++)
            {
                heights[x, y] = Ripple((float)(x - 256) / 100f, (float)(y - 256) / 100f, t) * strength;
                //heights[x, y] = Sine2DFunction((float)x / 10f, (float)y / 10f, Time.time);
                //
            }
        }
        for (int x = 251; x < 261; x++)
        {
            for (int y = 251; y < 261; y++)
            {
                //heights[x, y] = Ripple((float)(x - 256) / 100f, (float)(y - 256) / 100f, t) * strength;
                heights[x, y] = 0;
                //heights[x, y] = Sine2DFunction((float)x / 10f, (float)y / 10f, Time.time);
                //
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public SplatPrototype[] getSplatProttypes(Texture2D[] texs)
    {
        SplatPrototype[] splayPrototypes = new SplatPrototype[texs.Length];

        for (int i = 0; i < texs.Length; i++)
        {
            splayPrototypes[i] = new SplatPrototype();
            splayPrototypes[i].tileSize = Vector2.one*5;
            splayPrototypes[i].texture = texs[i];
        }

        return splayPrototypes;
    }

    private int getTextureIndex(float height)
    {
        if (height < 0.3f)
            return 0;
        if (height >= 0.3f && height < 0.6f)
            return 1;
        if (height >= 0.6)
            return 2;
        return 1;
    }

    static float Sine2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }

    static float Ripple(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * d - Time.time));

        y /= (30f + 10f * d);
        y /= (5*t+1f);
        return y;
    }

}
