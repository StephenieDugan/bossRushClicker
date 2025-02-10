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

    [Header("Timer Settings")]
    public Timer timer;  // Reference to the Timer script

    private void Awake() {
        bossesToFight = new List<GameObject>(allBosses);
        ShuffleBossList(bossesToFight); // Randomize the order
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
                float previousMaxHealth = bossScript.max_health; // Store previous health
                bossScript.max_health += currentBossIndex * healthIncreasePerBoss;
                bossScript.current_health = bossScript.max_health; // Set current health to max after increase
                latestSpawnedBoss = newBoss;
                bossScript.reward_min += 0.25f;
                bossScript.reward_max += 2.0f;

                // Start the timer when a new boss is spawned
                timer.Being(30);  // Set timer for 30 seconds (or adjust as needed)

                // Debug log to track health changes
                if (bossScript.max_health > previousMaxHealth)
                {
                    Debug.Log($"Boss {bossPrefab.name} spawned with increased health! Previous: {previousMaxHealth}, New: {bossScript.max_health}");
                }
                else
                {
                    Debug.Log($"Boss {bossPrefab.name} spawned with unchanged health: {bossScript.max_health}");
                }
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

    // Call this function to handle timer end if boss is not defeated
    public void TimerEnded()
    {
        if (latestSpawnedBoss != null)
        {
            // If boss is not defeated when time is up
            if (latestSpawnedBoss.GetComponent<BossScript>().current_health > 0)
            {
                Debug.Log("Timer ended, you lost the fight!");
                PlayerLostFight(); // Handle the lost fight case
            }
        }
    }

    // Track boss defeat and move to the next one
    public void DefeatBoss(GameObject defeatedBoss)
    {
        if (!defeatedBosses.Contains(defeatedBoss))
        {
            defeatedBosses.Add(defeatedBoss); // Add defeated boss to the list
            defeatedBoss.SetActive(false);
            timer.Being(30);  // Reset timer for the next fight
            SpawnNextBoss(); // Spawn the next boss
        }
    }

    // Handle when the player loses a fight
    public void PlayerLostFight()
    {
        if (defeatedBosses.Count > 0)
        {
            // Get the last defeated boss
            GameObject lastDefeatedBoss = defeatedBosses[defeatedBosses.Count - 1];
            defeatedBosses.RemoveAt(defeatedBosses.Count - 1); // Remove from defeated list

            if (lastDefeatedBoss == null)
            {
                Debug.LogError("Last defeated boss has been destroyed and cannot be restored.");
                return;
            }

            // Reactivate and reposition the boss
            lastDefeatedBoss.SetActive(true);
            lastDefeatedBoss.transform.position = transform.position; // Move to spawn location

            // Reset boss stats
            if (lastDefeatedBoss.TryGetComponent(out BossScript bossScript))
            {
                bossScript.current_health = bossScript.max_health; // Restore health
            }

            latestSpawnedBoss = lastDefeatedBoss; // Track as active boss

            // Restart the timer
            timer.Being(30);

            Debug.Log($"Player lost! Returning to previous boss: {lastDefeatedBoss.name}");
        }
        else
        {
            Debug.Log("No previous boss to return to!");
            // Handle restart or game over scenario
        }
    }


}
