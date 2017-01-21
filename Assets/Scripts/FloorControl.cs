using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControl : MonoBehaviour {
    public float speed = 0.05f;    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        moving();
	}

    private void moving()
    {
        //this.transform.Translate(new Vector2(.1f, 0f));
    }
}
