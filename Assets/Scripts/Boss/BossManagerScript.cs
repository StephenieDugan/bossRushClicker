using System.Collections.Generic;
using UnityEngine;

public class BossManagerScript : MonoBehaviour
{
    [Header("Boss Settings")]
    public List<GameObject> allBosses; // List of all available bosses
    public List<GameObject> defeatedBosses; // List of defeated bosses
    private List<GameObject> bossesToFight; // List of bosses remaining to fight
    private GameObject latestSpawnedBoss;  // Track the latest boss spawned


    [Header("Progression Settings")]
    public int healthIncreasePerBoss = 20; // Health increase for each new boss

    private int currentBossIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the bossesToFight list with all bosses
        bossesToFight = new List<GameObject>(allBosses);
        ShuffleBossList(bossesToFight); // Randomize the order
        SpawnNextBoss();
    }

    // Shuffle the boss list to randomize the fight order
    private void ShuffleBossList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Spawn the next boss
    public void SpawnNextBoss()
    {
        if (currentBossIndex < bossesToFight.Count)
        {
            GameObject bossPrefab = bossesToFight[currentBossIndex];
            GameObject newBoss = Instantiate(bossPrefab, transform.position, Quaternion.identity);

            // Increase the boss's health and money given progressively
            if (newBoss.TryGetComponent(out BossScript bossScript))
            {
                bossScript.max_health += currentBossIndex * healthIncreasePerBoss;
                bossScript.current_health = bossScript.max_health; // Set current health to max after increase

                bossScript.reward_min += 0.25f;
                bossScript.reward_max += 2.0f;
            }
            else
            {
                Debug.LogError("BossScript component not found on the spawned boss!");
            }

            currentBossIndex++;
        }
        else
        {
            Debug.Log("All bosses have been defeated!");
            // You could trigger an end game event or reset
        }
    }
    // Return the latest spawned boss
    public GameObject GetLatestSpawnedBoss()
    {
        return latestSpawnedBoss;
    }

    // Track boss defeat and move to the next one
    public void DefeatBoss(GameObject defeatedBoss)
    {
        if (!defeatedBosses.Contains(defeatedBoss))
        {
            defeatedBosses.Add(defeatedBoss); // Add defeated boss to the list
            SpawnNextBoss(); // Spawn the next boss
        }
    }
    
    // Handle when the player loses a fight
    public void PlayerLostFight()
    {
        if (currentBossIndex > 0)
        {
            currentBossIndex--; // Go back to the last boss
            SpawnNextBoss();
        }
        else
        {
            Debug.Log("No previous boss to return to!");
            // Handle restart or game over
        }
    }
}
