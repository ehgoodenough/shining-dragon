using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public int OriginalWidth; // TODO fetch this automatically

    private GameObject HealthBarHolder;
    public GameObject HPBarGlowEffectPreFab;

    public bool DebugHealthBar;
    public int DebugForcedHealth;
    public int DebugForcedHealthMax;

    private int StartingX;

    private int LastWidth;

    private RectTransform _RectTransform;
    public List<GlowEffect> LiveGlowEffects;

    private Image _Image;
    private float BestScore;

	// Use this for initialization
	void Start () {
        StartingX = (int)transform.position.x;
        LastWidth = OriginalWidth;
        _RectTransform = GetComponent<RectTransform>();
        HealthBarHolder = transform.parent.gameObject;

        _Image = GetComponent<Image>();
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
            var rightEdge = shift + LastWidth - 1;
            var totalWidth = rightEdge - leftEdge + 1;
            
            var subRightShift = Mathf.FloorToInt(totalWidth / 2.0f);
            
            SpawnGlowEffect(new Vector3(leftEdge + subRightShift, transform.position.y, 0), totalWidth);
        }

        _Image.color = new Color(1, percScale, percScale);
        
        LastWidth = newWidth;
    }

    private void SpawnGlowEffect(Vector3 position, int width)
    {
        var glowEffectGameObj = Instantiate(HPBarGlowEffectPreFab, position, Quaternion.identity);
        glowEffectGameObj.name = $"Glow Effect {position.x}";
        glowEffectGameObj.transform.SetParent(HealthBarHolder.transform , true);

        var rectTrans = glowEffectGameObj.GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(width, rectTrans.sizeDelta.y);

        var glowEffect = glowEffectGameObj.GetComponent<GlowEffect>();

        var score = (float)width;
        if (LiveGlowEffects.Count > 0)
            score += LiveGlowEffects[LiveGlowEffects.Count - 1].ComboMultiplier;
        glowEffect.ComboMultiplier = score;

        if(score > BestScore)
        {
            BestScore = score;
        }

        LiveGlowEffects.Add(glowEffect);
    }

    public void UpdateWidth (int health, int maxHealth)
    {
        if(!DebugHealthBar)
            ImplUpdateWidth(health, maxHealth);
    }
}
