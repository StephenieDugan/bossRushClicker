using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClickerScript : MonoBehaviour
{ 
    //PLAYER
    [SerializeField] TMP_Text money_text;
    private float money_amount;
    readonly private float click_damage = 10.0f;

    // Reference to the BossManager and BossScript
    [SerializeField] BossManagerScript boss_manager;
    private BossScript current_Boss;

	float[] currentDamagePerSec = { 0, 0, 0, 0, 0, 0, 0 };// Light, Void, Fire, Water, Air, Earth, Plant


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        // Find the BossManager in the scene
        boss_manager = Object.FindFirstObjectByType<BossManagerScript>();

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
            current_Boss.health_bar.value = current_Boss.current_health / current_Boss.max_health;
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
            current_Boss.TakeDamage(click_damage, ElementType.Fire); // Example element: Fire
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
        boss_manager.DefeatBoss(current_Boss.gameObject); // Register boss defeat
        SpawnNextBoss();  // Spawn the next boss
    }

    // Method to spawn the next boss from the BossManager
    private void SpawnNextBoss()
    {
        if (boss_manager != null)
        {
            boss_manager.SpawnNextBoss();  // Trigger the spawn of the next boss

            // Explicitly track the new boss after spawning
            GameObject newBoss = boss_manager.GetLatestSpawnedBoss();
            current_Boss = newBoss.GetComponent<BossScript>(); // Get the BossScript from the new boss

            if (current_Boss != null)
            {
                //current_Boss.health_bar.maxValue = current_Boss.max_health;  // Set the new boss's health bar max value
                current_Boss.health_bar.value = current_Boss.current_health/current_Boss.max_health;  // Set the initial health value
            }
            else Debug.LogError("Newly spawned boss does not have a BossScript attached.");
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
