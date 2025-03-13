using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundsSO : ScriptableObject {
    public AudioClip buttonPress;
    public float buttonPressVolume = .6f;
    public AudioClip boss;
    public float BossVolume = .6f;
}
