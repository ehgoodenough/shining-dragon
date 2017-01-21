using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public int OriginalWidth; // TODO fetch this automatically

    public bool DebugHealthBar;
    public int DebugForcedHealth;
    public int DebugForcedHealthMax;

    private int StartingX;

	// Use this for initialization
	void Start () {
        StartingX = (int)transform.position.x;
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

        var lossOfWidth = OriginalWidth * (1.0f - percScale);
        var leftShift = lossOfWidth / 2;
        transform.localScale = new Vector3(percScale, 1);
        transform.position = new Vector3(StartingX - leftShift, transform.position.y);
    }

    public void UpdateWidth (int health, int maxHealth)
    {
        if(!DebugHealthBar)
            ImplUpdateWidth(health, maxHealth);
    }
}
