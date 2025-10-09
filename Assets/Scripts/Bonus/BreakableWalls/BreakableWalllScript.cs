using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWalllScript : MonoBehaviour
{
    [SerializeField] private List<WallLayerStruct> wallLayers = new List<WallLayerStruct>();

    void Update()
    {
        CheckWallIntegridy();
    }

    private void CheckWallIntegridy()
    {
        float layerWeight = 0;
        float layerStrength;
        int currentLayer = wallLayers.Count;

        List<BreakableWallPartScript> partsToRemove = new List<BreakableWallPartScript>();

        WallLayerStruct wallLayer;

        for (int j = wallLayers.Count - 1; j >= 0; j--)
        {
            wallLayer = wallLayers[j];
            layerStrength = 0;

            foreach (BreakableWallPartScript part in wallLayer.wallParts)
            {
                layerWeight += part.weight;
                layerStrength += part.strength;
            }

            Debug.Log(layerWeight + " " + layerStrength + " " + (layerWeight > layerStrength));

            if (layerWeight > layerStrength)
            {
                Debug.Log("-");
                for (int i = currentLayer; i < wallLayers.Count; i++)
                {
                    Debug.Log(currentLayer + " " + wallLayers.Count);
                    Debug.Log("Remove Loop");
                    foreach (BreakableWallPartScript part in wallLayers[i].wallParts)
                    {
                        part.Break(false);
                        partsToRemove.Add(part);
                    }
                }
            }

            //Debug.Log("Layer: " + currentLayer);
            //Debug.Log("Weight: " + layerWeight);
            //Debug.Log("Layer strength: " + layerStrength);

            currentLayer--;
        }

        foreach (BreakableWallPartScript part in partsToRemove)
        {
            RemovePart(part);
        }
    }

    public void RemovePart(BreakableWallPartScript wallPartScript)
    {
        int wallLayerCount = wallLayers.Count;
        int wallPartsCount;

        for (int i = 0; i < wallLayerCount; i++)
        {
            wallPartsCount = wallLayers[i].wallParts.Count;
            for (int j = 0; j < wallPartsCount; j++)
            {
                if (wallLayers[i].wallParts[j] == wallPartScript)
                {
                    wallLayers[i].wallParts.Remove(wallPartScript);
                    return;
                }
            }
        }

        /*foreach (WallLayerStruct wallLayer in wallLayers)
        {
            foreach (BreakableWallPartScript part in wallLayer.wallParts)
            {
                if (part == wallPartScript)
                {
                    wallLayer.wallParts.Remove(part);
                }
            }
        }*/
    }

    public float GetWeightOnLayer(int layerIndex)
    {
        float weight = 0f;

        for (int i = layerIndex; i < wallLayers.Count; i++)
        {
            foreach (BreakableWallPartScript part in wallLayers[i].wallParts)
            {
                weight += part.weight;
            }
        }

        return weight;
    }

    public float GetLayerStrength(int layerIndex)
    {
        float strength = 0f;

        foreach (BreakableWallPartScript part in wallLayers[layerIndex].wallParts)
        {
            strength += part.strength;
        }

        return strength;
    }
}

[Serializable]
struct WallLayerStruct
{
    public List<BreakableWallPartScript> wallParts;
}