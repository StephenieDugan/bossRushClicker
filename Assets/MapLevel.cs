using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLevel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event EventHandler OnButtonPress;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
        button.onClick.AddListener(() => {
            BossManagerScript.instance.changeBoss(level);
            OnButtonPress?.Invoke(this, EventArgs.Empty);
        });
    }
}
