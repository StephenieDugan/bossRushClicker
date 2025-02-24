using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClickerScript : MonoBehaviour
{ 
    //PLAYER
    [SerializeField] public TMP_Text money_text;
    public float money_amount;

	public Slider health_bar;
	public static ClickerScript instance { get; private set; } 

    // Reference to the BossManager and BossScript
    [SerializeField] BossManagerScript boss_manager;
    public BossScript current_Boss;
    private PlayerTaps playerTaps;

    public bool movingOn = true; //asks if we move to the next boss or restart this one.

	float[] currentDamagePerSec = { 0, 0, 0, 0, 0, 0, 0 };// Light, Void, Fire, Water, Air, Earth, Plant

	private void Awake() {
		instance = this;
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        // Find the BossManager in the scene
        boss_manager = Object.FindFirstObjectByType<BossManagerScript>();
        playerTaps = Object.FindFirstObjectByType<PlayerTaps>();

        if (PlayerPrefs.HasKey("money")) 
        {
            money_amount = PlayerPrefs.GetFloat("money");
            money_text.text = money_amount.ToString();
        }
       ResetMoneyAmount(); /*Used For Testing*/
       SpawnNextBoss();
    }

    // Update is called once per frame
    void Update() {
        // Update boss health and money multiplier
        if (current_Boss != null) {
            health_bar.value = current_Boss.current_health / current_Boss.max_health;
        }
        if (current_Boss != null) {
            for (int i = 0; i < currentDamagePerSec.Length; i++) {
                current_Boss.TakeDamage(currentDamagePerSec[i] * Time.deltaTime, (ElementType)i);
            }
        }
    }

    public void HurtBoss() 
    {
        if (current_Boss != null)
        {
            // Retrieve player's elemental damage values
            Dictionary<ElementType, float> playerElements = playerTaps.GetTapElements();

            // Apply damage using the new TakeDamage method
            current_Boss.TakeDamage(playerElements);
            if (current_Boss.current_health <= 0)
            {
                // Boss defeated
                DefeatCurrentBoss();
            }
        }
    }
    // Method to handle the boss defeat and spawn the next one
    private void DefeatCurrentBoss()
    {
        // Reward player and spawn the next boss
        EarnPoints(current_Boss.CalculateMultiplier());
        if (movingOn) {
        boss_manager.DefeatBoss(current_Boss.gameObject); // Register boss defeat
            SpawnNextBoss();
            // Spawn the next boss
        } else {
            RespawnThisBoss();
        }
    }

    private void RespawnThisBoss() {

		GameObject newBoss = boss_manager.GetLatestSpawnedBoss();
        boss_manager.respawnBoss();

		if (current_Boss != null) {
			current_Boss.startBoss();
			//current_Boss.health_bar.maxValue = current_Boss.max_health;  // Set the new boss's health bar max value
			health_bar.value = current_Boss.current_health / current_Boss.max_health;  // Set the initial health value
		}
	}

    // Method to spawn the next boss from the BossManager
    private void SpawnNextBoss()
    {
        if (boss_manager != null)
        {
            boss_manager.SpawnNextBoss();  // Trigger the spawn of the next boss

            if (boss_manager.currentBossIndex < boss_manager.bossesToFight.Count) 
            {
                // Explicitly track the new boss after spawning
                GameObject newBoss = boss_manager.GetLatestSpawnedBoss();
                current_Boss = newBoss.GetComponent<BossScript>(); // Get the BossScript from the new boss

                if (current_Boss != null)
                {
                    current_Boss.startBoss();
                    //current_Boss.health_bar.maxValue = current_Boss.max_health;  // Set the new boss's health bar max value
                    health_bar.value = current_Boss.current_health / current_Boss.max_health;  // Set the initial health value
                }
                else Debug.LogError("Newly spawned boss does not have a BossScript attached.");

            }
            Debug.Log("Win Screen");
        }
    }

    public void upgradeAllies(float damage, ElementType type) {
        currentDamagePerSec[(int)type] += damage;
    }

    private void EarnPoints(float multiplier = 1) 
    {
        money_amount += multiplier;
        PlayerPrefs.SetFloat("money", money_amount);
        money_text.text = money_amount.ToString();
    }

    public void ResetMoneyAmount()
    {
        money_amount = 0f;  // Reset the money amount to 0
        PlayerPrefs.SetFloat("money", money_amount);  // Save the reset value to PlayerPrefs
        money_text.text = money_amount.ToString();  // Update the UI with the reset value
    }
}
