using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum ElementType
{
    Light,
    Void,
    Fire,
    Water,
    Air,
    Earth,
    Plant
}

public class BossScript : MonoBehaviour
{
    [Header("Visual Attachments")]
    public BossSpriteManager sprite_manager;
    [SerializeField] private int sprite_value;

    [Header("Health Settings")]
    public float max_health;
    public float current_health;
    public Slider health_bar;

    [Header("Red Health & Enrage")]
    [Range(0f, 1f)] public float redHealth_percentage = 0.25f; // 25% default
    public bool isEnraged = false;
    public bool isDefeated = false;

    [Header("Reward Settings")]
    public float reward_min;
    public float reward_max;
    private float money_multiplier = 1.0f;

    [Header("Elemental Properties")]
    public ElementType[] boss_elements;
    
    private static readonly float[,] dmg_multipliers = new float[,]
    {
        // Light  Void  Fire  Water  Air  Earth  Plant
        { 1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f }, // Light
        { 0.5f, 1f,   2f,   1f,   2f,   1f,   2f },   // Void
        { 1f,   0.5f, 1f,   0.5f, 2f,   0.5f, 2f },   // Fire
        { 1f,   1f,   2f,   1f,   1f,   2f,   0.5f }, // Water
        { 1f,   0.5f, 0.5f, 1f,   1f,   2f,   1f },   // Air
        { 1f,   1f,   2f,   1f,   0.5f, 1f,   2f },   // Earth
        { 2f,   0.5f, 0.5f, 2f,   1f,   2f,   1f }    // Plant
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite_manager.SetSprite(sprite_value);
        current_health = max_health;
        UpdateHealthBar();
    }

    public void TakeDamage(float base_damage, ElementType attack_element)
    {
        // Calculate average multiplier from all boss elements
        float avgMultiplier = boss_elements.Length > 0
            ? boss_elements.Select(element => GetElementalMultiplier(attack_element, element)).Average()
            : 1f; // Default to 1x if the boss has no elements (shouldn't happen)

        float finalDamage = base_damage * avgMultiplier;

        current_health -= finalDamage;
        current_health = Mathf.Clamp(current_health, 0, max_health);

        UpdateHealthBar();
        CheckEnrageState();

        if (current_health <= 0)
            Die();
    }
    public float CalculateMultiplier()
    {
        money_multiplier = Random.Range(reward_min, reward_max);
        money_multiplier = Mathf.Floor(money_multiplier * 100) / 100f;  // Truncate to 2 decimal places
        return money_multiplier;
    }
    private float GetElementalMultiplier(ElementType attacker, ElementType defender)
    {
        return dmg_multipliers[(int)attacker, (int)defender];
    }

    private void CheckEnrageState()
    {
        if (current_health <= max_health * redHealth_percentage && !isDefeated) isEnraged = true;
    }
    private void UpdateHealthBar()
    {
        if (health_bar != null) health_bar.value = current_health / max_health;
    }
    private void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        Destroy(gameObject);
    }
}
