using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedPrefab
{
    public GameObject prefab;
    public int weight;

    public WeightedPrefab(GameObject pf, int w = 1)
    {
        prefab = pf;
        weight = w;
    }
}

[CreateAssetMenu(fileName = "New Item Pool", menuName = "ItemPool")]
public class ItemSpawnPool : ScriptableObject
{
    public WeightedPrefab[] Items;

    public static GameObject SelectFromPool(ItemSpawnPool pool)
    {
        if(!pool)
        {
            Debug.LogWarning("Invalid item spawn pool attemping to be used!");
            return null;
        }

        int totalWeight = 0;
        foreach(WeightedPrefab item in pool.Items)
        {
            totalWeight += item.weight;
        }

        int selectedItem = Random.Range(0, totalWeight) + 1;

        foreach(WeightedPrefab item in pool.Items)
        {
            selectedItem -= item.weight;
            if(selectedItem <= 0)
            {
                return item.prefab;
            }
        }

        return null;
    }
}
