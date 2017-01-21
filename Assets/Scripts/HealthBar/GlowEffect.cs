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

	// Use this for initialization
	void Start () {
        GlowDurationRemaining = GlowDuration;
        _Image = GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update () {
        GlowDurationRemaining -= Time.deltaTime;
        
        if(GlowDurationRemaining <= 0)
        {
            Destroy(gameObject);
        }else
        {
            var percToDone = (GlowDuration - GlowDurationRemaining) / GlowDuration;

            if (ColorModifier == 0 || ColorGlowSpeed == 0)
                _Image.color = DecideColorAlphaFade(percToDone);
            else
                _Image.color = DecideColorAlternateFade(percToDone, 0, GlowDuration, WorstColor, BestColor);
        }
	}

    Color DecideColorAlphaFade(float progress)
    {
        return new Color(_Image.color.r, _Image.color.g, _Image.color.b, 1.0f - progress);
    }

    Color DecideColorAlternateFade(float progress, float initial, float final, Color color1, Color color2)
    {
        var sine = Mathf.Sin(initial * ColorGlowSpeed);

        return new Color(
            
            );
    }
}
