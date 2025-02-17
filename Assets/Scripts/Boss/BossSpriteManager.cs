using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BossSpriteManager : MonoBehaviour
{
    [Header("Boss Sprite")]
    public Image spriteRenderer;
    public Sprite[] bossSprites; // Array of sprites for different bosses
    public static BossSpriteManager instance {get; private set;}

	private void Awake() {
		instance = this;
	}

    public void TapBoss() {
        spriteRenderer.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //force on googly eyes

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
        }
        else
        {
            Debug.LogWarning("Invalid sprite index! No sprite set.");
        }
    }
}
