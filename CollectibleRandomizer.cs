using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleRandomizer : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    private List<GameObject> locations = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            locations.Add(transform.GetChild(i).gameObject);
        }
        SceneManager.sceneLoaded += RandomizeCollectibleLocations;
    }
    
    private void RandomizeCollectibleLocations(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("Real_Level_1")) return;

        var wrtPrfbs = new List<GameObject>(collectiblePrefabs);
        var wrtLocs = new List<GameObject>(locations);
        if (wrtPrfbs.Count != wrtLocs.Count)
        {
            return;
        }

        while (wrtPrfbs.Count > 0)
        {
            var randomPrefab = wrtPrfbs[new System.Random().Next(0, wrtPrfbs.Count)];
            var randomLocation = wrtLocs[new System.Random().Next(0, wrtLocs.Count)];
            var newThing = Instantiate(randomPrefab, randomLocation.transform.position, randomLocation.transform.rotation);
            wrtPrfbs.Remove(randomPrefab);
            wrtLocs.Remove(randomLocation);
            Debug.Log($"Instantiated new collectible {newThing.name} at {newThing.transform.position}, {wrtPrfbs.Count} collectibles remaining");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
}
