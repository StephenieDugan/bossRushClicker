using UnityEngine;

public class googlyEye : MonoBehaviour {
	public float forceAmount = 5f;
	[SerializeField] Rigidbody2D pupil;

	public void ApplyRandomForce() {
		Rigidbody2D rb = pupil;
		if (rb != null) {
			Vector2 randomDirection = Random.insideUnitCircle.normalized; // Get a random direction
			rb.AddForce(randomDirection * forceAmount, ForceMode2D.Impulse);
		} else {
			Debug.LogWarning("No Rigidbody2D found on " + gameObject.name);
		}
	}
}
