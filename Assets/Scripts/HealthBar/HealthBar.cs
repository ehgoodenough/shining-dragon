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
        LastWidth = OriginalWidth;
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

        var newWidth = Mathf.FloorToInt(OriginalWidth * percScale);
        var lossOfWidth = OriginalWidth - newWidth;

        var leftShift = Mathf.CeilToInt(lossOfWidth / 2.0f);
        transform.position = new Vector3(StartingX - leftShift, transform.position.y, transform.position.z);
        _RectTransform.sizeDelta = new Vector2(newWidth, _RectTransform.sizeDelta.y);

        if(newWidth < LastWidth)
        {
            var shift = StartingX - (OriginalWidth / 2);
            var leftEdge = shift + newWidth;
            var rightEdge = shift + LastWidth;
            var totalWidth = rightEdge - leftEdge + 1;

            var subRightShift = Mathf.CeilToInt(totalWidth / 2.0f);

            Debug.Log($"left edge = {leftEdge}, right edge = {rightEdge}, totalWidth = {totalWidth}; subRightShift = {subRightShift}");
            SpawnGlowEffect(new Vector3(leftEdge + subRightShift, transform.position.y, 0), totalWidth);
        }

        LastWidth = newWidth;
    }

    private void SpawnGlowEffect(Vector3 position, int width)
    {
        var glowEffect = Instantiate(HPBarGlowEffectPreFab, position, Quaternion.identity);
        glowEffect.name = $"Glow Effect {position.x}";
        glowEffect.transform.SetParent(HealthBarHolder.transform, true);

        var rectTrans = glowEffect.GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(width, rectTrans.sizeDelta.y);
    }

    public void UpdateWidth (int health, int maxHealth)
    {
        if(!DebugHealthBar)
            ImplUpdateWidth(health, maxHealth);
    }
}
