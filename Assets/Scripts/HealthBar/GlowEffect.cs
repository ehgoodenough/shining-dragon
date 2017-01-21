using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GlowEffect : MonoBehaviour {
    public float GlowDuration;
    public float GlowDurationRemaining;

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
            
            _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, 1.0f - percToDone);
        }
	}
}
