using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Button Open;
    [SerializeField] private Button Close;
    [SerializeField] private GameObject Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event EventHandler OnButtonPress;
    //private void Awake() {
    //PlayButton.onClick.AddListener(() => {
    //OnButtonPress?.Invoke(this, EventArgs.Empty);
    //});
    //}
    void Start() {
        Open.onClick.AddListener(() => {
            Menu.SetActive(true);
            OnButtonPress?.Invoke(this, EventArgs.Empty);
        });
		Close.onClick.AddListener(() => {
			Menu.SetActive(false);
            OnButtonPress?.Invoke(this, EventArgs.Empty);
        });
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
