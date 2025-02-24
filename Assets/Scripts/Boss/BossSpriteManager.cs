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
	[SerializeField] TextMeshProUGUI elementType;
	public bool clickable;
	private void Awake() {
		instance = this;
	}

    public void TapBoss() {
		if (clickable) {
			spriteRenderer.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			//force on googly eyes
			googlyEyes[0].ApplyRandomForce();
			googlyEyes[1].ApplyRandomForce();
		}
	}

	public void Update() {
		spriteRenderer.gameObject.transform.localScale = math.lerp(spriteRenderer.gameObject.transform.localScale, Vector3.one, Time.deltaTime);
	}

	// This function can be called to change the sprite based on a boss type or selection
	public void SetSprite(int index, ElementType[] types)
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
		string Elements = "";
		foreach (ElementType type in types) {
			string element = "<sprite name=Light>";
			switch (type) {
				default:
				case ElementType.Light:
					element = "<sprite name=Light> ";
					break;
				case ElementType.Void:
					element = "<sprite name=Void> ";
					break;
				case ElementType.Fire:
					element = "<sprite name=Fire> ";
					break;
				case ElementType.Water:
					element = "<sprite name=Water>";
					break;
				case ElementType.Air:
					element = "<sprite name=Air>";
					break;
				case ElementType.Earth:
					element = "<sprite name=Earth>";
					break;
				case ElementType.Plant:
					element = "<sprite name=Plant>";
					break;
			}

			Elements += element;
		}

		elementType.text = Elements;
	}

	public void spawnDamageText(ElementType type, float damage) {
        string element = "<sprite name=Light>";
		switch (type) {
			default:
			case ElementType.Light:
				element = "<sprite name=Light> ";
				break;
			case ElementType.Void:
				element = "<sprite name=Void> ";
				break;
			case ElementType.Fire:
				element = "<sprite name=Fire> ";
				break;
			case ElementType.Water:
				element = "<sprite name=Water>";
				break;
			case ElementType.Air:
				element = "<sprite name=Air>";
				break;
			case ElementType.Earth:
				element = "<sprite name=Earth>";
				break;
			case ElementType.Plant:
				element = "<sprite name=Plant>";
				break;
		}
		GameObject textGO = Instantiate(floatingTextPrefab, spriteRenderer.gameObject.transform);
        textGO.transform.position = Input.mousePosition;
		textGO.GetComponentInChildren<TextMeshProUGUI>().text = element + damage.ToString();
		Destroy(textGO, 1);
	}
}
