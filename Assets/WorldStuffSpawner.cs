using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStuffSpawner : MonoBehaviour
{
    public GameObject TreePreFab;
    public GameObject CloudsPreFab;

	void Start ()
    {
        SpawnTree(0);
        SpawnClouds(0); 
	}
	
	void Update ()
    {
		
	}

    void SpawnTree (float x)
    {
        SpawnEnvironment(TreePreFab, x);
    }

    void SpawnClouds (float x)
    {
        SpawnEnvironment(CloudsPreFab, x);
    }

    void SpawnEnvironment (GameObject prefab, float x)
    {
        var obj = Instantiate(prefab, new Vector2(x, prefab.transform.position.y), Quaternion.identity);
        obj.transform.parent = transform.parent;
    }
}
