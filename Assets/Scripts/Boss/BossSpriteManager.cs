using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BossSpriteManager : MonoBehaviour {
    [Header("Boss Sprite")]
    public Image spriteRenderer;
    public Sprite[] bossSprites; // Array of sprites for different bosses
    public static BossSpriteManager instance { get; private set; }
    [SerializeField] public List<Vector2> GooglyEyePositions;
    [SerializeField] public List<googlyEye> googlyEyes;
	[SerializeField] GameObject floatingTextPrefab;

	private void Awake() {
		instance = this;
	}

    public void TapBoss() {
        spriteRenderer.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //force on googly eyes
        googlyEyes[0].ApplyRandomForce();
        googlyEyes[1].ApplyRandomForce();

	}

	public void Update() {
		spriteRenderer.gameObject.transform.localScale = math.lerp(spriteRenderer.gameObject.transform.localScale, Vector3.one, Time.deltaTime);
	}

	// This function can be called to change the sprite based on a boss type or selection
	public void SetSprite(int index)
    {
        if (index >= 0 && index < bossSprites.Length)
        {
            spriteRenderer.sprite = bossSprites[index];
            googlyEyes[0].transform.localPosition = GooglyEyePositions[index * 2];
            googlyEyes[1].transform.localPosition = GooglyEyePositions[(index * 2)+1];
        }
        else
        {
            Debug.LogWarning("Invalid sprite index! No sprite set.");
        }
    }

	public void spawnDamageText(ElementType type, float damage) {
		GameObject textGO = Instantiate(floatingTextPrefab, spriteRenderer.gameObject.transform);
        textGO.transform.position = Input.mousePosition;
		textGO.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
		Destroy(textGO, 1);
	}
}
