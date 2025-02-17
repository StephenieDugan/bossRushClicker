using System.Collections.Generic;
using UnityEngine;

public class BossManagerScript : MonoBehaviour
{
    public static BossManagerScript instance { get; private set; }

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
        instance = this;
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
        if (latestSpawnedBoss != null)
        {
            Debug.Log("A boss is already active. No need to spawn a new one.");
            return;
        }

        if (currentBossIndex < bossesToFight.Count)
        {
            GameObject bossPrefab = bossesToFight[currentBossIndex];
            GameObject newBoss = Instantiate(bossPrefab, transform.position, Quaternion.identity);

            if (newBoss.TryGetComponent(out BossScript bossScript))
            {
                bossScript.startBoss();
                bossScript.max_health += currentBossIndex * healthIncreasePerBoss;
                bossScript.current_health = bossScript.max_health;
                latestSpawnedBoss = newBoss;
                bossScript.reward_min += 0.25f;
                bossScript.reward_max += 2.0f;

                timer.Begin(30);

                Debug.Log($"Boss {bossPrefab.name} spawned with health {bossScript.max_health}");
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
    public void DefeatBoss(GameObject defeatedBoss)
    {
        if (!defeatedBosses.Contains(defeatedBoss))
        {
            defeatedBosses.Add(defeatedBoss);
            defeatedBoss.SetActive(false); // Hide the boss instead of destroying
            latestSpawnedBoss = null; // Clear reference to avoid accidental reactivation

            timer.Begin(30); // Reset the timer
            //SpawnNextBoss(); // Spawn the next boss
        }
    }

    // Handle when the player loses a fight
    public void PlayerLostFight()
    {
        if (defeatedBosses.Count > 0)
        {
            GameObject lastDefeatedBoss = defeatedBosses[defeatedBosses.Count - 1];
            defeatedBosses.RemoveAt(defeatedBosses.Count - 1);

            if (lastDefeatedBoss == null)
            {
                Debug.LogError("Last defeated boss has been destroyed and cannot be restored.");
                return;
            }

            // Reactivate and reposition
            lastDefeatedBoss.SetActive(true);
            lastDefeatedBoss.transform.position = transform.position;

            // Reset boss stats
            if (lastDefeatedBoss.TryGetComponent(out BossScript bossScript))
            {
                bossScript.startBoss();
                ClickerScript.instance.current_Boss = bossScript;
                bossScript.current_health = bossScript.max_health; // Restore health
            }
            latestSpawnedBoss.SetActive(false);
            latestSpawnedBoss = lastDefeatedBoss; // Ensure proper tracking
            currentBossIndex = Mathf.Max(0, currentBossIndex - 1); // Ensure we revert boss progression

            // Restart the timer
            timer.Begin(30);

            Debug.Log($"Player lost! Returning to previous boss: {lastDefeatedBoss.name}");
        }
        else
        {
            Debug.Log("No previous boss to return to!");
            // Handle restart or game over
        }
    }

    public void changeBoss(int level) {
        if (level - 1 >= defeatedBosses.Count) {// if trying to return to most recent boss not defeated yet

			latestSpawnedBoss.SetActive(false);
			currentBossIndex = Mathf.Max(0, level - 1); // Ensure we revert boss progression
            latestSpawnedBoss = null;
            SpawnNextBoss();
            return;
		}
        defeatedBosses[level-1].SetActive(true); 
        if (defeatedBosses[level-1].TryGetComponent(out BossScript bossScript)) {
			bossScript.startBoss();
			ClickerScript.instance.current_Boss = bossScript;
			bossScript.current_health = bossScript.max_health; // Restore health
		}
		latestSpawnedBoss.SetActive(false);
		latestSpawnedBoss = defeatedBosses[level-1]; // Ensure proper tracking
		currentBossIndex = Mathf.Max(0, level - 1); // Ensure we revert boss progression
	}

}
