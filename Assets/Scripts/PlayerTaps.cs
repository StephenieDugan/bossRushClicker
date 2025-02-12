using System.Collections.Generic;
using UnityEngine;

public class PlayerTaps : MonoBehaviour
{
    private Dictionary<ElementType, float> playerElements = new();
    public static PlayerTaps Instance { get; private set; }


    private void Awake()
    {
        ResetElements();
        AddToElement(ElementType.Light, 15f);
        Instance = this;
    }

    public void AddToElement(ElementType element, float damage)
    {
        if (playerElements.ContainsKey(element))
        {
            playerElements[element] += damage; // Increase the existing damage value
        }
        else
        {
            playerElements[element] = damage; // Add a new element
        }
    }

    public Dictionary<ElementType, float> GetTapElements()
    {
        return new Dictionary<ElementType, float>(playerElements);
    }

    public void ResetElements()
    {
        playerElements.Clear();
    }
}
