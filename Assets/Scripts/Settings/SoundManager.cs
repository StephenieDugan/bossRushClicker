using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance { get; private set; }
	[SerializeField] private SoundsSO sounds;
	private float volume = 1f;

	private void Awake() {
		instance = this;
	}

	private void Start() {
		//MainMenuUI.OnButtonPress += OnButtonPress;
		SettingsMenuManager.OnButtonPress += OnButtonPress;
		MapLevel.OnButtonPress += OnButtonPress;
		UpgradePanel.OnbuttonPress += OnButtonPress;
		Upgrade.OnbuttonPress += OnButtonPress;
		ClickerScript.onBossTap += onBossTap;
	}

	//example of what MainMenuUI.OnButtonPress
	// public static event EventHandler OnButtonPress;
	//private void Awake() {
		//PlayButton.onClick.AddListener(() => {
			//OnButtonPress?.Invoke(this, EventArgs.Empty);
		//});
	//}

		//play sounds
	private void OnButtonPress(object sender, System.EventArgs e) {
		PlaySound(sounds.buttonPress, Camera.allCameras[0].transform.position, sounds.buttonPressVolume);
	}
    private void onBossTap(object sender, System.EventArgs e)
    {
        PlaySound(sounds.boss, Camera.allCameras[0].transform.position, sounds.BossVolume);
    }

    //sound manager functions
    private void PlaySound(AudioClip audio, Vector3 position, float VolumeMultiplier = 1f) {
		AudioSource.PlayClipAtPoint(audio, position, VolumeMultiplier * volume);
	}

	public void ChangeVolume(float newVolume) {
		volume = newVolume;
	}

	public float GetVolume() {
		return volume;
	}

	private void OnDestroy() {
		//MainMenuUI.OnButtonPress -= OnButtonPress;
	}
}
