using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GlowEffect : MonoBehaviour {
    public float GlowDuration;

    private float RealGlowDuration;
    private float RealGlowDurationRemaining;

    public float ComboMultiplier;

    public float Progress
    {
        get
        {
            return (RealGlowDuration - RealGlowDurationRemaining) / RealGlowDuration;
        }
    }

    public Color BetterColor;
    public float BetterDuration;
    public Color EvenBetterColor;
    public float EvenBetterDuration;
    public Color BestColor;
    public float BestDuration;
    
    private SpriteRenderer _Sprite;

    private HealthBar _HealthBar;

    private Color BaseColor;

	// Use this for initialization
	void Start () {
        _Sprite = GetComponent<SpriteRenderer>();
        _HealthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        if (ComboMultiplier < 20)
        {
            RealGlowDuration = GlowDuration;
            BaseColor = _Sprite.color;
        }
        else if (ComboMultiplier < 50)
        {
            RealGlowDuration = BetterDuration;
            BaseColor = BetterColor;
        }
        else if (ComboMultiplier < 100)
        {
            RealGlowDuration = EvenBetterDuration;
            BaseColor = EvenBetterColor;
        }
        else 
        {
            RealGlowDuration = BestDuration;
            BaseColor = BestColor;
        }

        RealGlowDurationRemaining = RealGlowDuration;
        var yScale = 1.0f + ComboMultiplier / 50.0f;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * yScale, transform.localScale.z);
    }
	
	// Update is called once per frame
	void Update () {
        RealGlowDurationRemaining -= Time.deltaTime;
        
        if(RealGlowDurationRemaining <= 0)
        {
            _HealthBar.LiveGlowEffects.Remove(this);
            Destroy(gameObject);
        }else
        {
            var current = RealGlowDuration - RealGlowDurationRemaining;
            var percToDone = current / RealGlowDuration;

            _Sprite.color = ColorAlphaFade(BaseColor, percToDone);
        }
	}

    Color ColorAlphaFade(Color baseColor, float percProgress)
    {
        return new Color(baseColor.r, baseColor.g, baseColor.b, 1.0f - percProgress);
    }
}
