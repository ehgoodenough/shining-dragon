using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStuffSpawner : MonoBehaviour
{
    public GameObject TreePreFab;

    public GameObject Floor;

    private List<GameObject> Environments;
    

	void Start ()
    {
        
        for(int i = 0; i < 7; i++)
        {
            var variance = Random.Range(-1f, 1f);

            SpawnTree(-i * 5 + variance);
        }
	}
	
	void Update ()
    {
		
	}
    
    void SpawnTree (float x)
    {
        SpawnEnvironment(TreePreFab, x);
    }

    void SpawnEnvironment (GameObject prefab, float x)
    {
        var obj = Instantiate(prefab, new Vector2(x, prefab.transform.position.y), Quaternion.identity);
        obj.transform.parent = transform.parent;
    }
}
