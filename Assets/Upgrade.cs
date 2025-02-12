using TMPro;
using UnityEngine;

public class Upgrade : MonoBehaviour {
	[SerializeField] TextMeshProUGUI costText;

	[SerializeField] float CurrentCost;
	[SerializeField] float CostIncrease;
	[SerializeField] ElementType type = ElementType.Light;
	[SerializeField] float DamageIncrease = 1;
	[SerializeField] bool IsItTapPower = true;// if false it will do ally damage instead

	private void Start() {
		costText.text = "$" + CurrentCost.ToString();
	}

	public void Purchase() {
		float currentMoney = ClickerScript.instance.money_amount;
		if (currentMoney > CurrentCost) {
			ClickerScript.instance.money_amount = currentMoney - CurrentCost;

			ClickerScript.instance.money_text.text = ClickerScript.instance.money_amount.ToString();
			CurrentCost += CostIncrease;
			costText.text = "$" + CurrentCost.ToString();
			if (IsItTapPower) {
				PlayerTaps.Instance.AddToElement(type, DamageIncrease);
			} else {
				ClickerScript.instance.upgradeAllies(DamageIncrease, type);
			}
		}
	}
}
