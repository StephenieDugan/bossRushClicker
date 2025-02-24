using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    List<MapLevel> levels;

    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject content;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        levels = new List<MapLevel>();
    }

    // Update is called once per frame
    void Update() {
        if (levels.Count < BossManagerScript.instance.defeatedBosses.Count + 1) { // if more bosses defeated then recoreded
            GameObject go = Instantiate(levelPrefab, content.transform);
            MapLevel ml = go.GetComponent<MapLevel>();
            levels.Add(ml);
            ml.UpdateLevel(levels.Count);
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100 + ((levels.Count - 1) * 150), 0);
    }
}
