using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public int OriginalWidth; // TODO fetch this automatically

    public GameObject HealthBarHolder;
    public GameObject HPBarGlowEffectPreFab;

    public bool DebugHealthBar;
    public int DebugForcedHealth;
    public int DebugForcedHealthMax;

    private int StartingX;

    private int LastWidth;

    private RectTransform _RectTransform;

	// Use this for initialization
	void Start () {
        StartingX = (int)transform.position.x;
        _RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
        if(DebugHealthBar)
        {
            ImplUpdateWidth(DebugForcedHealth, DebugForcedHealthMax);
        }
	}

    private void ImplUpdateWidth(int health, int maxHealth)
    {
        float percScale = health / (float)maxHealth;

        var newWidth = Mathf.RoundToInt(OriginalWidth * percScale);
        var lossOfWidth = OriginalWidth - newWidth;

        if(lossOfWidth % 2 != 0)
        {
            return;
        }

        var leftShift = (int)(lossOfWidth / 2);
        transform.position = new Vector3(StartingX - leftShift, transform.position.y, transform.position.z);
        _RectTransform.sizeDelta = new Vector2(newWidth, _RectTransform.sizeDelta.y);

        if(newWidth < LastWidth)
        {
            for(int offset = newWidth + 1; offset <= LastWidth; offset++)
            {
                var x = StartingX - (OriginalWidth / 2) + offset - 1;
                SpawnGlowEffect(new Vector3(x, transform.position.y, 0));
            }
        }

        LastWidth = newWidth;
    }

    private void SpawnGlowEffect(Vector3 position)
    {
        var glowEffect = Instantiate(HPBarGlowEffectPreFab, position, Quaternion.identity);
        glowEffect.name = $"Glow Effect {position.x}";
        glowEffect.transform.SetParent(HealthBarHolder.transform, true);
    }

    public void UpdateWidth (int health, int maxHealth)
    {
        if(!DebugHealthBar)
            ImplUpdateWidth(health, maxHealth);
    }
}
