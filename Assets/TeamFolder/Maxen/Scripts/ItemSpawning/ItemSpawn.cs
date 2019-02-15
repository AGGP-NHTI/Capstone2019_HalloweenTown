using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    #region Variables
    //Item spawning variables
    public float timeUntilItemRespawn;
    public ItemSpawnPool spawnPool;
    protected GameObject currentItem;
    protected bool coolDownActive = false;
    #endregion

    protected virtual IEnumerator itemRespawnCoolDown()
    {
        Debug.Log("Cooldown start...");
        coolDownActive = true;
        for(float timer = timeUntilItemRespawn; timer > 0.0f; timer -= Time.deltaTime)
        {
            yield return null;
        }

        GameObject itemToSpawn = ItemSpawnPool.SelectFromPool(spawnPool);
        if(itemToSpawn)
        {
            currentItem = Instantiate(itemToSpawn, transform);
        }
        else
        {
            Debug.LogWarning("Could not spawn item at " + name + ", null prefab returned.");
        }

        coolDownActive = false;
    }

    private void FixedUpdate()
    {
        if(currentItem == null && !coolDownActive)
        {
            StartCoroutine(itemRespawnCoolDown());
        }
    }
}
