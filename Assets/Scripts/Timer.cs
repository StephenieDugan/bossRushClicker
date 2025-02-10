using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Timer : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Pause = !Pause;
    }

    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private BossManagerScript boss_manager;

    public int Duration;

    private float remainingTime;
    private bool Pause;

    private void Start()
    {
        Begin(Duration);
    }

    public void Begin(int seconds)
    {
        remainingTime = seconds;
    }

    private void Update()
    {
        if (!Pause && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            int displayTime = Mathf.Max(Mathf.FloorToInt(remainingTime), 0);
            uiText.text = displayTime.ToString();
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingTime);

            if (remainingTime <= 0)
            {
                OnEnd();
            }
        }
    }

    private void OnEnd()
    {
        Debug.Log("End");
        boss_manager.TimerEnded();
    }
}
