using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStuffSpawner : MonoBehaviour
{
    public GameObject TreePreFab;

	void Start ()
    {
        SpawnTree(0);
	}
	
	void Update ()
    {
		
	}

    void SpawnTree (float x)
    {
        Instantiate(TreePreFab, new Vector2(x, TreePreFab.transform.position.y), Quaternion.identity);
    }
}
