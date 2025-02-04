using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClickerScript : MonoBehaviour
{
    //BOSS
    [SerializeField] float boss_current_health;
    [SerializeField] float boss_max_health = 100;
    [SerializeField] Slider boss_healthbar;
    [SerializeField]  float money_mult;

    //PLAYER
    [SerializeField] TMP_Text money_text;
    private float money_amount;
    private float click_damage = 10.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss_current_health = boss_max_health;
        boss_healthbar.value = boss_current_health;

        if (PlayerPrefs.HasKey("money")) 
        {
            money_amount = PlayerPrefs.GetFloat("money");
            money_text.text = money_amount.ToString();
        }
       ResetMoneyAmount(); /*Used For Testing*/
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMultiplier();
    }


    //public void SetBossHealth(float health)
    //{
    //    boss_healthbar.value = health;
    //}
    public void HurtBoss() 
    {
        boss_current_health -= click_damage;
        boss_healthbar.value = boss_current_health;
    }

    public void EarnPoints() 
    {
        EarnPoints(money_mult);
    }

    private void EarnPoints(float multiplier = 1) 
    {
        money_amount += multiplier;
        PlayerPrefs.SetFloat("money", money_amount);

        money_text.text = money_amount.ToString();
    }
  
    private float CalculateMultiplier()
    {
        money_mult = Random.Range(0.25f, 10.0f);
        money_mult = Mathf.Floor(money_mult * 100) / 100f;  // Truncate to 2 decimal places
        return money_mult;
    }

    public void ResetMoneyAmount()
    {
        money_amount = 0f;  // Reset the money amount to 0
        PlayerPrefs.SetFloat("money", money_amount);  // Save the reset value to PlayerPrefs
        money_text.text = money_amount.ToString();  // Update the UI with the reset value
    }
}
