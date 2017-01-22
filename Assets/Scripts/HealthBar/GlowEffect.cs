using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GlowEffect : MonoBehaviour {
    public float GlowDuration;
    public float GlowDurationRemaining;

    /// <summary>
    /// The "worst" color that the glow effect can be. If there is
    /// </summary>
    public Color WorstColor;

    /// <summary>
    /// The "best" color that the glow effect can be
    /// </summary>
    public Color BestColor;

    /// <summary>
    /// This is multiplied by seconds to go through a sine wave for modifying
    /// redness by the redness modifier. If the color glow speed is 0 the redness
    /// is not effected
    /// </summary>
    public float ColorGlowSpeed;

    /// <summary>
    /// The maximum shift in color throughout the color glow speed. Highest value
    /// is 1. At 1 the color will alternate from the best color to the worst color
    /// at the color glow speed.
    /// </summary>
    public float ColorModifier;

    private Image _Image;

    private HealthBar _HealthBar;

	// Use this for initialization
	void Start () {
        GlowDurationRemaining = GlowDuration;
        _Image = GetComponent<Image>();
        _HealthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
    }
	
	// Update is called once per frame
	void Update () {
        GlowDurationRemaining -= Time.deltaTime;
        
        if(GlowDurationRemaining <= 0)
        {
            _HealthBar.LiveGlowEffects.Remove(this);
            Destroy(gameObject);
        }else
        {
            var current = GlowDuration - GlowDurationRemaining;
            var percToDone = current / GlowDuration;

            if (GlowDuration <= 0.5f)
                _Image.color = DecideColorAlphaFade(percToDone);
            else
            {
                _Image.color = DecideColorAlternateFade(current, 0, GlowDuration, WorstColor, BestColor);
                Debug.Log($"color = {_Image.color}");
            }
                
        }
	}

    Color DecideColorAlphaFade(float percProgress)
    {
        return new Color(_Image.color.r, _Image.color.g, _Image.color.b, 1.0f - percProgress);
    }

    Color DecideColorAlternateFade(float current, float initial, float final, Color color1, Color color2)
    {
        Debug.Log($"Fade({current}, {current}, {final}, {color1}, {color2})");
        var sine = Mathf.Sin(current * ColorGlowSpeed);

        return new Color(
            color1.r + (color2.r - color1.r) * sine,
            color1.g + (color2.g - color1.g) * sine,
            color1.b + (color2.b - color1.b) * sine,
            color1.a + (color2.a - color1.a) * sine
            );
    }
}
